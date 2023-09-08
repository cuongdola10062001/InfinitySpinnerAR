using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SpinningTopsGameManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public GameObject ui_InformPanel;
    public TextMeshProUGUI ui_InformText;
    public GameObject searchForGameButton;


    // Start is called before the first frame update
    void Start()
    {
        ui_InformPanel.SetActive(true);
        ui_InformText.text = "Search For Games to BATTLE!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region UI Callback Methods
    public void JoinRandomRoom()
    {
        ui_InformText.text = "Searching for available rooms...";

        PhotonNetwork.JoinRandomRoom();

        searchForGameButton.SetActive(false);
    }

    #endregion

    #region PHOTON Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ui_InformText.text = message;
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount ==1)
        {
            ui_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + ". Waiting for other players ...";

        } else
        {
            ui_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(ui_InformPanel, 2f));

        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ui_InformText.text = newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount;

        StartCoroutine(DeactivateAfterSeconds(ui_InformPanel, 2f));
    }


    #endregion

    #region Private Methods
    void CreateAndJoinRoom()
    {
        string randomRoomName = "Room "+Random.Range(1,1000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    IEnumerator DeactivateAfterSeconds(GameObject gameObject,float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }

    #endregion
}
