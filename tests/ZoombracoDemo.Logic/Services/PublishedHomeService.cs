// <copyright file="PublishedHomeService.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Services
{
    using Zoombraco.Helpers;
    using ZoombracoDemo.Logic.Models;

    /// <summary>
    /// An <see cref="IHomeService"/> implementation using the <see cref="ContentHelper"/>
    /// </summary>
    public class PublishedHomeService : IHomeService
    {
        private readonly ContentHelper contentHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedHomeService"/> class.
        /// </summary>
        public PublishedHomeService()
        {
            this.contentHelper = new ContentHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedHomeService"/> class.
        /// </summary>
        /// <param name="contentHelper">The content helper</param>
        public PublishedHomeService(ContentHelper contentHelper)
        {
            this.contentHelper = contentHelper;
        }

        /// <inheritdoc/>
        public Home GetById(int id)
        {
            return this.contentHelper.GetById<Home>(id);
        }
    }
}