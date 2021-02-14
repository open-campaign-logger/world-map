// Copyright 2017-2020 Jochen Linnemann, Cory Gill
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Linq.Expressions;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CampaignKit.WorldMap.ViewHelpers
{
    /// <summary>
    ///     Class HtmlHelperExtensions.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        #region Methods

        /// <summary>
        ///     Descriptions for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="self">The self.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>System.String.</returns>
        public static string DescriptionFor<TModel, TValue>(
            this IHtmlHelper<TModel> self, ModelExpressionProvider provider,
            Expression<Func<TModel, TValue>> expression)
        {
            var modelExpression = provider.CreateModelExpression(self.ViewData, expression);
            var metadata = modelExpression.Metadata;

            return metadata.Description;
        }

        #endregion
    }
}