// <copyright file="File.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System;
    using System.Web;
    using Our.Umbraco.Ditto;
    using Umbraco.Core;
    using Zoombraco.ComponentModel.Search;

    /// <summary>
    /// Represents a single published file in the media section.
    /// </summary>
    [DittoLazy]
    [UmbracoPicker]
    public class File : IEntity, IFile, IUrl
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

        /// <inheritdoc/>
        public virtual string Url { get; set; }

        /// <inheritdoc/>
        public virtual string UrlAbsolute()
        {
            // TODO: It might be possible to get the domain using the domain service.
            string root = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            return new Uri(new Uri(root, UriKind.Absolute), this.Url).ToString();
        }
    }
}