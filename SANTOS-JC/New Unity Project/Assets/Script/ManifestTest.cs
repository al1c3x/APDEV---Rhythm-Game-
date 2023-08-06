using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ManifestTest : MonoBehaviour
{
    public TMP_Text accelTxt;
    public TMP_Text gyroTxt;
    // Start is called before the first frame update
    void Start()
    {
        //internet enable
        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            Handheld.Vibrate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.gyro.enabled)
        {
            Input.gyro.enabled = true;
        }

        //Vector3(x,y,z)
        accelTxt.text = $"Accelerometer: {Input.acceleration}";
        gyroTxt.text = $"Gyroscope:{Input.gyro.rotationRate}";

    }

    public void OnButtonPress()
    {
        Debug.Log("Success");
    }

    public void OnToggleChange(Toggle t)
    {
        Debug.Log($"Toggle:{t.name}-{t.isOn}");
    }

    public void OnSliderChange(Slider sl)
    {
        Debug.Log($"Slider Value:{sl.name}-{sl.value}");
    }
    public void OnDropDownChange(Dropdown dd)
    {
        Debug.Log($"DropDown Value:  {dd.options[dd.value].text}-{dd.value}");
    }

    public void OntextChange(InputField fd)
    {
        Debug.Log($"Text Change:  {fd.text}");
    }
    public void OntextDone(InputField fd)
    {
        Debug.Log($"Text Done:  {fd.text}");
    }
}
