// <copyright file="IGenericService.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Services
{
    using ZoombracoDemo.Logic.Models;

    /// <summary>
    /// Provides methods for retrieving generic documents from the data store
    /// </summary>
    public interface IGenericService
    {
        /// <summary>
        /// Gets the generic page by id
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>The <see cref="Generic"/></returns>
        Generic GetById(int id);
    }
}
