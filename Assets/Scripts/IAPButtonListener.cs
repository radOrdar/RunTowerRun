using System;
using Core;
using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPButtonListener : MonoBehaviour
{
    private const string subId = "com.onimka.towerrunner.stopads";

    #region Dependencies

    private PersistentDataProvider _persistentDataProvider;
    private EventsProvider _eventsProvider;
    private AdsProvider _adsProvider;
    private AssetProvider _assetProvider;

    #endregion

    private void Start()
    {
        _persistentDataProvider = ProjectContext.I.PersistentDataProvider;
        _eventsProvider = ProjectContext.I.EventsProvider;
        _adsProvider = ProjectContext.I.AdsProvider;
        _assetProvider = ProjectContext.I.AssetProvider;
    }

    public void OnPurchaseComplete(Product product)
    {
        Debug.Log(product.definition.id);
        if (product.definition.id != subId)
        {
            ShowPopup(false);
            return;
        }

        if (IsSubscribedTo(product, out DateTime datetime))
        {
            _eventsProvider.OnAdsRemoved();
            _adsProvider.RemoveAds();
            _persistentDataProvider.SaveSubscriptionExpirationDate(datetime);
            ShowPopup(true);
        } else
        {
            ShowPopup(false);
        }
    }
    
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription pfDescription)
    {
        ShowPopup(false);
    }

    public void OnProductFetched(Product product)
    {
            
    }

    private async UniTask ShowPopup(bool success)
    {
        UiPopup uiPopup = await _assetProvider.InstantiateAsync<UiPopup>(Constants.Assets.UI_POPUP);
        await uiPopup.AwaitForCompletion(success? "Ads Removed!" : "Smth went wrong Try again later");
        _assetProvider.UnloadInstance(uiPopup.gameObject);
    }
    
    bool IsSubscribedTo(Product subscription, out DateTime subExpireDate)
    {
        // If the product doesn't have a receipt, then it wasn't purchased and the user is therefore not subscribed.
        if (subscription.receipt == null)
        {
            subExpireDate = default;
            return false;
        }

        //The intro_json parameter is optional and is only used for the App Store to get introductory information.
        var subscriptionManager = new SubscriptionManager(subscription, null);

        // The SubscriptionInfo contains all of the information about the subscription.
        // Find out more: https://docs.unity3d.com/Packages/com.unity.purchasing@3.1/manual/UnityIAPSubscriptionProducts.html
        var info = subscriptionManager.getSubscriptionInfo();

        subExpireDate = subscriptionManager.getSubscriptionInfo().getExpireDate();
        return info.isSubscribed() == Result.True;
    }
}