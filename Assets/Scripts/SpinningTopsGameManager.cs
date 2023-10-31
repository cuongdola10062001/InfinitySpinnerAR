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
    public GameObject adjust_Button;
    public GameObject raycastCenter_Img;


    // Start is called before the first frame update
    void Start()
    {
        ui_InformPanel.SetActive(true);
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

    public void OnQuitMatchButtonClicked()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneLoader.Ins.LoadScene(NameScene.Scene_Lobby.ToString());
        }
    }

    #endregion

    #region PHOTON Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ui_InformText.text = message;
        CreateAndJoinRoom();
    }

 /*   public override void  Room()*/
    public void  Room()
    {
        adjust_Button.SetActive(false);
        raycastCenter_Img.SetActive(false);

        if (PhotonNetwork.CurrentRoom.PlayerCount ==1)
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

    public override void OnLeftRoom()
    {
        SceneLoader.Ins.LoadScene(NameScene.Scene_Lobby.ToString());
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
