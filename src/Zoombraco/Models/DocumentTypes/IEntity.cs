// <copyright file="IEntity.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System;

    /// <summary>
    /// Defines the basic properties required to identify the object from the Umbraco back office.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the Umbraco Id for this content, media, or member.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the Umbraco node name for this content, media, or member.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the document type alias for this content, or media.
        /// </summary>
        string DocumentTypeAlias { get; set; }

        /// <summary>
        /// Gets or sets  the Umbraco node level for this content, or media.
        /// </summary>
        int Level { get; set; }

        /// <summary>
        /// Gets or sets  the Umbraco node created date for this content, media, or member.
        /// </summary>
        DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets  the Umbraco node updated date for this content, media, or member.
        /// </summary>
        DateTime UpdateDate { get; set; }
    }
}
