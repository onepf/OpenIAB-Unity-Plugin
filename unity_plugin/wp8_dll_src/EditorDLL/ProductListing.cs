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

namespace OnePF.WP8
{
    // Summary:
    //     Provides localized info about an in-app offer in your app.
    public sealed class ProductListing
    {
        public ProductListing(
                    string productId,
                    string name,
                    string description,
                    string formattedPrice)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            FormattedPrice = formattedPrice;
        }

        // Summary:
        //     Gets the description for the product.
        //
        // Returns:
        //     The description for the product.
        public string Description { get; private set; }
        //
        // Summary:
        //     Gets the app's purchase price with the appropriate formatting for the current
        //     market.
        //
        // Returns:
        //     The app's purchase price with the appropriate formatting for the current
        //     market.
        public string FormattedPrice { get; private set; }
        //
        // Summary:
        //     Gets the descriptive name of the product or feature that can be shown to
        //     customers in the current market.
        //
        // Returns:
        //     The feature's descriptive name as it is seen by customers in the current
        //     market.
        public string Name { get; private set; }
        //
        // Summary:
        //     Gets the ID of an app's feature or product.
        //
        // Returns:
        //     The ID of an app's feature.
        public string ProductId { get; private set; }
    }
}
