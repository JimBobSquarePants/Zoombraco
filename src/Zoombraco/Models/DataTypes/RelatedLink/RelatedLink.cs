// <copyright file="RelatedLink.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    /// <summary>
    /// Represents a single related link.
    /// </summary>
    public class RelatedLink : RelatedLinkBase
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public int? Id { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the link is deleted
        /// </summary>
        internal bool IsDeleted { get; set; }
    }
}
