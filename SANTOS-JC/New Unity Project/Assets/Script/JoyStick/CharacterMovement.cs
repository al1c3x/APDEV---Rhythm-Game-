using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;

    public OnScreenJoyStick joystick;

    private void FixedUpdate()
    {
        float x = joystick.JoyStickAxis.x;
        float y = joystick.JoyStickAxis.y;
        transform.Translate(x * speed * Time.deltaTime, y * speed * Time.deltaTime, 0);
    }
}
