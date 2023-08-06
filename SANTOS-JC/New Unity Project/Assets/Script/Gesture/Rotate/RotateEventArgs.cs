using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public enum RotateDirections
{
    CW,
    CCW
}

public class RotateEventArgs : EventArgs
{
    public Touch Finger1
    {
        get;
        private set;
    }

    public Touch Finger2
    {
        get;
        private set;
    }

    public float Angle
    {
        get;
        private set;
            
    }

    public RotateDirections RotateDirection
    {
        get;
        private set;
    }

    public GameObject HitObject
    {
        get;
        private set;
    }
    
    public RotateEventArgs(Touch finger1, Touch finger2, float angle , RotateDirections rotateDirections, GameObject hitObj)
    {
        Finger1 = finger1;
        Finger2 = finger2;
        Angle = angle;
        RotateDirection = rotateDirections;
        HitObject = hitObj;
    }


}
