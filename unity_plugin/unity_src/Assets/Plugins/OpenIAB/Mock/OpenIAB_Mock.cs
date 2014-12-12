namespace OnePF
{
#if UNITY_EDITOR
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Object = UnityEngine.Object;

    public class OpenIAB_Mock : IOpenIAB
    {
        #region Fields

        private readonly Dictionary<string, string> skuToStoreSku = new Dictionary<string, string>();

        private readonly Dictionary<string, string> storeSkuToSku = new Dictionary<string, string>();

        private OpenIABEventManager eventManager;

        #endregion

        #region Public Methods and Operators

        public bool areSubscriptionsSupported()
        {
            return true;
        }

        public void consumeProduct(Purchase purchase)
        {
            throw new NotImplementedException();
        }

        public void enableDebugLogging(bool enabled)
        {
            throw new NotImplementedException();
        }

        public void enableDebugLogging(bool enabled, string tag)
        {
            throw new NotImplementedException();
        }

        public void init(Options options)
        {
            this.eventManager = Object.FindObjectOfType<OpenIABEventManager>();
            if (this.eventManager != null)
            {
                this.eventManager.SendMessage("OnBillingSupported");
            }
        }

        public bool isDebugLog()
        {
            throw new NotImplementedException();
        }

        public void mapSku(string sku, string storeName, string storeSku)
        {
            this.skuToStoreSku[sku] = storeSku;
            this.storeSkuToSku[storeSku] = sku;
        }

        public void purchaseProduct(string sku, string developerPayload = "")
        {
            if (this.eventManager != null)
            {
                this.eventManager.SendMessage("OnPurchaseSucceeded", sku);
            }
        }

        public void purchaseSubscription(string sku, string developerPayload = "")
        {
            this.purchaseProduct(sku, developerPayload);
        }

        public void queryInventory()
        {
            this.queryInventory(this.skuToStoreSku.Keys.ToArray());
        }

        public void queryInventory(string[] inAppSkus)
        {
            // Build fake inventory for testing in editor.
            var inventory = new Inventory();

            foreach (var sku in inAppSkus)
            {
                inventory.AddSkuDetails(
                    new SkuDetails { Sku = sku, Price = "$9.99", PriceValue = "9.99", CurrencyCode = "$" });
            }

            if (this.eventManager != null)
            {
                this.eventManager.SendMessage("OnQueryInventorySucceeded", inventory);
            }
        }

        public void restoreTransactions()
        {
            throw new NotImplementedException();
        }

        public void unbindService()
        {
        }

        #endregion
    }
#endif
}