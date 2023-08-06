using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelGyroHandler : MonoBehaviour
{
    public float speed = 5.0f;
    public float minChange = 0.2f;

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

    public static Quaternion GyroToUnity(Quaternion d)
    {
        return new Quaternion(d.x, d.y, -d.z, -d.w);
    }
  

    private void FixedUpdate()
    {
        if (Input.gyro.enabled)
        {
            Quaternion rotation = GyroToUnity(Input.gyro.attitude);
            transform.rotation = rotation;
        }
       

        /*Vector3 accel = Input.acceleration;
        
       if(Mathf.Abs(accel.x) >= minChange)
        {
            accel.x *= (speed * Time.deltaTime);
            transform.Translate(accel.x, 0, 0);
        }*/
       
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Debug.DrawRay(transform.position, Input.acceleration.normalized, Color.red);
        }
    }
}
