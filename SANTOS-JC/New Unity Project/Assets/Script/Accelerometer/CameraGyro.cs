using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGyro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            Debug.Log("no gyro");
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.gyro.enabled)
        {
            Vector3 rotation = Input.gyro.rotationRate;

            rotation.x *= -1;
            rotation.y *= -1;

            transform.Rotate(rotation);
        }
    }
}
