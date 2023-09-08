using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInputField;
    public GameObject ui_Login;


    [Header("Login UI")]
    public GameObject ui_Lobby;
    public GameObject ui_3D;

    [Header("Connected Status UI")]
    public GameObject ui_ConnectionStatus;
    public Text connectionStatusTxt;
    public bool showConnectionStatus = false;


    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            ui_Lobby.SetActive(true);
            ui_3D.SetActive(true);
            ui_ConnectionStatus.SetActive(false);

            ui_Login.SetActive(false);
        } else
        {
            ui_Lobby.SetActive(false);
            ui_3D.SetActive(false);
            ui_ConnectionStatus.SetActive(false);

            ui_Login.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (showConnectionStatus)
        {
            connectionStatusTxt.text = "Connection Status" + PhotonNetwork.NetworkClientState;
        }
    }
    #endregion

    #region UI Callback Methods
    public void OnEnterGameButtonClicked()
    {
        string playerName = playerNameInputField.text;

        if(!string.IsNullOrEmpty(playerName))
        {
            ui_Lobby.SetActive(false);
            ui_3D.SetActive(false);
            ui_Login.SetActive(false);

            showConnectionStatus = true;
            ui_ConnectionStatus.SetActive(true);

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        } else
        {
            Debug.Log("Fidle not invalid or empty");
        }
    }

    public void OnQuickMatchButtonClicked()
    {
        //SceneManager.LoadScene(NameScene.Scene_Loading.ToString());
        SceneLoader.Ins.LoadScene(NameScene.Scene_PlayerSelection.ToString());
    }

    #endregion


    #region PHOTON Callback Methods
    public override void OnConnected()
    {
        Debug.Log("We are connected Internet");
    }

    public override void OnConnectedToMaster()
    {
        ui_Lobby.SetActive(true);
        ui_3D.SetActive(true);
        ui_Login.SetActive(false);
        ui_ConnectionStatus.SetActive(false);

    }

    #endregion

}

public enum NameScene
{
    Scene_Gameplay,
    Scene_Loading,
    Scene_Lobby,
    Scene_PlayerSelection
}
