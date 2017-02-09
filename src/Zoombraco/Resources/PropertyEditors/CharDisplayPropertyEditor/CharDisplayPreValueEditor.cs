// <copyright file="CharDisplayPreValueEditor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Resources.PropertyEditors
{
    using Umbraco.Core.PropertyEditors;

    /// <summary>
    /// A property editor that displays the number of used characters.
    /// </summary>
    public class CharDisplayPreValueEditor : PreValueEditor
    {
        /// <summary>
        /// Gets or sets a value indicating whether to to use a multiline textarea control.
        /// </summary>
        [PreValueField("multiple", "TextArea", "boolean", Description = "Check to use a multiline textarea control.")]
        public bool Multiple { get; set; }
    }
}
