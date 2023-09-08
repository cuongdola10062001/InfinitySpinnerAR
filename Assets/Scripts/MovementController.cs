using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 3f;
    private Vector3 velocityVector = Vector3.zero;

    public float maxVelocityChange = 4f;
    private Rigidbody m_rb;
    public float tiltAmount = 3f;

    void Start()
    {
        m_rb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float _xMovementInput = joystick.Horizontal;
        float _zMovementInput = joystick.Vertical;

        Vector3 _movementHorizontal = transform.right * _xMovementInput;
        Vector3 _movementVertical = transform.forward * _zMovementInput;

        Vector3 _movementVelocityVector = (_movementHorizontal+_movementVertical).normalized * speed;

        Move(_movementVelocityVector);

        transform.rotation = Quaternion.Euler(joystick.Vertical * speed * tiltAmount, 0, -joystick.Horizontal * speed * tiltAmount);

    }

    private void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;
    }

    private void FixedUpdate()
    {
        if(velocityVector != Vector3.zero)
        {
            Vector3 velocity = m_rb.velocity;
            Vector3 velocityChange = (velocityVector - velocity);

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            m_rb.AddForce(velocityChange, ForceMode.Acceleration);
        }

    }
}
