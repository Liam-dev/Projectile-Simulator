using System;
using System.ComponentModel;

namespace Simulator.Converters
{
    /// <summary>
    /// Converter for expandable objects which can be serialized.
    /// </summary>
    class SerializableExpandableObjectConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType is string)
            {
                return false;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType is string)
            {
                return false;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }
    }
}