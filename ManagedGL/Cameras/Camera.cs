using ManagedGL.Helpers;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ManagedGL.Cameras
{
    /// <summary>
    /// Kamera osztály, hogy a Színtéren körülnézhessünk.
    /// </summary>
    [XmlInclude(typeof(MobileCamera))]
    public class Camera
    {
        #region Private Fields

        private DirtyState dirtyState = DirtyState.All;

        private Vector3 up;
        private Vector3 direction;
        private Vector3 position;

        private float farPlane = 1000.0f;
        private float nearPlane = 0.06f;
        private float aspectRatio;
        private float fieldOfView;


        private Matrix4 projection;
        private Matrix4 view;
        private Matrix4 viewProjectionMatrix;

        #endregion

        public Camera()
        {
            aspectRatio = (800 / (float)600);

            fieldOfView = MathHelper.PiOver4;

            position = Vector3.Zero;
            direction = Vector3.UnitZ;
            up = Vector3.UnitY;

            dirtyState = DirtyState.All;
        }

        #region Projection

        [Category("Vetítés (Projection)"), Description("Látószög")]
        public float FieldOfView
        {
            get { return fieldOfView; }
            set
            {
                fieldOfView = value;

                dirtyState |= DirtyState.AllProj;
            }
        }

        [Category("Vetítés (Projection)"), Description("Nézetarány")]
        public float AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                aspectRatio = value;

                dirtyState |= DirtyState.AllProj;
            }
        }

        [Category("Vetítés (Projection)"), Description("Vetítés síkja")]
        public float NearPlane
        {
            get { return nearPlane; }
            set
            {
                nearPlane = value;

                dirtyState |= DirtyState.AllProj;
            }
        }

        [Category("Vetítés (Projection)"), Description("Távoli sík321űkl")]
        public float FarPlane
        {
            get { return farPlane; }
            set
            {
                farPlane = value;

                dirtyState |= DirtyState.AllProj;
            }
        }

        [XmlIgnore, Browsable(false)]
        public Matrix4 Projection
        {
            get
            {
                if ((dirtyState & DirtyState.ProjDirty) ==
                    DirtyState.ProjDirty)
                {
                    Matrix4.CreatePerspectiveFieldOfView(
                        fieldOfView,
                        aspectRatio,
                        nearPlane,
                        farPlane,
                        out projection);

                    dirtyState |= DirtyState.AllProj;
                    dirtyState ^= DirtyState.ProjDirty;
                }

                return projection;
            }
        }

        #endregion

        #region View

        [Category("Nézet (View)"), TypeConverter(typeof(ExpandableFieldsConverter<Vector3>)), Description("Célpont beállítása")]
        public Vector3 Target
        {
            get { return position + direction; }
            set
            {
                direction = (value - position);
                direction.Normalize();

                dirtyState |= DirtyState.AllView;
            }
        }

        [Category("Nézet (View)"), TypeConverter(typeof(ExpandableFieldsConverter<Vector3>)), Description("Irányvektor")]
        public Vector3 Direction
        {
            get { return direction; }
            set
            {
                direction = value;

                dirtyState |= DirtyState.AllView;
            }
        }

        [Category("Nézet (View)"), TypeConverter(typeof(ExpandableFieldsConverter<Vector3>)), Description("Lokális 'fel' irány")]
        public Vector3 Up
        {
            get { return up; }
            set
            {
                up = value;

                dirtyState |= DirtyState.AllView;
            }
        }

        [XmlIgnore, Browsable(false)]
        public Vector3 Side
        {
            get { return Vector3.Cross(up, direction); }
        }

        [Category("Nézet (View)"), TypeConverter(typeof(ExpandableFieldsConverter<Vector3>)), Description("A kamera elhelyezkedése a térben avagy nézőpont")]
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;

                dirtyState |= DirtyState.AllView;
            }
        }

        [XmlIgnore, Browsable(false)]
        public Matrix4 View
        {
            get
            {
                if ((dirtyState & DirtyState.ViewDirty) ==
                    DirtyState.ViewDirty)
                {
                    view = Matrix4.LookAt(
                        position,
                        position + direction,
                        up);

                    dirtyState |= DirtyState.AllView;
                    dirtyState ^= DirtyState.ViewDirty;
                }

                return view;
            }
        }

        [XmlIgnore, Browsable(false)]
        public Matrix4 ViewProjection
        {
            get
            {
                if ((dirtyState & DirtyState.ViewProjDirty) ==
                    DirtyState.ViewProjDirty)
                {
                    viewProjectionMatrix = Matrix4.Mult(View, Projection);

                    dirtyState ^= DirtyState.ViewProjDirty;
                    dirtyState |= DirtyState.InvViewProjDirty;
                }
                return viewProjectionMatrix;
            }
        }

        #endregion

        #region OpenGL-el kapcsolatos függvényeket tartalmaz

        /// <summary>
        /// Látótér beállítása
        /// </summary>
        /// <param name="x">A látótér bal alsó pontjának koordinátája</param>
        /// <param name="y">A látótér bal alsó pontjának ordinátája</param>
        /// <param name="width">A látótér szélessége</param>
        /// <param name="height">A látótér magassága</param>
        public void SetViewPort(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
            AspectRatio = width / (float)height;
        }

        /// <summary>
        /// Látótér beállítása
        /// </summary>
        /// <param name="x">A látótér bal alsó pontjának koordinátája</param>
        /// <param name="y">A látótér bal alsó pontjának ordinátája</param>
        /// <param name="width">A látótér szélessége</param>
        /// <param name="height">A látótér magassága</param>
        /// <param name="nearPlane">A legközelebbi objektumok határa</param>
        /// <param name="farPlane">A legtávolabbi objektumok határa</param>
        public void SetViewPort(int x, int y, int width, int height, float nearPlane, float farPlane)
        {
            GL.Viewport(x, y, width, height);

            this.AspectRatio = width / (float)height;
            this.NearPlane = nearPlane;
            this.FarPlane = farPlane;
        }

        /// <summary>
        /// Aktuális projekció feltöltése a megjelenítőnek
        /// </summary>
        /// <param name="onlyIfItGotDirty">Ha nem volt újraszámolás, esetleg nem szükséges újboli feltöltés</param>
        public void UpdateProjection(bool onlyIfItGotDirty)
        {
            // változás esetén az új kiszámítása (identikus a Projection property get ágában találhatóval)
            if ((dirtyState & DirtyState.ProjDirty) == DirtyState.ProjDirty)
            {
                Matrix4.CreatePerspectiveFieldOfView(
                    fieldOfView,
                    aspectRatio,
                    nearPlane,
                    farPlane,
                    out projection);

                dirtyState |= DirtyState.AllProj;
                dirtyState ^= DirtyState.ProjDirty;

                return;
            }

            if (onlyIfItGotDirty)
                return;

            //TODO event: got updated
        }

        /// <summary>
        /// Aktuális nézet feltöltése a megjelenítőnek
        /// </summary>
        /// <param name="onlyIfItGotDirty">Ha nem volt újraszámolás, esetleg nem szükséges újboli feltöltés</param>
        public void UpdateView(bool onlyIfItGotDirty)
        {
            // változás esetén az új kiszámítása (identikus a View property get ágában találhatóval)
            if ((dirtyState & DirtyState.ViewDirty) == DirtyState.ViewDirty)
            {
                view = Matrix4.LookAt(
                    position,
                    position + direction,
                    up);

                dirtyState |= DirtyState.AllView;
                dirtyState ^= DirtyState.ViewDirty;
            }

            if (onlyIfItGotDirty)
                return;

            return;

            //TODO event: got updated
        }

        #endregion

        #region Rejtett/Privát: DirtyState

        [Flags]
        private enum DirtyState
        {
            None = 0,
            ViewDirty = 1,
            ProjDirty = 2,
            ViewProjDirty = 4,
            InvViewDirty = 8,
            InvProjDirty = 16,
            InvViewProjDirty = 32,
            AllView = ViewDirty | InvViewDirty |
                      ViewProjDirty | InvViewProjDirty,
            AllProj = ProjDirty | InvProjDirty |
                      ViewProjDirty | InvViewProjDirty,
            All = ViewDirty | ProjDirty | ViewProjDirty |
                  InvViewDirty | InvProjDirty | InvViewProjDirty,
        }

        #endregion

        [Description("A kamera neve")]
        public string Name { get; set; }

        public override string ToString()
        {
            return String.Format("{0} <{1}>", Name, base.ToString());
        }
    }
}
