using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ScaleController : MonoBehaviour
{
    ARSessionOrigin _arSessionOrigin;
    public Slider scaleSlider;

    private void Awake()
    {
        _arSessionOrigin=GetComponent<ARSessionOrigin>();
    }

    // Start is called before the first frame update
    void Start()
    {
        scaleSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSliderValueChanged(float value)
    {
        if(scaleSlider != null)
        {
            _arSessionOrigin.transform.localScale = Vector3.one / value;
        }
    }
}
