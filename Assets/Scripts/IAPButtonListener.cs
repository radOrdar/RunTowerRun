using System;
using Cysharp.Threading.Tasks;
using Services;
using Services.Ads;
using Services.Asset;
using Services.Events;
using Services.Factory;
using Services.Save;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPButtonListener : MonoBehaviour
{
    private const string subId = "com.onimka.towerrunner.stopads";

    #region Dependencies

    private IPersistentDataService _persistentDataService;
    private IEventService _eventService;
    private IAdsService _adsService;
    private IGameFactory _gameFactory;

    #endregion

    private void Start()
    {
        _persistentDataService = ServiceLocator.Instance.Get<IPersistentDataService>();
        _eventService = ServiceLocator.Instance.Get<IEventService>();
        _adsService = ServiceLocator.Instance.Get<IAdsService>();
        _gameFactory = ServiceLocator.Instance.Get<IGameFactory>();
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
            _eventService.OnAdsRemoved();
            _adsService.RemoveAds();
            _persistentDataService.SaveSubscriptionExpirationDate(datetime);
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
        UiPopup uiPopup = await _gameFactory.GetUiPopupAsync();
        // UiPopup uiPopup = await _gameFactory.InstantiateAsync<UiPopup>(Constants.Assets.UI_POPUP);
        await uiPopup.AwaitForCompletion(success? "Ads Removed!" : "Smth went wrong Try again later");
        _gameFactory.ReleaseInstance(uiPopup);
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