using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace ManagedGL.Helpers
{
    /// <summary>
    /// A mezőket is megjeleníti PropertyGridben és szerkeszthetővé teszi kinyitható formában
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpandableFieldsConverter<T> : ExpandableObjectConverter
    {
        private readonly PropertyDescriptorCollection propertyDescriptions;

        public ExpandableFieldsConverter()
        {
            Type type = typeof(T);

            FieldInfo[] fields = type.GetFields();

            var propDescriptors = new List<PropertyDescriptor>();

            for (int i = 0; i < fields.Length; i++)
            {
                if (!fields[i].IsStatic && fieldInfoHasBrowsableAttributeTrue(fields[i]))
                    propDescriptors.Add(new FieldPropertyDescriptor(fields[i]));
            }

            propertyDescriptions = new PropertyDescriptorCollection(propDescriptors.ToArray());
        }

        private static bool fieldInfoHasBrowsableAttributeTrue(FieldInfo field)
        {
            object[] attrs = field.GetCustomAttributes(true);
            BrowsableAttribute brattribute;

            for (int i = 0; i < attrs.Length; i++)
            {
                if ((brattribute = attrs[i] as BrowsableAttribute) != null)
                {
                    return brattribute.Browsable;
                }
            }
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value,
                                                                   Attribute[] attributes)
        {
            PropertyDescriptorCollection properties = base.GetProperties(context, value, attributes);
            PropertyDescriptorCollection fields = propertyDescriptions.Sort();

            foreach (PropertyDescriptor pd in properties)
            {
                fields.Add(pd);
            }

            return fields;
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return base.GetPropertiesSupported(context);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return null;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (value != null)
                return value.ToString();
            return "";
        }

        #region Nested type: FieldPropertyDescriptor

        internal class FieldPropertyDescriptor : PropertyDescriptor
        {
            #region FieldPropertyDescriptor Fields

            private readonly FieldInfo _field;
            private readonly MemberInfo _member;

            #endregion

            #region FieldPropertyDescriptor Constructors

            public FieldPropertyDescriptor(FieldInfo field)
                : base(field.Name, new Attribute[] { new BrowsableAttribute(true) })
            {
                _field = field;
                _member = field;
                // Evaluation version.
            }

            #endregion

            #region FieldPropertyDescriptor Methods

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override bool Equals(object obj)
            {
                var memberPropertyDescriptor = obj as FieldPropertyDescriptor;
                if (memberPropertyDescriptor == null)
                {
                    return false;
                }
                else
                {
                    return memberPropertyDescriptor._member.Equals(_member);
                }
            }

            public override object GetValue(object component)
            {
                return _field.GetValue(component);
                // Evaluation version.
            }

            public override void SetValue(object component, object value)
            {
                _field.SetValue(component, value);
                base.OnValueChanged(component, EventArgs.Empty);
            }

            #endregion

            #region FieldPropertyDescriptor Properties

            public override bool IsReadOnly
            {
                get { return false; }
            }

            public override Type ComponentType
            {
                get { return _member.DeclaringType; }
            }

            public override Type PropertyType
            {
                get { return _field.FieldType; }
            }

            #endregion

            public override void ResetValue(object component)
            {
                throw new NotImplementedException();
            }

            public override bool ShouldSerializeValue(object component)
            {
                return fieldInfoHasBrowsableAttributeTrue(_field);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        #endregion
    }
}
