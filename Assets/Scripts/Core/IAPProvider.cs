using System.Collections.Generic;
using Core;
using Gley.EasyIAP;
using Infrastructure;
using UnityEngine;

public class IAPProvider
{
    public bool Initialized { get; private set; }
    
    private List<StoreProduct> _shopProducts;


    public IAPProvider()
    {
        API.Initialize(InitializationComplete);
    }

    private void InitializationComplete(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
    {
        if (status == IAPOperationStatus.Success)
        {
            Initialized = true;
            _shopProducts = shopProducts;
            //IAP was successfully initialized
            //loop through all products
            for (int i = 0; i < shopProducts.Count; i++)
            {
                //if remove ads was bought before, mark it as owned.
                if (shopProducts[i].productName == "CancelAds")
                {
                    if (shopProducts[i].active)
                    {
                        Debug.Log("Cancel Ads");
                        PlayerPrefs.SetInt(Constants.PrefsKeys.REMOVE_ADS, 1);
                    }
                }
            }
        } else
        {
            Initialized = false;
            Debug.Log("Error occurred: " + message);
        }
    }

    public void BuyCancelAds()
    {
        //  this is the normal implementation
        // but since your products will not have the same names, we will use the string version to avoid compile error

         API.BuyProduct(ShopProductNames.CancelAds, ProductBought);
    }
    
    private void ProductBought(IAPOperationStatus status, string message, StoreProduct product)
    {
        if (status == IAPOperationStatus.Success)
        {
            if (product.productName == "CancelAds")
            {
                PlayerPrefs.SetInt(Constants.PrefsKeys.REMOVE_ADS, 1);
                Debug.Log("Cancel Ads");
                ProjectContext.I.AdsProvider.RemoveAds();
                ProjectContext.I.EventsProvider.OnAdsRemoved();
            }
        }
        else
        {
            //Tooltip show operation failure
            Debug.Log("Error occurred: " + message);
        }
    }
}