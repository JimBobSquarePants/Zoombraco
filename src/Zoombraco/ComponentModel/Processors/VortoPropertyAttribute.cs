// <copyright file="VortoPropertyAttribute.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.ComponentModel.Processors
{
    using System.Linq;
    using System.Web;
    using Our.Umbraco.Ditto;
    using Our.Umbraco.Vorto.Extensions;
    using Umbraco.Core;
    using Umbraco.Core.Models;

    /// <summary>
    /// Provides a unified way to return multilingual Vorto properties from Umbraco.
    /// </summary>
    public class VortoPropertyAttribute : UmbracoPropertyAttribute
    {
        /// <summary>
        /// Gets or sets the default fallback culture name.
        /// </summary>
        public string FallBackCultureName { get; set; }

        /// <inheritdoc/>
        public override object ProcessValue()
        {
            string umbracoPropertyName = this.PropertyName;

            if (this.Context.PropertyDescriptor != null)
            {
                if (string.IsNullOrWhiteSpace(umbracoPropertyName))
                {
                    umbracoPropertyName = this.Context.PropertyDescriptor.Name;
                }
            }

            // First parse the culture.
            HttpCookie languageCookie = HttpContext.Current.Request.Cookies[ZoombracoConstants.Content.CultureCookieName];
            string culture = languageCookie?.Value;
            string fallbackCulture = string.IsNullOrWhiteSpace(this.FallBackCultureName)
                ? this.Context.Culture.Name
                : this.FallBackCultureName;

            // Now check for a value. We check vorto values first then non-vorto fallbacks.
            IPublishedContent content = this.Context.Content;
            object result = null;

            if (content.HasVortoValue(umbracoPropertyName, culture, this.Recursive, fallbackCulture))
            {
                result = content.GetVortoValue(umbracoPropertyName, culture, this.Recursive, null, fallbackCulture);
            }

            if (result == null && !string.IsNullOrWhiteSpace(this.AltPropertyName))
            {
                umbracoPropertyName = this.AltPropertyName;
                result = content.HasVortoValue(umbracoPropertyName, culture, this.Recursive, fallbackCulture)
                    ? content.GetVortoValue(umbracoPropertyName, culture, this.Recursive, null, fallbackCulture)
                    : this.GetPropertyValue(content, umbracoPropertyName, this.Recursive);
            }

            // If a vorto property is null and it's type is a string the base processor will return the property type name as the value.
            if (result == null)
            {
                IPublishedProperty prop = content.Properties.FirstOrDefault(p => p.PropertyTypeAlias.InvariantEquals(umbracoPropertyName));
                if (prop != null)
                {
                    if (prop.DataValue.ToString().InvariantContains("dtdGuid"))
                    {
                        return null;
                    }
                }
            }

            return result ?? base.ProcessValue();
        }
    }
}