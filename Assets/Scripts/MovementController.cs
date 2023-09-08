using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float _xMovementInput = joystick.Horizontal;
        float _zMovementInput = joystick.Vertical;

        Vector3 _movementHorizontal = transform.right * _xMovementInput;
        Vector3 _movementVertical = transform.forward * _xMovementInput;

        Vector3 _movementVelocityVector = (_movementHorizontal+_movementVertical).normalized * speed;
    }
}
