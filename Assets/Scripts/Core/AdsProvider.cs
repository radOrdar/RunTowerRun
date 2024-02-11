
using System;
using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using Core;
using UnityEngine;

public class AdsProvider : MonoBehaviour
{
    [SerializeField] private bool isTestMode;
    
    private const string AppKey = "a287d5f56949919de334e67dc0188e8f68c19f09704bea5a";

    private bool _active;

    public void Initialize()
    {
        _active = true;
        
        int adTypes = AppodealAdType.Interstitial | AppodealAdType.Banner;

        Appodeal.SetTesting(isTestMode);
        Appodeal.SetAutoCache(adTypes, true);
        
        AppodealCallbacks.Sdk.OnInitialized += OnAppodealInitalized;
        
        AppodealCallbacks.Interstitial.OnLoaded += OnInterstitialLoaded;
        AppodealCallbacks.Interstitial.OnFailedToLoad += OnInterstitialFailedToLoad;
        AppodealCallbacks.Interstitial.OnShown += OnInterstitialShown;
        
        Appodeal.Initialize(AppKey, adTypes);
    }

    public void ShowInterstitial()
    {
        if (!_active) 
            return;
        
        if (Appodeal.IsLoaded(AppodealAdType.Interstitial))
        {
            Appodeal.Show(AppodealAdType.Interstitial);
        }
    }

    public void ShowBanner()
    {
        if (!_active) 
            return;
        
        Appodeal.Show(AppodealShowStyle.BannerBottom);
    }

    public void RemoveAds()
    {
        _active = false;
        HideBanner();
    }

    public void HideBanner()
    {
        Appodeal.Hide(AppodealAdType.Banner);
    }

    private void OnInterstitialShown(object sender, EventArgs e)
    {
        Debug.Log("Interstitial shown");
    }

    private void OnInterstitialFailedToLoad(object sender, EventArgs e)
    {
        Debug.Log("Interstitial failed to load");
    }

    private void OnInterstitialLoaded(object sender, AdLoadedEventArgs e)
    {
        Debug.Log("Interstitial Loaded");
    }

    private void OnAppodealInitalized(object sender, SdkInitializedEventArgs e)
    {
        _active = true;
        Debug.Log("Appodeal Initialized");
    }
}
