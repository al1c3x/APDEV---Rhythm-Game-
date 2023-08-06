using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject playbutton;
    public GameObject playerShip;
    public GameObject EnemySpawner;
    public GameObject GameOverGo;
    public GameObject scoreUITextGO;
    public GameObject TimeCounterGO;
    public GameObject TitleGO;
    public GameObject GoldGO;
    public GameObject HeartGO;
    public GameObject AdsGO;

    public GameObject NotifButtonGO;

    public Text GoldUIText;
    public int sGold = 0;
    int level = 1;
    public enum GameManagerState
    {
        Opening,
        GamePlay,
        GameOver
    }

    GameManagerState GMState;
    // Start is called before the first frame update
    void Start()
    {
        GoldUIText.text = "Gold:" + sGold;
        GMState = GameManagerState.Opening;
    }

    // Update is called once per frame
    void UpdateGameManagerState()
    {
        switch (GMState)
        {
            case GameManagerState.Opening:
                AdsGO.SetActive(true);
                HeartGO.SetActive(true);
                GoldGO.SetActive(true);
                GameOverGo.SetActive(false);
                NotifButtonGO.SetActive(true);
                playbutton.SetActive(true);
                TitleGO.SetActive(true);
                break;
            case GameManagerState.GamePlay:
                AdsGO.SetActive(false);
                HeartGO.SetActive(false);
                GoldGO.SetActive(false);
                TitleGO.SetActive(false);
                NotifButtonGO.SetActive(false);
                scoreUITextGO.GetComponent<GameScore>().Score = 0;
                playbutton.SetActive(false);
                playerShip.GetComponent<PlayerControl>().Init();
                EnemySpawner.GetComponent<EnemySpawner>().StartSpawner();
                TimeCounterGO.GetComponent<TimeCounter>().StartTimeCounter();
                break;
            case GameManagerState.GameOver:
                AdsGO.SetActive(true);
                HeartGO.SetActive(false);
                GoldGO.SetActive(false);
                NotifButtonGO.SetActive(false);
                TimeCounterGO.GetComponent<TimeCounter>().StopTimeCounter();
                EnemySpawner.GetComponent<EnemySpawner>().StopSpawner();
                GameOverGo.SetActive(true);
                Invoke("OpeningState", 8.0f);
                sGold += Random.Range(8, 20);
                GoldUIText.text = "Gold:" + sGold;
                break;
        }
    }

    public void AddGold()
    {
        sGold += Random.Range(8, 50);
        GoldUIText.text = "Gold:" + sGold;
    }

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    public void StartGamePlay()
    {
        GMState = GameManagerState.GamePlay;
        UpdateGameManagerState();
    }

    public void OpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);
    }

    public void GenerateNotif()
    {
        AndroidNotificationCenter.CancelAllDisplayedNotifications();


        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notifications Channel",
            Importance = Importance.Default,
            Description = "Reminder notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        var notification = new AndroidNotification();
        notification.Title = "Test Notification";
        notification.Text = "Sample";
        //notification.FireTime = System.DateTime.Now.AddMinutes(1);
        notification.FireTime = System.DateTime.Now.AddSeconds(10);

        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }
    }

    public void UpgradeHp()
    {
        if(sGold >= 25 * level && level < 5)
        {
            level++;
            sGold -= 25;
            playerShip.GetComponent<PlayerControl>().IncreaseHp();
            GoldUIText.text = "Gold:" + sGold;
        }
    }
}
