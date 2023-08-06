using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GestureManager : MonoBehaviour
{
    public EventHandler<TapEventArgs> OnTap;
    public EventHandler<SwipeEventArgs> OnSwipe;
    public EventHandler<DragEventArgs> OnDrag;
    public EventHandler<TwoFingerPanEventArgs> OnTwoFingerPan;
    public EventHandler<SpreadEventArgs> OnSpread;
    public EventHandler<RotateEventArgs> OnRotate;

    public static GestureManager Instance;

    public TwoFingerPanProperty _twoFingerPanProperty;
    public DragProperty _dragProperty;
    public SwipeProperty _swipeProperty;
    public TapProperty _tapProperty;
    public SpreadProperty _spreadProperty;
    public RotateProperty _rotateProperty;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Touch trackedFinger1;
    private Touch trackedFinger2;
    private float gestureTime;






    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            // Single Control;er
            if (Input.touchCount == 1)
            {
                SingleFingerControl();
            }
            else if (Input.touchCount >  1)
            {
                trackedFinger1 = Input.GetTouch(0);
                trackedFinger2 = Input.GetTouch(1);
                if ((trackedFinger1.phase == TouchPhase.Moved || trackedFinger2.phase == TouchPhase.Moved) 
                    && Vector2.Distance(trackedFinger1.position,trackedFinger2.position) >= (_rotateProperty.MinDistance  *Screen.dpi))
                {
                    Vector2 prevPoint1 = GetPrevPoint(trackedFinger1);
                    Vector2 prevPoint2 = GetPrevPoint(trackedFinger2);

                    Vector2 diffVector = trackedFinger1.position - trackedFinger2.position;
                    Vector2 prevDiffVector = prevPoint1 - prevPoint2;

                    float angle = Vector2.Angle(prevDiffVector,diffVector);
                    if(angle >= _rotateProperty.minAngleChange)
                    {
                        Vector3 cross = Vector3.Cross(prevDiffVector, diffVector);
                        FireRotateEvent(angle, cross);
                        
                    }
                  
                }

                //Pinch/Spread
                if (trackedFinger1.phase == TouchPhase.Moved || trackedFinger2.phase == TouchPhase.Moved)
                {
                    Vector2 prevPoint1 = GetPrevPoint(trackedFinger1);
                    Vector2 prevPoint2 = GetPrevPoint(trackedFinger2);

                    float prevDistance = Vector2.Distance(prevPoint1, prevPoint2);
                    float currDistance = Vector2.Distance(trackedFinger1.position, trackedFinger2.position);

                    if(Mathf.Abs(currDistance - prevDistance) >=  (_spreadProperty.MinDistanceChange * Screen.dpi))
                    {
                        FireSpreadEvent(currDistance - prevDistance);
                    }
                }

                //TwoFingerPan
                if(trackedFinger1.phase == TouchPhase.Moved && trackedFinger2.phase == TouchPhase.Moved 
                    && Vector2.Distance(trackedFinger1.position, trackedFinger2.position) <= (Screen.dpi * _twoFingerPanProperty.MaxDistance))
                {
                    FireTwoFingerPanEvent();
                }
            }

        }
         
    }

    private void FireRotateEvent(float angle, Vector3 cross)
    {
        RotateDirections rotDir = RotateDirections.CW;
        if (cross.z > 0)
        {
            Debug.Log($"CCW:{angle}");
            rotDir = RotateDirections.CCW;
        }
        else
        {
            Debug.Log($"CW:{angle}");
            rotDir = RotateDirections.CW;
        }
        Vector2 mid = GetMidPoint(trackedFinger1.position, trackedFinger2.position);
        Ray r = Camera.main.ScreenPointToRay(mid);
        RaycastHit hit = new RaycastHit();
        GameObject hitObj = null;

        if (Physics.Raycast(r, out hit, Mathf.Infinity))
        {
            hitObj = hit.collider.gameObject;
        }

        RotateEventArgs args = new RotateEventArgs(trackedFinger1, trackedFinger2, angle, rotDir, hitObj);

        if(OnRotate != null)
        {
            OnRotate(this, args);
        }
        if(hitObj != null)
        {
            IRotate rotate = hitObj.GetComponent<IRotate>();
            if (rotate != null)
            {
                rotate.OnRotate(args);
            }

        }

    }
    private void FireSpreadEvent(float delta)
    {
        Debug.Log("Spread / Pinch");
        Vector2 mid = GetMidPoint(trackedFinger1.position, trackedFinger2.position);

        Ray r = Camera.main.ScreenPointToRay(mid);
        RaycastHit hit = new RaycastHit();
        GameObject hitObj = null;

        if(Physics.Raycast(r,out hit, Mathf.Infinity))
        {
            hitObj = hit.collider.gameObject;
        }

        SpreadEventArgs args = new SpreadEventArgs(trackedFinger1, trackedFinger2, delta, hitObj);

        if(OnSpread != null)
        {
            OnSpread(this, args);
        }

        if(hitObj != null)
        {
            ISpread spread = hitObj.GetComponent<ISpread>();
            if(spread != null)
            {
                spread.OnSpread(args);
            }
        }
    }

    private Vector2 GetMidPoint(Vector2 finger1,Vector2 finger2)
    {
        return (finger1 + finger2) / 2;
    }

    private Vector2 GetPrevPoint(Touch finger)
    {
        return finger.position - finger.deltaPosition;
    }
   
    private void SingleFingerControl()
    {
        trackedFinger1 = Input.GetTouch(0);
        if (trackedFinger1.phase == TouchPhase.Began)
        {
            gestureTime = 0;
            startPoint = trackedFinger1.position;
        }
        else if (trackedFinger1.phase == TouchPhase.Ended)
        {
            endPoint = trackedFinger1.position;

            //swipe
            if(gestureTime <= _swipeProperty.swipeTime && Vector2.Distance(startPoint,endPoint) >= (Screen.dpi * _swipeProperty.minSwipeDistance))
            {
                FireSwipeEvent();
            }

            //Tap
            if(gestureTime <= _tapProperty.tapTime && Vector2.Distance(startPoint,endPoint) < (Screen.dpi *_tapProperty.tapMaxDistance))
            {
                FireTapEvent(startPoint);
                Debug.Log("Tap");
            }
        }
        else
        {
            gestureTime += Time.deltaTime;
            if (gestureTime >= _dragProperty.dragBufferTime)
            {
                FireDragEvent();
            }
        }

    }

    private void FireTwoFingerPanEvent()
    {
        Debug.Log("two finger pan");
        TwoFingerPanEventArgs args = new TwoFingerPanEventArgs(trackedFinger1, trackedFinger2);

        if(OnTwoFingerPan != null)
        {
            OnTwoFingerPan(this, args);
        }
    }

    private void FireDragEvent()
    {
        Debug.Log($"Drag: {trackedFinger1.position}");

        GameObject hitObj = null;
        Ray r = Camera.main.ScreenPointToRay(trackedFinger1.position);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(r, out hit, Mathf.Infinity))
        {
            hitObj = hit.collider.gameObject;
        }

        DragEventArgs args = new DragEventArgs(trackedFinger1, hitObj);
        if(OnDrag != null)
        {
            OnDrag(this, args);
        }
        if(hitObj != null)
        {
            IDrag drag = hitObj.GetComponent<IDrag>();
            if(drag != null)
            {
                drag.OnDrag(args);
            }
        }
    }
    private void FireSwipeEvent()
    {
        Debug.Log("Swipe!");
        Vector2 dir = endPoint - startPoint;

        GameObject hitObj = null;
        Ray r = Camera.main.ScreenPointToRay(startPoint);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(r, out hit, Mathf.Infinity))
        {
            hitObj = hit.collider.gameObject;
        }
        SwipeDirections swipedir = SwipeDirections.RIGHT;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if(dir.x > 0)
            {
                Debug.Log("Right!");
                swipedir = SwipeDirections.RIGHT;

            }
            else
            {
                Debug.Log("Left!");
                swipedir = SwipeDirections.LEFT;
            }
        }
        else
        {
            if(dir.y > 0)
            {
                Debug.Log("Up!");
                swipedir = SwipeDirections.UP;
            }
            else
            {
                Debug.Log("Down!");
                swipedir = SwipeDirections.DOWN;
            }
        }
        SwipeEventArgs args = new SwipeEventArgs(startPoint, swipedir, dir, hitObj);
        if(OnSwipe != null)
        {
            OnSwipe(this, args);
        }
        if(hitObj != null)
        {
            ISwipe swipeInterface = hitObj.GetComponent<ISwipe>();
            if(swipeInterface != null)
            {
                swipeInterface.OnSwipe(args);
            }
        }
    }

    private void FireTapEvent(Vector2 pos)
    {
        Debug.Log("Tap");
        if(OnTap != null)
        {
            GameObject hitObj = null;
            Ray r = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit = new RaycastHit();

            if(Physics.Raycast(r,out hit, Mathf.Infinity))
            {
                hitObj = hit.collider.gameObject;
            }



            TapEventArgs args = new TapEventArgs(pos, hitObj);
            OnTap(this, args);

            if(hitObj != null)
            {
                Itap receiver = hitObj.GetComponent<Itap>();
                if(receiver != null)
                {
                    receiver.Ontap();
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        if(Input.touchCount > 0)
        {
            Ray r = Camera.main.ScreenPointToRay(trackedFinger1.position);
            Gizmos.DrawIcon(r.GetPoint(10), "ina");

            if(Input.touchCount > 1)
            {

                Ray r2 = Camera.main.ScreenPointToRay(trackedFinger2.position);
                Gizmos.DrawIcon(r2.GetPoint(10), "rushia");

            }


        
        }
    }

}
