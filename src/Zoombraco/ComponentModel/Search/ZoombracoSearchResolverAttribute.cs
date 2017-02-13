// <copyright file="ZoombracoSearchResolverAttribute.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.ComponentModel.Search
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Newtonsoft.Json;
    using Our.Umbraco.Vorto.Extensions;
    using Our.Umbraco.Vorto.Models;
    using Umbraco.Core.Models;

    /// <summary>
    /// A base class for resolving content for search results.
    /// </summary>
    public abstract class ZoombracoSearchResolverAttribute : Attribute
    {
        /// <summary>
        /// Gets the alias of the property.
        /// </summary>
        public string PropertyAlias { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property is recursive.
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        /// Gets the content to resolve the value for.
        /// </summary>
        public IPublishedContent Content { get; private set; }

        /// <summary>
        /// Gets the property to resolve the value for.
        /// </summary>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// Gets the raw value.
        /// </summary>
        public string RawValue { get; private set; }

        /// <summary>
        /// Gets the raw vorto value or <code>null</code> if there isn't one.
        /// </summary>
        public VortoValue RawVortoValue { get; private set; }

        /// <summary>
        /// Gets the culture object.
        /// </summary>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        /// Performs the value resolution.
        /// </summary>
        /// /// <returns>
        /// The <see cref="string"/> representing the converted value.
        /// </returns>
        public abstract string ResolveValue();

        /// <summary>
        /// Converts the raw value from an Umbraco property into a format that can be indexed by Examine.
        /// </summary>
        /// <param name="content">The <see cref="IPublishedContent"/>to resolve the value for.</param>
        /// <param name="property">The <see cref="PropertyInfo"/> to resolve the value for.</param>
        /// <param name="propertyAlias">The content property alias to resolve the value for.</param>
        /// <param name="rawValue">The raw property value from Umbraco.</param>
        /// <param name="culture"> The <see cref="CultureInfo"/> to help parse values with the correct culture.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        internal virtual string ResolveValue(IPublishedContent content, PropertyInfo property, string propertyAlias, string rawValue, CultureInfo culture)
        {
            this.Content = content;
            this.RawValue = rawValue;
            this.Culture = culture;
            this.Property = property;
            this.PropertyAlias = propertyAlias;
            this.RawVortoValue = this.HasVortoValue() ? JsonConvert.DeserializeObject<VortoValue>(this.RawValue) : null;

            return this.ResolveValue();
        }

        /// <summary>
        /// Returns a value indicating whether this property has a vorto value.
        /// </summary>
        /// <returns>The <see cref="bool"/></returns>
        private bool HasVortoValue()
        {
            return this.Content.HasVortoValue(this.PropertyAlias, this.Culture.Name, this.Recursive);
        }
    }
}