using System;
using System.ComponentModel;
using System.Xml.Serialization;
using OpenTK;
using ManagedGL.Helpers;
using ManagedGL.Cameras;

namespace ManagedGL
{
    public abstract class Component : IDisposable
    {
        public Component()
        {
            Position = Vector3.Zero;
            Extent = Vector3.One;
            Name = "Névtelen";
        }

        public Component(string name)
        {
            Position = Vector3.Zero;
            Extent = Vector3.One;
            Name = name;
        }

        [Category("Kiterjedtség"), TypeConverter(typeof(ExpandableFieldsConverter<Vector3>))]
        public Vector3 Position
        {
            set;
            get;
        }

        [Category("Kiterjedtség"), TypeConverter(typeof(ExpandableFieldsConverter<Vector3>))]
        public Vector3 Extent
        {
            set;
            get;
        }

        [DefaultValue("Névtelen")]
        public String Name
        {
            set;
            get;
        }

        [XmlIgnore, Browsable(false)]
        public BoundingBox BoundingBox
        {
            get
            {
                var min = Position - Extent;
                var max = Position + Extent;
                return new BoundingBox(ref min, ref max);
            }
        }

        public override string ToString()
        {
            var cat = GetType().GetCustomAttributes(typeof(CategoryAttribute), false)[0] as CategoryAttribute;
            return String.Format("[{0}] <{1}>", Name, cat.Category);
        }

        /// <summary>
        /// A komponens frissítése
        /// </summary>
        public abstract void Update(Camera camera, double t);

        /// <summary>
        /// A komponens megjelenítése
        /// </summary>
        public abstract void Render(Camera camera, double t);

        /// <summary>
        /// A komponenst "elintézése"
        /// </summary>
        public virtual void Dispose()
        {
            OnUnload();
        }

        public abstract void OnLoad();

        public abstract void OnUnload();
    }
}
