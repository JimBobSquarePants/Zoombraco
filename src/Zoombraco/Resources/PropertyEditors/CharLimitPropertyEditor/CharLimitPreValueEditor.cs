// <copyright file="CharLimitPreValueEditor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Resources.PropertyEditors
{
    using Umbraco.Core.PropertyEditors;

    /// <summary>
    /// A property editor that limits the number of used characters.
    /// </summary>
    public class CharLimitPreValueEditor : PreValueEditor
    {
        /// <summary>
        /// Gets or sets a value to limit the editor content to.
        /// </summary>
        [PreValueField("limit", "Limit", ZoombracoConstants.EmbeddedResources.ResourceRoot + "requirednumber.html", Description = "Enter the number of chars to limit the editor to.")]
        public int Limit { get; set; }
    }
}
