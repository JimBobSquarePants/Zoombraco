// <copyright file="CharLimitPropertyEditor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Resources.PropertyEditors
{
    using ClientDependency.Core;

    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    /// <summary>
    /// A property editor that limits the number of characters used.
    /// </summary>
    [PropertyEditor("Zoombraco.CharLimitEditor", "Zoombraco.CharLimitEditor", ZoombracoConstants.EmbeddedResources.ResourceRoot + "charlimiteditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, ZoombracoConstants.EmbeddedResources.ResourceRoot + "charlimiteditor.controller.js" + ZoombracoConstants.EmbeddedResources.ResourceExtension)]

    public class CharLimitPropertyEditor : PropertyEditor
    {
        /// <inheritdoc/>
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new CharLimitPreValueEditor();
        }
    }
}
