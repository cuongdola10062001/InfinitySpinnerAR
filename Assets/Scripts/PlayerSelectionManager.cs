using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitcherTransform;
    public GameObject[] spinnerTopModels;
   

    public int playerSelectionNumber;

    [Header("UI")]
    public TextMeshProUGUI playerModelType_Text;
    public Button nextBtn;
    public Button prevBtn;

    public GameObject ui_Selection;
    public GameObject ui_AfterSelection;


    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        ui_Selection.SetActive(true);
        ui_AfterSelection.SetActive(false);

        playerSelectionNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region UI Callback Methods
    public void NextPlayer()
    {
        playerSelectionNumber += 1;

        if(playerSelectionNumber >= spinnerTopModels.Length)
        {
            playerSelectionNumber = 0;
        }

        nextBtn.enabled = false;
        prevBtn.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1f));

        if(playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            //Model type Attack
            playerModelType_Text.text = "Attack";
        } else
        {
            playerModelType_Text.text = "Defend";
        }
    }

    public void PreviousPlayer()
    {
        playerSelectionNumber -= 1;

        if (playerSelectionNumber < 0 )
        {
            playerSelectionNumber = spinnerTopModels.Length -1;
        }


        nextBtn.enabled = false;
        prevBtn.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1f));

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            playerModelType_Text.text = "Attack";
        }
        else
        {
            playerModelType_Text.text = "Defend";
        }
    }
    
    public void OnSelectionButtonClicked()
    {
        ui_Selection.SetActive(false); 
        ui_AfterSelection.SetActive(true);

        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { { MultiplayerARSpinerTopGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);

    }

    public void OnReSelectionButtonClicked()
    {
        ui_Selection.SetActive(true);
        ui_AfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        SceneLoader.Ins.LoadScene(NameScene.Scene_Gameplay.ToString());
    }

    public void OnBackButtonClicked()
    {
        SceneLoader.Ins.LoadScene(NameScene.Scene_Lobby.ToString());
    }
    #endregion


    #region Private Methods
    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transformToRotate.rotation = finalRotation;

        nextBtn.enabled = true;
        prevBtn.enabled = true;
    }
    #endregion

}
