/*******************************************************************************
 * Copyright 2012-2014 One Platform Foundation
 *
 *       Licensed under the Apache License, Version 2.0 (the "License");
 *       you may not use this file except in compliance with the License.
 *       You may obtain a copy of the License at
 *
 *           http://www.apache.org/licenses/LICENSE-2.0
 *
 *       Unless required by applicable law or agreed to in writing, software
 *       distributed under the License is distributed on an "AS IS" BASIS,
 *       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *       See the License for the specific language governing permissions and
 *       limitations under the License.
 ******************************************************************************/

using UnityEngine;
using OnePF;
using System.Collections.Generic;

/**
 * Example of OpenIAB usage
 */ 
public class OpenIABTest : MonoBehaviour
{
    const string SKU = "sku";

    string _label = "";
    bool _isInitialized = false;
    Inventory _inventory = null;

    private void OnEnable()
    {
        // Listen to all events for illustration purposes
        OpenIABEventManager.billingSupportedEvent += billingSupportedEvent;
        OpenIABEventManager.billingNotSupportedEvent += billingNotSupportedEvent;
        OpenIABEventManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        OpenIABEventManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
        OpenIABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
        OpenIABEventManager.purchaseFailedEvent += purchaseFailedEvent;
        OpenIABEventManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
        OpenIABEventManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
    }
    private void OnDisable()
    {
        // Remove all event handlers
        OpenIABEventManager.billingSupportedEvent -= billingSupportedEvent;
        OpenIABEventManager.billingNotSupportedEvent -= billingNotSupportedEvent;
        OpenIABEventManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
        OpenIABEventManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
        OpenIABEventManager.purchaseSucceededEvent -= purchaseSucceededEvent;
        OpenIABEventManager.purchaseFailedEvent -= purchaseFailedEvent;
        OpenIABEventManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
        OpenIABEventManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
    }

    private void Start()
    {
        // Map skus for different stores       
        OpenIAB.mapSku(SKU, OpenIAB_Android.STORE_GOOGLE, "sku");
        OpenIAB.mapSku(SKU, OpenIAB_Android.STORE_AMAZON, "sku");
        OpenIAB.mapSku(SKU, OpenIAB_iOS.STORE, "sku");
        OpenIAB.mapSku(SKU, OpenIAB_WP8.STORE, "ammo");
    }

    const float X_OFFSET = 10.0f;
    const float Y_OFFSET = 10.0f;
    const int SMALL_SCREEN_SIZE = 800;
    const int LARGE_FONT_SIZE = 34;
    const int SMALL_FONT_SIZE = 24;
    const int LARGE_WIDTH = 380;
    const int SMALL_WIDTH = 160;
    const int LARGE_HEIGHT = 100;
    const int SMALL_HEIGHT = 40;

    int _column = 0;
    int _row = 0;

    private bool Button(string text)
    {
        float width = Screen.width / 2.0f - X_OFFSET * 2;
        float height = (Screen.width >= SMALL_SCREEN_SIZE || Screen.height >= SMALL_SCREEN_SIZE) ? LARGE_HEIGHT : SMALL_HEIGHT;
        
        bool click = GUI.Button(new Rect(
            X_OFFSET + _column * X_OFFSET * 2 + _column * width, 
            Y_OFFSET + _row * Y_OFFSET + _row * height, 
            width, height),
            text);

        ++_column;
        if (_column > 1)
        {
            _column = 0;
            ++_row;
        }

        return click;
    }

    private void OnGUI()
    {
        _column = 0;
        _row = 0;
        
        GUI.skin.button.fontSize = (Screen.width >= SMALL_SCREEN_SIZE || Screen.height >= SMALL_SCREEN_SIZE) ? LARGE_FONT_SIZE : SMALL_FONT_SIZE;

        if (Button("Initialize OpenIAB"))
        {
            // Application public key
            var publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqibEPHCtfPm3Rn26gbE6vhCc1d6A072im+oWNlkUAJYV//pt1vCkYLqkkw/P2esPSWaw1nt66650vfVYc3sYY6L782n/C+IvZWQt0EaLrqsSoNfN5VqPhPeGf3wqsOvbKw9YqZWyKL4ddZUzRUPex5xIzjHHm3qIJI5v7iFJHOxOj0bLuEG8lH0Ljt/w2bNe4o0XXoshYDqpzIKmKy6OYNQOs8iBTJlfSmPrlGudmldW6CsuAKeVGm+Z+2xx3Xxsx3eSwEgEaUc1ZsMWSGsV6dXgc3JrUvK23JRJUu8X5Ec1OQLyxL3VelD5f0iKVTJ1kw59tMAVZ7DDpzPggWpUkwIDAQAB";

            var options = new Options();
            options.checkInventoryTimeoutMs = Options.INVENTORY_CHECK_TIMEOUT_MS * 2;
            options.discoveryTimeoutMs = Options.DISCOVER_TIMEOUT_MS * 2;
            options.checkInventory = false;
            options.verifyMode = OptionsVerifyMode.VERIFY_SKIP;
            options.prefferedStoreNames = new string[] { OpenIAB_Android.STORE_GOOGLE, OpenIAB_Android.STORE_AMAZON, OpenIAB_Android.STORE_YANDEX };
            options.availableStoreNames = new string[] { OpenIAB_Android.STORE_GOOGLE, OpenIAB_Android.STORE_APPLAND };
            options.storeKeys = new Dictionary<string, string> { {OpenIAB_Android.STORE_GOOGLE, publicKey} };
            options.storeSearchStrategy = SearchStrategy.INSTALLER_THEN_BEST_FIT;

            // Transmit options and start the service
            OpenIAB.init(options);
        }

        if (!_isInitialized)
            return;

        if (Button("Query Inventory"))
        {
            OpenIAB.queryInventory(new string[] { SKU });
        }

        if (Button("Purchase Product"))
        {
            OpenIAB.purchaseProduct(SKU);
        }

        if (Button("Consume Product"))
        {
            if (_inventory != null && _inventory.HasPurchase(SKU))
                OpenIAB.consumeProduct(_inventory.GetPurchase(SKU));
        }

// Android specific buttons
#if UNITY_ANDROID
        if (Button("Test Purchase"))
        {
            OpenIAB.purchaseProduct("android.test.purchased");
        }

        if (Button("Test Consume"))
        {
            if (_inventory != null && _inventory.HasPurchase("android.test.purchased"))
                OpenIAB.consumeProduct(_inventory.GetPurchase("android.test.purchased"));
        }

        if (Button("Test Item Unavailable"))
        {
            OpenIAB.purchaseProduct("android.test.item_unavailable");
        }

        if (Button("Test Purchase Canceled"))
        {
            OpenIAB.purchaseProduct("android.test.canceled");
        }
#endif
    }

    private void billingSupportedEvent()
    {
        _isInitialized = true;
        Debug.Log("billingSupportedEvent");
    }
    private void billingNotSupportedEvent(string error)
    {
        Debug.Log("billingNotSupportedEvent: " + error);
    }
    private void queryInventorySucceededEvent(Inventory inventory)
    {
        Debug.Log("queryInventorySucceededEvent: " + inventory);
        if (inventory != null)
        {
            _label = inventory.ToString();
            _inventory = inventory;
        }
    }
    private void queryInventoryFailedEvent(string error)
    {
        Debug.Log("queryInventoryFailedEvent: " + error);
        _label = error;
    }
    private void purchaseSucceededEvent(Purchase purchase)
    {
        Debug.Log("purchaseSucceededEvent: " + purchase);
        _label = "PURCHASED:" + purchase.ToString();
    }
    private void purchaseFailedEvent(int errorCode, string errorMessage)
    {
        Debug.Log("purchaseFailedEvent: " + errorMessage);
        _label = "Purchase Failed: " + errorMessage;
    }
    private void consumePurchaseSucceededEvent(Purchase purchase)
    {
        Debug.Log("consumePurchaseSucceededEvent: " + purchase);
        _label = "CONSUMED: " + purchase.ToString();
    }
    private void consumePurchaseFailedEvent(string error)
    {
        Debug.Log("consumePurchaseFailedEvent: " + error);
        _label = "Consume Failed: " + error;
    }
}