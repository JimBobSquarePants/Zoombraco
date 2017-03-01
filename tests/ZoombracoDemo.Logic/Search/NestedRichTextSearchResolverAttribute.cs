// <copyright file="NestedRichTextSearchResolverAttribute.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Search
{
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Umbraco.Core;
    using Zoombraco.ComponentModel.Search;
    using ZoombracoDemo.Logic.Models;

    /// <summary>
    /// Resolves the <see cref="M:NestedRichText.BodyText"/> property for Examine.
    /// </summary>
    public class NestedRichTextSearchResolverAttribute : ZoombracoSearchResolverAttribute
    {
        /// <inheritdoc/>
        public override string ResolveValue()
        {
            // Vorto nested values
            if (this.RawVortoValue != null)
            {
                if (this.RawVortoValue.Values.ContainsKey(this.Culture.Name))
                {
                    List<object> nestedValue = JsonConvert.DeserializeObject<List<object>>(this.RawVortoValue.Values[this.Culture.Name].ToString());
                    return this.GetNestedContentRichText(nestedValue);
                }
            }

            // Standard nested values
            List<object> rawValue = JsonConvert.DeserializeObject<List<object>>(this.RawValue);
            return this.GetNestedContentRichText(rawValue);
        }

        /// <summary>
        /// Returns the nested content type alias from the given item.
        /// </summary>
        /// <param name="item">The JSON object</param>
        /// <returns>The <see cref="string"/></returns>
        private static string GetContentTypeAliasFromItem(JObject item)
        {
            JToken contentTypeAliasProperty = item["ncContentTypeAlias"];
            return contentTypeAliasProperty?.ToObject<string>();
        }

        /// <summary>
        /// Returns the rich text values from the nested content.
        /// </summary>
        /// <param name="nestedRawValue">The raw value from nested content.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string GetNestedContentRichText(List<object> nestedRawValue)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object value in nestedRawValue)
            {
                JObject item = (JObject)value;
                string contentTypeAlias = GetContentTypeAliasFromItem(item);
                if (string.IsNullOrEmpty(contentTypeAlias))
                {
                    continue;
                }

                // TODO: Get rich text editors for now. Think of something more generic later.
                if (contentTypeAlias.InvariantEquals(nameof(NestedRichText)))
                {
                    JToken contentTypeAliasProperty = item[nameof(NestedRichText.BodyText).ToFirstLowerInvariant()];
                    sb.Append(contentTypeAliasProperty?.ToObject<string>());
                }
            }

            return sb.ToString();
        }
    }
}