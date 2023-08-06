using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationControl : MonoBehaviour
{
    DeviceOrientation lastOrientation;

    // Start is called before the first frame update
    public void Update()
    {
        lastOrientation = Input.deviceOrientation;
    }

   public void FingerUp()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;

        Screen.orientation = ScreenOrientation.AutoRotation;

    }

    public void FingerDown()
    {
        int orientation = (int)lastOrientation;

        if(orientation > 4)
        {
            orientation = (int)DeviceOrientation.LandscapeLeft;
        }

        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        Screen.orientation = (ScreenOrientation)orientation;
    }
}
