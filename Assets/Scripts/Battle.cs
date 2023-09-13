using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Battle : MonoBehaviour
{
    public Spinner spinner;

    private float startSpinSpeed;
    private float currentSpinSpeed;
    public Image spinSpeedBar_Image;
    public TextMeshProUGUI spinSpeedRatio_Txt;

    private void Awake()
    {
        startSpinSpeed = spinner.spinSpeed;
        currentSpinSpeed = spinner.spinSpeed;
        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            if(mySpeed > otherPlayerSpeed)
            {
                Debug.Log("You damage the other player");

                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    // Apply Damage to the slower player
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, 400f);
                }
            } else
            {
                Debug.Log("You get damage");
            }
        }
    }

    [PunRPC]
    public void DoDamage(float damageAmount)
    {
        spinner.spinSpeed -= damageAmount;
        currentSpinSpeed = spinner.spinSpeed;
        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
        spinSpeedRatio_Txt.text = currentSpinSpeed + "/"+ startSpinSpeed;

    }

    // Start is called before the first frame update
    void Start()
    {
                                                             
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
