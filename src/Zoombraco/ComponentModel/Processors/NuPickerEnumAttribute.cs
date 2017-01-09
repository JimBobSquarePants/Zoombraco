// <copyright file="NuPickerEnumAttribute.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.ComponentModel.Processors
{
    using nuPickers;

    using Our.Umbraco.Ditto;

    /// <summary>
    /// Provides a unified way of converting the <see cref="Picker"/> type to the given enum. />
    /// </summary>
    public class NuPickerEnumAttribute : DittoMultiProcessorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuPickerEnumAttribute"/> class.
        /// </summary>
        public NuPickerEnumAttribute()
            : base(new DittoProcessorAttribute[] { new NuPickerEnumConverterAttribute(), new EnumAttribute() })
        {
        }

        /// <summary>
        /// Returns the correct value to pass to the <see cref="EnumAttribute"/>
        /// </summary>
        private class NuPickerEnumConverterAttribute : DittoProcessorAttribute
        {
            /// <inheritdoc/>
            public override object ProcessValue()
            {
                Picker picker = this.Value as Picker;
                return picker != null ? picker.SavedValue : this.Value;
            }
        }
    }
}
