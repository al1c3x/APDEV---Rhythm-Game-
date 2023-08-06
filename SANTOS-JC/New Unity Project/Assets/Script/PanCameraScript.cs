using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCameraScript : MonoBehaviour
{
    public float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        GestureManager.Instance.OnTwoFingerPan += OnFingerPanEvent;
    }

    private void OnDisable()
    {
        GestureManager.Instance.OnTwoFingerPan -= OnFingerPanEvent;

    }

    private void OnFingerPanEvent(object sender, TwoFingerPanEventArgs args)
    {
        Vector2 delta1 = args.TrackedFinger1.deltaPosition;
        Vector2 delta2 = args.TrackedFinger2.deltaPosition;

        Vector2 ave = (delta1 + delta2) / 2;
        ave = ave / Screen.dpi;

        Vector3 change = (Vector3)ave * speed;
        transform.position += change;
    }
}
