// <copyright file="EnumExtensions.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Extensions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Extensions methods for <see cref="Enum"/> class.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Extends the <see cref="T:System.Enum"/> type to return the display attribute for the given type.
        /// Useful for when the type to match in the data source contains spaces.
        /// </summary>
        /// <param name="expression">The given <see cref="T:System.Enum"/> that this method extends.</param>
        /// <returns>A string containing the <see cref="T:System.Enum"/>'s description attribute.</returns>
        public static string ToDisplay(this Enum expression)
        {
            DisplayAttribute[] displayAttribute =
                (DisplayAttribute[])expression
                    .GetType()
                    .GetField(expression.ToString())
                    .GetCustomAttributes(typeof(DisplayAttribute), false);

            return displayAttribute.Length > 0 ? displayAttribute[0].Name : expression.ToString();
        }
    }
}