// <copyright file="NuPickerAttribute.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.ComponentModel.Processors
{
    using System;
    using System.Linq;

    using nuPickers;

    using Our.Umbraco.Ditto;

    /// <summary>
    /// Provides a unified way of converting the <see cref="Picker"/> type to the given <see cref="Type"/>
    /// </summary>
    public class NuPickerAttribute : DittoMultiProcessorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuPickerAttribute"/> class.
        /// </summary>
        public NuPickerAttribute()
            : base(new DittoProcessorAttribute[] { new NuPickerConverterAttribute(), new UmbracoPickerAttribute() })
        {
        }

        /// <summary>
        /// Returns the correct value to pass to the <see cref="UmbracoPickerAttribute"/>
        /// </summary>
        private class NuPickerConverterAttribute : DittoProcessorAttribute
        {
            /// <inheritdoc/>
            public override object ProcessValue()
            {
                Picker picker = this.Value as Picker;
                return picker != null && picker.PickedKeys.Any() ? picker.PickedKeys : this.Value;
            }
        }
    }
}