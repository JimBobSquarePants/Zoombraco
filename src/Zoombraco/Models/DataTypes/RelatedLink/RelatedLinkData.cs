// <copyright file="RelatedLinkData.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    /// <summary>
    /// The related link data
    /// </summary>
    public class RelatedLinkData : RelatedLinkBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the related link is internal.
        /// </summary>
        public int? Internal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the link has been edited.
        /// </summary>
        public bool Edit { get; set; }

        /// <summary>
        /// Gets or sets the internal name.
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }
    }
}
