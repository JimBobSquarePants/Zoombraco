// <copyright file="IHomeService.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Services
{
    using ZoombracoDemo.Logic.Models;

    /// <summary>
    /// Provides methods for retrieving home documents from the data store
    /// </summary>
    public interface IHomeService
    {
        /// <summary>
        /// Gets the home page by id
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>The <see cref="Home"/></returns>
        Home GetById(int id);
    }
}
