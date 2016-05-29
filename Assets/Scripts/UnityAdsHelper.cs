using System;
using UnityEngine;
using UnityEngine.Advertisements;
/// <summary>
/// Simple Helper for Unity ADS
/// </summary>
public class UnityAdsHelper
{

    // actions
    public static event Action OnFinished;
    public static event Action OnFailed;
    public static event Action OnSkipped;

    /// <summary>
    /// Show the video ADS
    /// </summary>
    public void Show()
    {
        if (Advertisement.IsReady())
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(null, options);
        }

    }
    /// <summary>
    /// Handler the result of video ADS
    /// </summary>
    /// <param name="result"></param>
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");               
                if (OnFinished != null)
                {
                    OnFinished();
                }
                break;
            case ShowResult.Skipped:
                if (OnSkipped != null)
                {
                    OnSkipped();
                }
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                if (OnFailed != null)
                {
                    OnFailed();
                }
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}