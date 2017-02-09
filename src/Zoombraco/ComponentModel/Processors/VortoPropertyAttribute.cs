// <copyright file="VortoPropertyAttribute.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.ComponentModel.Processors
{
    using System.Web;
    using Our.Umbraco.Ditto;
    using Our.Umbraco.Vorto.Extensions;
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

            HttpCookie languageCookie = HttpContext.Current.Request.Cookies[ZoombracoConstants.Content.CultureCookieName];
            string culture = languageCookie?.Value;
            string fallbackCulture = string.IsNullOrWhiteSpace(this.FallBackCultureName)
                ? this.Context.Culture.Name
                : this.FallBackCultureName;

            IPublishedContent content = this.Context.Content;
            object result = content.GetVortoValue(umbracoPropertyName, culture, this.Recursive, null, fallbackCulture);

            if (result == null && !string.IsNullOrWhiteSpace(this.AltPropertyName))
            {
                result = content.GetVortoValue(this.AltPropertyName, culture, this.Recursive, null, fallbackCulture);
            }

            return result ?? base.ProcessValue();
        }
    }
}