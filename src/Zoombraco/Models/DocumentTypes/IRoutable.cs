// <copyright file="IRoutable.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using Umbraco.Core.Models;

    /// <summary>
    /// Encapsulates properties that allow the manipulation of page urls.
    /// </summary>
    public interface IRoutable
    {
        /// <summary>
        /// Gets or sets an alternative url than the one generated from the name.
        /// </summary>
        string AlternativeUrl { get; set; }

        /// <summary>
        /// Gets or sets an additional url to the one generated from the name.
        /// </summary>
        string AdditionalUrl { get; set; }

        /// <summary>
        /// Gets or sets the node to perform a temporary 302 redirect to.
        /// </summary>
        IPublishedContent TemporaryRedirect { get; set; }

        /// <summary>
        /// Gets or sets the node to perform an internal redirect to. The url does not change.
        /// </summary>
        IPublishedContent TransparentRedirect { get; set; }
    }
}
