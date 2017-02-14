// <copyright file="NestedComponent.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System;
    using Our.Umbraco.Ditto;

    /// <summary>
    /// Represents a single published component in the content section.
    /// </summary>
    [DittoLazy]
    [DittoDocTypeFactory]
    public class NestedComponent : IEntity
    {
        /// <inheritdoc/>
        public virtual int Id { get; set; }

        /// <inheritdoc/>
        public virtual string Name { get; set; }

        /// <inheritdoc/>
        public virtual string DocumentTypeAlias { get; set; }

        /// <inheritdoc/>
        public virtual int Level { get; set; }

        /// <inheritdoc/>
        public virtual DateTime CreateDate { get; set; }

        /// <inheritdoc/>
        public virtual DateTime UpdateDate { get; set; }
    }
}