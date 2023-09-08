using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    public TextMeshProUGUI playerName;

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            // The player is local player
            transform.GetComponent<MovementController>().enabled = true;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);
        } else
        {
            // The player is remote player
            transform.GetComponent<MovementController>().enabled = false;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);
        }
        SetPlayerName();
    }
    
    void SetPlayerName()
    {
        if(playerName != null)
        {
            if (photonView.IsMine)
            {
                playerName.text = "YOU";
                playerName.color = Color.red;

            }
            else
            {
                playerName.text = photonView.Owner.NickName;
            }

        }
    }
}
