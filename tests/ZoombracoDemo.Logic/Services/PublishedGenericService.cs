// <copyright file="PublishedGenericService.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Services
{
    using Zoombraco.Helpers;
    using ZoombracoDemo.Logic.Models;

    /// <summary>
    /// An <see cref="IGenericService"/> implementation using the <see cref="ContentHelper"/>
    /// </summary>
    public class PublishedGenericService : IGenericService
    {
        private readonly ContentHelper contentHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedGenericService"/> class.
        /// </summary>
        public PublishedGenericService()
        {
            this.contentHelper = new ContentHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedGenericService"/> class.
        /// </summary>
        /// <param name="contentHelper">The content helper</param>
        public PublishedGenericService(ContentHelper contentHelper)
        {
            this.contentHelper = contentHelper;
        }

        /// <inheritdoc/>
        public Generic GetById(int id)
        {
            return this.contentHelper.GetById<Generic>(id);
        }
    }
}