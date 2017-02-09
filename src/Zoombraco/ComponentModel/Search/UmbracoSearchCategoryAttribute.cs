// <copyright file="UmbracoSearchCategoryAttribute.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.ComponentModel.Search
{
    using System;

    /// <summary>
    /// The search category attribute. Used to tell Examine that the contents of this property should be added to
    /// a specific category or categories for filtered searching.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UmbracoSearchCategoryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoSearchCategoryAttribute"/> class.
        /// </summary>
        /// <param name="categories">
        /// The categories to to assign to the document type to search within.
        /// </param>
        public UmbracoSearchCategoryAttribute(string[] categories)
        {
            this.Categories = categories;
        }

        /// <summary>
        /// Gets or sets the categories to to assign to the document type to search within.
        /// </summary>
        public string[] Categories { get; set; }
    }
}
