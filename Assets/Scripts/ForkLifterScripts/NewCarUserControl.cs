using System;
using UnityEngine;

[RequireComponent(typeof(NewCarController))]
public class NewCarUserControl : MonoBehaviour
{
    private NewCarController m_Car;
    public FixedJoystick fixedJoystick;

    private void Start()
    {
        m_Car = GetComponent<NewCarController>();

        fixedJoystick = GameObject.FindAnyObjectByType<FixedJoystick>();
    }


    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

#if !MOBILE_INPUT
        float handbrake = Input.GetAxis("Jump");
        m_Car.Move(fixedJoystick.Horizontal, fixedJoystick.Vertical, fixedJoystick.Vertical, handbrake);

#else
            m_Car.Move(fixedJoystick.Horizontal, fixedJoystick.Vertical, fixedJoystick.Vertical, 0f);
#endif
    }
}

