// <copyright file="RelatedLinkBase.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    /// <summary>
    /// A base class for creating related links
    /// </summary>
    public class RelatedLinkBase
    {
        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to open the link in a new window.
        /// </summary>
        public bool NewWindow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the link is internal.
        /// </summary>
        public bool IsInternal { get; set; }

        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public RelatedLinkType Type { get; set; }
    }
}