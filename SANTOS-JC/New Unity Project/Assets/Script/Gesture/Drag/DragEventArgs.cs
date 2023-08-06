using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
public class DragEventArgs 
{
   public Touch TargetFinger
    {
        get;
        private set;

    }
    public GameObject HitObject
    {
        get;
        private set;
    } = null;

    public DragEventArgs(Touch targetFinger, GameObject hitObject)
    {
        TargetFinger = targetFinger;
        HitObject = hitObject;
    }
}
