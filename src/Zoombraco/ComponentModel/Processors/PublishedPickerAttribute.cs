// <copyright file="PublishedPickerAttribute.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.ComponentModel.Processors
{
    using Our.Umbraco.Ditto;

    /// <summary>
    /// A picker for returning IPublished content from pickers. Otherwise Umbraco returns an int.
    /// </summary>
    public class PublishedPickerAttribute : DittoMultiProcessorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedPickerAttribute"/> class.
        /// </summary>
        public PublishedPickerAttribute()
            : base(
                  new DittoProcessorAttribute[]
                      {
                          new UmbracoPropertyAttribute(),
                          new UmbracoPickerAttribute()
                      })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedPickerAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="altPropertyName">Name of the alternative property.</param>
        /// <param name="recursive">If set to <c>true</c>, a recursive lookup is performed.</param>
        /// <param name="defaultValue">The default value.</param>
        public PublishedPickerAttribute(string propertyName, string altPropertyName = null, bool recursive = false, object defaultValue = null)
            : base(
                  new DittoProcessorAttribute[]
                      {
                          new UmbracoPropertyAttribute(propertyName, altPropertyName, recursive, defaultValue),
                          new UmbracoPickerAttribute()
                      })
        {
        }
    }
}
