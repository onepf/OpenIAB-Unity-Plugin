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

using System;
using Windows.ApplicationModel.Store;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using System.Windows.Threading;
using System.Windows;

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

        static string GetErrorDescription(Exception exception)
        {
            string errorMessage;
            switch ((HResult) exception.HResult)
            {
                case HResult.E_FAIL:
                    errorMessage = "Purchase cancelled";
                    break;
                case HResult.E_404:
                    errorMessage = "Not found";
                    break;
                default:
                    errorMessage = exception.Message;
                    break;
            }
            return errorMessage;
        }

        public static IEnumerable<string> Inventory
        {
            get
            {
                List<string> productLicensesList = new List<string>();
                IReadOnlyDictionary<string, ProductLicense> productLicenses = CurrentApp.LicenseInformation.ProductLicenses;
                if (productLicenses != null)
                    foreach (var pl in productLicenses.Values)
                        if (pl.IsActive)
                            productLicensesList.Add(pl.ProductId);
                return productLicensesList;
            }
        }

        public static void LoadListings(string[] productIds)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Dictionary<string, ProductListing> resultListings = new Dictionary<string, ProductListing>();
                IAsyncOperation<ListingInformation> asyncOp;
                try
                {
                    asyncOp = CurrentApp.LoadListingInformationByProductIdsAsync(productIds);
                }
                catch(Exception e)
                {
                    if (LoadListingsFailed != null)
                        LoadListingsFailed(GetErrorDescription(e));
                    return;
                }

                asyncOp.Completed = (op, status) =>
                {
                    if (op.Status == AsyncStatus.Error)
                    {
                        if (LoadListingsFailed != null)
                            LoadListingsFailed(GetErrorDescription(op.ErrorCode));
                        return;
                    }

                    if (op.Status == AsyncStatus.Canceled)
                    {
                        if (LoadListingsFailed != null)
                            LoadListingsFailed("QueryInventory was cancelled");
                        return;
                    }

                    var listings = op.GetResults();
                    foreach (var l in listings.ProductListings)
                    {
                        var listing = l.Value;
                        var resultListing = new ProductListing(
                            listing.ProductId,
                            listing.Name,
                            listing.Description,
                            listing.FormattedPrice);
                        resultListings[l.Key] = resultListing;
                    }
                    if (LoadListingsSucceeded != null)
                        LoadListingsSucceeded(resultListings);
                };
            });
        }

        public static void PurchaseProduct(string productId, string developerPayload)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Kick off purchase; don't ask for a receipt when it returns
                IAsyncOperation<string> asyncOp;
                try
                {
                    asyncOp = CurrentApp.RequestProductPurchaseAsync(productId, false);
                }
                catch (Exception e)
                {
                    if (PurchaseFailed != null)
                        PurchaseFailed(GetErrorDescription(e));
                    return;
                }

                asyncOp.Completed = (op, status) =>
                {
                    if (op.Status == AsyncStatus.Error)
                    {
                        if (PurchaseFailed != null)
                            PurchaseFailed(GetErrorDescription(op.ErrorCode));
                        return;
                    }

                    if (op.Status == AsyncStatus.Canceled)
                    {
                        if (PurchaseFailed != null)
                            PurchaseFailed("Purchase was cancelled");
                        return;
                    }

                    string errorMessage;
                    ProductLicense productLicense = null;
                    if (CurrentApp.LicenseInformation.ProductLicenses.TryGetValue(productId, out productLicense))
                    {
                        if (productLicense.IsActive)
                        {
                            if (PurchaseSucceeded != null)
                                PurchaseSucceeded(productId, developerPayload);
                            return;
                        }
                        else
                        {
                            errorMessage = op.ErrorCode.Message;
                        }
                    }
                    else
                    {
                        errorMessage = op.ErrorCode.Message;
                    }

                    if (PurchaseFailed != null)
                        PurchaseFailed(errorMessage);
                };
            });
        }

        public static void ConsumeProduct(string productId)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    CurrentApp.ReportProductFulfillment(productId);
                }
                catch (Exception e)
                {
                    if (ConsumeFailed != null)
                        ConsumeFailed(e.Message);
                    return;
                }
                if (ConsumeSucceeded != null)
                    ConsumeSucceeded(productId);
            });
        }
    }
}
