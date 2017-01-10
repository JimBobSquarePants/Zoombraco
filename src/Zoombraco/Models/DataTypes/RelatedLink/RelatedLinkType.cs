// <copyright file="RelatedLinkType.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Provides enumeration for the various link target options.
    /// </summary>
    public enum RelatedLinkType
    {
        /// <summary>
        /// Opens the linked document in the same frame as it was clicked (this is default)
        /// </summary>
        [Display(Description = "_self")]
        Internal,

        /// <summary>
        /// Opens the linked document in a new window or tab
        /// </summary>
        [Display(Description = "_blank")]
        External
    }
}