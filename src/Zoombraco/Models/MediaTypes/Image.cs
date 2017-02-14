// <copyright file="Image.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System;
    using System.ComponentModel;
    using Our.Umbraco.Ditto;
    using Umbraco.Core;
    using Umbraco.Web.Models;
    using Zoombraco.ComponentModel.Search;

    /// <summary>
    /// Represents a single published image in the media section.
    /// </summary>
    [DittoLazy]
    [UmbracoPicker]
    public class Image : IEntity, IFile
    {
        /// <inheritdoc/>
        public virtual int Id { get; set; }

        /// <inheritdoc/>
        [UmbracoSearchMergedField(ZoombracoConstants.Search.NodeName)]
        public virtual string Name { get; set; }

        /// <inheritdoc/>
        public virtual string DocumentTypeAlias { get; set; }

        /// <inheritdoc/>
        public virtual int Level { get; set; }

        /// <inheritdoc/>
        public virtual DateTime CreateDate { get; set; }

        /// <inheritdoc/>
        public virtual DateTime UpdateDate { get; set; }

        /// <inheritdoc/>
        [UmbracoProperty(Constants.Conventions.Media.File)]
        public virtual string FileName { get; set; }

        /// <inheritdoc/>
        [UmbracoProperty(Constants.Conventions.Media.Bytes)]
        public virtual int Bytes { get; set; }

        /// <inheritdoc/>
        [UmbracoProperty(Constants.Conventions.Media.Extension)]
        public virtual string Extension { get; set; }

        /// <summary>
        /// Gets or sets the width in pixels.
        /// </summary>
        [UmbracoProperty(Constants.Conventions.Media.Width)]
        public virtual int Width { get; set; }

        /// <summary>
        /// Gets or sets the height in pixels.
        /// </summary>
        [UmbracoProperty(Constants.Conventions.Media.Height)]
        public virtual int Height { get; set; }

        /// <summary>
        /// Gets or sets the crops.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [UmbracoProperty(Constants.Conventions.Media.File)]
        public virtual ImageCropDataSet Crops { get; set; }
    }
}