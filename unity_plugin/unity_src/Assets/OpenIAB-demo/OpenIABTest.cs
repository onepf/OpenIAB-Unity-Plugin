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
        OpenIAB.mapSku(SKU, OpenIAB_Android.STORE_SAMSUNG, "100000105017/samsung_sku");
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
            var googlePublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgEEaiFfxugLWAH4CQqXYttXlj3GI2ozlcnWlZDaO2VYkcUhbrAz368FMmw2g40zgIDfyopFqETXf0dMTDw7VH3JOXZID2ATtTfBXaU4hqTf2lSwcY9RXe/Uz0x1nf1oLAf85oWZ7uuXScR747ekzRZB4vb4afm2DsbE30ohZD/WzQ22xByX6583yYE19RdE9yJzFckEPlHuOeMgKOa4WErt11PHB6FTdT5eN96/jjjeEoYhX/NGkOWKW0Y0T0A7CdUC0D4t2xxkzAQHdgLfcRw9+/EIcaysLhncWYiCifJrRBGpqZU1IrNuehrC5FXUN99786c/TwlxNG5nflE6sWwIDAQAB";
            var yandexPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApvU8l4ONEEsSGznPN6DnjIbJnv6vEgm08nbbi+2fMc0V46N7x7jBWTWAf2K6XLZg/rLUkqbWISq12PLvt7ydcsD+Hb9ZubdN2h8LNCTohVPeDbJjd5khtF4J5FNP2/XSTc1C7cSCBTGmqH0fUr77v4x/JMpxKlSjPN6KbNnaF2BLDAdi3012lz2XX4BVgUj7LArID/vYSYGlwMzMkvhUSpvZOM/WIPN+8YDgQAFBlRGRjLhY/3Vpq/AtXtVAzzyfTOZYkwNqdXpwAq5+/51LphowUI5NEBYh8lhQeOJmPNA6EcF1h5L9cJTVLy3bkuCXcjoN2eEO1Nq0h/40G0R4pwIDAQAB";
            var slideMePublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAogOQb0mMbuq4FQ4ZhWRhN8k76/gXOUE370VubZa9Up25GdptYXoRNniecUTDLyjfvWp7+YFW8iPqIp523qNXtQ0EynNhK4xNLvJCd1CjfAju6M0f+o8MOL1zV7g3dHqxICZoHwqBbQOWneDzG/DzJ22AVdLKwty0qbv8ESaCOCJe31ZnoYVMw5KNVkSuRrrhiGGh6xj7F3qZ0T5TOSp3fK7soDamQLevuU7Ndn5IQACjo92HNN0O2PR2cvEjkCRuIkNk2hnqinac984JCzCC0SC/JBnUZUAeYJ7Y8sjT+79z1T1g7yGgDesopnqORiBkeXEZHrFy7PifdA/ZX7rRwQIDAQAB";

            var options = new Options();
            options.checkInventoryTimeoutMs = Options.INVENTORY_CHECK_TIMEOUT_MS * 2;
            options.discoveryTimeoutMs = Options.DISCOVER_TIMEOUT_MS * 2;
            options.checkInventory = false;
            options.verifyMode = OptionsVerifyMode.VERIFY_SKIP;
            options.prefferedStoreNames = new string[] { OpenIAB_Android.STORE_AMAZON };
            options.availableStoreNames = new string[] { OpenIAB_Android.STORE_AMAZON };
            options.storeKeys = new Dictionary<string, string> { {OpenIAB_Android.STORE_GOOGLE, googlePublicKey} };
            options.storeKeys = new Dictionary<string, string> { { OpenIAB_Android.STORE_YANDEX, yandexPublicKey } };
            options.storeKeys = new Dictionary<string, string> { { OpenIAB_Android.STORE_SLIDEME, slideMePublicKey } };
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