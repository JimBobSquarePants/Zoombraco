// <copyright file="UmbracoSearchMergedFieldAttribute.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.ComponentModel.Search
{
    using System;

    /// <summary>
    /// The search merged field attribute. Used to tell Examine that the contents of this property should be added to
    /// a single merged search field for indexing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UmbracoSearchMergedFieldAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoSearchMergedFieldAttribute"/> class.
        /// </summary>
        public UmbracoSearchMergedFieldAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoSearchMergedFieldAttribute"/> class.
        /// </summary>
        /// <param name="examineKey">The Examine key used by Umbraco that overrides the property name.</param>
        public UmbracoSearchMergedFieldAttribute(string examineKey)
        {
            this.ExamineKey = examineKey;
        }

        /// <summary>
        /// Gets or sets the Examine key used by Umbraco that overrides the property name.
        /// </summary>
        public string ExamineKey { get; set; }
    }
}