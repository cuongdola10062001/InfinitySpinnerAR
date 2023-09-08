using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float spinSpeed = 3600;
    public bool doSpin = false;
    public GameObject playerGraphics;

    private Rigidbody m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(doSpin)
        {
            playerGraphics.transform.Rotate(new Vector3(0, Time.deltaTime * spinSpeed, 0));
        }
    }
}
