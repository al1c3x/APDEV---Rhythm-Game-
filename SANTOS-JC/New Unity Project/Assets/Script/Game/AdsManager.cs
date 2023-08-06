using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
#if UNITY_ANDROID
    private string gameId = "4265991";
#elif UNITY_IOS
    private string gameId = "4265990";
#endif

    
    bool TestMode = true;
    public GameObject GameManagerGO;
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize("4265991");
        Advertisement.AddListener(this);
    }
    public void PlayAd()
    {
        if (Advertisement.IsReady("ApDevInter"))
        {
            Advertisement.Show("ApDevInter");
        }
    }

    public void PlayRewardAd()
    {
        if (Advertisement.IsReady("ApDevReward"))
        {
            Advertisement.Show("ApDevReward");
        }
        else
        {
            Debug.Log("Reward ad not ready");
        }
    }
    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("ad is ready");
    }
    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("Error" + message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("started");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == "ApDevReward" && showResult == ShowResult.Finished)
        {
            Debug.Log("reward claimed");
            //Dito ilalagay Reward sa ads
            GameManagerGO.GetComponent<GameManager>().AddGold();
        }
    }
}
