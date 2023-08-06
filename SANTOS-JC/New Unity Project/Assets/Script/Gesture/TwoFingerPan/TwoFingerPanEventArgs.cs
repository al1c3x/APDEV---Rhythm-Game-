using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class TwoFingerPanEventArgs:EventArgs
{
    public Touch TrackedFinger1
    {
        get;
        private set;
    }

    public Touch TrackedFinger2
    {
        get;
        private set;
    }

    public TwoFingerPanEventArgs(Touch finger1, Touch finger2)
    {
        TrackedFinger1 = finger1;
        TrackedFinger2 = finger2;
    }
}
    