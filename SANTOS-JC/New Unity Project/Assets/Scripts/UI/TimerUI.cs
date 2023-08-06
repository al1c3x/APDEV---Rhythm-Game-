using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public GameHandler gameHandler;
    public Image timerBar;
    public GameObject TapButton;


    // Update is called once per frame
    void Update()
    {
        timerBar.fillAmount = 1 - (gameHandler.CurrentTime / gameHandler.MaxTime);
       
        if(timerBar.fillAmount < 0.25f)
        {
            timerBar.color = Color.green;
            TapButton.SetActive(true);
            
        }
        else if(timerBar.fillAmount < 0.50f)
        {
            timerBar.color = Color.yellow;
        }
        else
        {
            timerBar.color = Color.red;
            TapButton.SetActive(false);
        }
    }
}
