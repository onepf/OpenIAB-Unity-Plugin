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

using System.Collections.Generic;
using System;

namespace OnePF.WP8
{
    public class Store
    {
        public static event Action<Dictionary<string, ProductListing>> LoadListingsSucceeded;
        public static event Action<string> LoadListingsFailed;
        public static event Action<string, string> PurchaseSucceeded;
        public static event Action<string> PurchaseFailed;
        public static event Action<string> ConsumeSucceeded;
        public static event Action<string> ConsumeFailed;

        public static IEnumerable<string> Inventory { get { return new List<string>(); } }

        public static void LoadListings(string[] productIds)
        {
            if (LoadListingsSucceeded != null)
                LoadListingsSucceeded(new Dictionary<string, ProductListing>());
        }

        public static void PurchaseProduct(string productId, string developerPayload) 
        { 
            if (PurchaseSucceeded != null)
                PurchaseSucceeded(productId, developerPayload);
        }

        public static void ConsumeProduct(string productId) 
        {
            if (ConsumeSucceeded != null)
                ConsumeSucceeded(productId);
        }
    }
}

