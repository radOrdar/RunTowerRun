
using System;
using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using Core;
using UnityEngine;

public class AdsProvider : MonoBehaviour
{
    [SerializeField] private bool isTestMode;
    
    private const string AppKey = "89dd55964f87db9d175943d5de2e415c2fc20d3826b3766b";

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
