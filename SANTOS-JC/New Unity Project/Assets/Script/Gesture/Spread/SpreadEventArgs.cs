using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
public class SpreadEventArgs : EventArgs
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

    public float DistanceDelta{
        get;
        private set;
    }

    public GameObject HitObject
    {
        get;
        private set;
    }

    public SpreadEventArgs(Touch finger1, Touch finger2, float delta, GameObject hitObj)
    {
        Finger1 = finger1;
        Finger2 = finger2;
        DistanceDelta = delta;
        HitObject = hitObj;
    }
}
