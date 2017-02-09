// <copyright file="CharDisplayPropertyEditor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Resources.PropertyEditors
{
    using ClientDependency.Core;

    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    /// <summary>
    /// A property editor that displays the number of characters used.
    /// </summary>
    [PropertyEditor("Zoombraco.CharDisplayEditor", "Zoombraco.CharDisplayEditor", ZoombracoConstants.EmbeddedResources.ResourceRoot + "chardisplayeditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, ZoombracoConstants.EmbeddedResources.ResourceRoot + "chardisplayeditor.controller.js" + ZoombracoConstants.EmbeddedResources.ResourceExtension)]

    public class CharDisplayPropertyEditor : PropertyEditor
    {
        /// <inheritdoc/>
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new CharDisplayPreValueEditor();
        }
    }
}
