using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MySynchronization : MonoBehaviour, IPunObservable
{
    Rigidbody m_rb;
    PhotonView _photonView;

    Vector3 networkedPosition;
    Quaternion networkedRotation;

    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocity = true;
    public bool isTeleprotEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;

    private float _distance;
    private float _angle;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();

        networkedPosition = new Vector3();
        networkedRotation = new Quaternion();
    }

    private void FixedUpdate()
    {
        if(!_photonView.IsMine)
        {
            m_rb.position = Vector3.MoveTowards(m_rb.position, networkedPosition, _distance * (1.0f / PhotonNetwork.SerializationRate));
            m_rb.rotation = Quaternion.RotateTowards(m_rb.rotation, networkedRotation, _angle * (1.0f / PhotonNetwork.SerializationRate));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            // Then, photoView is mine and i am the one who controls this player
            // Should send position, velocity etc. data to the other players
            stream.SendNext(m_rb.position);
            stream.SendNext(m_rb.rotation);

            if(synchronizeVelocity)
            {
                stream.SendNext(m_rb.velocity);
            }
            if(synchronizeAngularVelocity)
            {
                stream.SendNext(m_rb.angularVelocity);
            }
        }
        else
        {
            // Called on my player gameobject that exists in remote player's game
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();

            if (isTeleprotEnabled)
            {
                if (Vector3.Distance(m_rb.position, networkedPosition) > teleportIfDistanceGreaterThan)
                {
                    m_rb.position = networkedPosition;
                }
            }

            if (synchronizeVelocity || synchronizeAngularVelocity)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (synchronizeVelocity)
                {
                    m_rb.velocity = (Vector3)stream.ReceiveNext();

                    networkedPosition += m_rb.velocity * lag;

                    _distance = Vector3.Distance(m_rb.position, networkedPosition);
                }

                if (synchronizeAngularVelocity)
                {
                    m_rb.angularVelocity = (Vector3)stream.ReceiveNext();

                    networkedRotation = Quaternion.Euler(m_rb.angularVelocity * lag) * networkedRotation;

                    _distance = Vector3.Distance(m_rb.position, networkedPosition);

                    _angle = Quaternion.Angle(m_rb.rotation, networkedRotation);
                }
            }

        }
    }
}
