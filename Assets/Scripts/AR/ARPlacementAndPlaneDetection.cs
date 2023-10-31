using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARPlacementAndPlaneDetection : MonoBehaviour
{
    ARPlaneManager _arPlaneManager;
    ARPlacementManager _arPlacementManager;

    public GameObject placeButton;
    public GameObject adjustButton;
    public GameObject searchForGameButton;
    public GameObject sliderScale;
    public TextMeshProUGUI informUIPanel_Txt;

    private void Awake()
    {
        _arPlaneManager=GetComponent<ARPlaneManager>();
        _arPlacementManager = GetComponent<ARPlacementManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        placeButton.SetActive(true);
        sliderScale.SetActive(true);
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);
        informUIPanel_Txt.text = "Move phone to detect plane and place the Battle Arena";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableARPlacementAndPlaneDetection()
    {
        _arPlaneManager.enabled = false;
        _arPlacementManager.enabled = false;

        SetAllPlanesActiveOrDeactive(false);

        placeButton.SetActive(false);
        sliderScale.SetActive(false);
        adjustButton.SetActive(true);
        searchForGameButton.SetActive(true);

        informUIPanel_Txt.text = "Great! You place the Arena... Now, search for game to Battle";

    }

    public void EnableARPlacementAndPlaneDetection()
    {
        _arPlaneManager.enabled = true;
        _arPlacementManager.enabled = true;

        SetAllPlanesActiveOrDeactive(true);

        placeButton.SetActive(true);
        sliderScale.SetActive(true);
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);

        informUIPanel_Txt.text = "Move phone to detect plane and place the Battle Arena";
    }

    private void SetAllPlanesActiveOrDeactive(bool value)
    {
        foreach(var plane in _arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }
}
