using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapObjectReceiver : MonoBehaviour, Itap ,ISwipe, IDrag , ISpread , IRotate
{
    public float speed = 10.0f;
    //pinch/spread
    public float scaleSpeed = 3.0f;
    private Vector3 TargetPos = Vector3.zero;
    //rotate
    public float rotateSpeed = 1.0f;
    public void OnRotate(RotateEventArgs args)
    {
        float angle = args.Angle * rotateSpeed;
        if(args.RotateDirection == RotateDirections.CW)
        {
            angle *= -1;
        }
        transform.Rotate(0, 0, angle);
    }

    public void OnSpread(SpreadEventArgs args)
    {
        float scale = (args.DistanceDelta /Screen.dpi) *scaleSpeed;
        Vector3 scaleDiff = new Vector3(scale, scale, scale);
        transform.localScale += scaleDiff;
    }
    public void OnDrag(DragEventArgs args)
    {
        if(args.HitObject == gameObject)
        {
            Ray r = Camera.main.ScreenPointToRay(args.TargetFinger.position);
            Vector3 worldPos = r.GetPoint(10);

            TargetPos = worldPos;
        }
    }
        
    public void OnSwipe(SwipeEventArgs args)
    {
        Vector3 dir = args.SwipeVector.normalized;

        /*switch (args.SwipeDirection)
        {
            case SwipeDirections.RIGHT: dir.x = 1; break;
            case SwipeDirections.LEFT: dir.x = -1; break;
            case SwipeDirections.UP: dir.y = 1; break;
            case SwipeDirections.DOWN: dir.y = -1; break;
        }*/

        TargetPos += (dir *5);
    }

    private void OnEnable()
    {
        TargetPos = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
    }

    public void Ontap()
    {
        Destroy(gameObject);
    }
}
