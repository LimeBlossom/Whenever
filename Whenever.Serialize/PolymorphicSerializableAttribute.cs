using System;

namespace Serialization
{
    [AttributeUsage(AttributeTargets.Class |
                           AttributeTargets.Struct)
    ]
    public class PolymorphicSerializableAttribute : Attribute
    {
        public readonly string typeKey;

        public PolymorphicSerializableAttribute(string typeKey)
        {
            this.typeKey = typeKey;
        }
    }
}