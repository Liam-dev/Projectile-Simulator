using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Simulator.Converters
{
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
