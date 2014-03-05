using ManagedGL.Cameras;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ManagedGL
{
    /// <summary>
    /// Színtér osztálya
    /// </summary>
    [Serializable]
    public class Scene : IDisposable
    {
        #region Static

        static Scene current;
        [XmlIgnore]
        public static Scene Current
        {
            get { return current; }
            set { current = value; }
        }

        private static void Save<T>(string fileName, T map) where T : Scene
        {
            map.Save();
        }

        public static T Open<T>(string fileName, params Type[] componentTypes) where T : Scene
        {
            T ret;

            XmlSerializer serializer = new XmlSerializer(typeof(T), componentTypes);
            using (TextReader reader = new StreamReader(fileName))
            {
                ret = serializer.Deserialize(reader) as T;
                reader.Close();
            }

            ret.fileName = fileName;
            return ret;
        }

        //private static readonly Type[] componentTypes = Assembly.GetExecutingAssembly()
        //.GetTypes();
        //.Where(t => t.IsSubclassOf(typeof(Core.Component)))
        //.ToArray<Type>();

        #endregion

        #region File
        string fileName;

        private Type[] componentTypes;

        [XmlIgnore]
        public string FileName
        {
            get { return fileName; }
            protected set { fileName = value; }
        }

        /// <summary>
        /// Színtér elmentése
        /// </summary>
        public void Save()
        {
            if (fileName == null || fileName == "")
                Trace.Fail("A színteret nem lehet menteni, nincs megadva fájlnév.");

            // Write to file
            XmlSerializer serializer = new XmlSerializer(this.GetType(), componentTypes);
            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, this);
                writer.Close();
            }
        }

        /// <summary>
        /// Színtér elmentése másként
        /// </summary>
        /// <param name="fileName">fájlnév</param>
        public void SaveAs(string fileName)
        {
            this.fileName = fileName;
            Save();
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Új, üres Színtér létrehozása
        /// </summary>
        public Scene(params Type[] componentTypes)
        {
            Components = new List<Component>();
            Cameras = new List<Camera>();
            current = this;

            this.componentTypes = componentTypes;
        }

        #endregion

        #region Properties

        public List<Camera> Cameras { get; set; }
        public List<Component> Components { get; set; }

        [XmlIgnore]
        public Camera ActiveCamera { get; set; }
        [XmlIgnore]
        public Component ActiveComponent { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            if (fileName != null)
                return fileName;

            return base.ToString();
        }

        public virtual void Update(double t)
        {
            foreach (var c in Components)
                c.Update(ActiveCamera, t);
        }

        public virtual void Render(double t)
        {
            if (ActiveCamera != null)
            {
                ActiveCamera.UpdateProjection(false);
                ActiveCamera.UpdateView(false);
            }

            foreach (var c in Components)
            {
                //TODO rendermanager for each (shader, trasformation, model)
                c.Render(ActiveCamera, t);
            }
        }

        public virtual void OnLoad()
        {
            foreach (var c in Components)
                c.OnLoad();
        }

        public virtual void OnUnload()
        {
            foreach (var c in Components)
                c.OnUnload();
        }

        #endregion

        public void Dispose()
        {
            foreach (var c in Components)
                c.Dispose();
        }

        /// <summary>
        /// Kamera váltása (a sorban a következő)
        /// </summary>
        public void NextCamera()
        {
            int i;
            for (i = 0; i < Cameras.Count && Cameras[i] != ActiveCamera; ++i) ;
            ActiveCamera = Cameras[(i + 1) % Cameras.Count];
        }

        /// <summary>
        /// Kamera váltása (a sorban a következő) Viewport paraméterekkel
        /// </summary>
        /// <param name="x">x elhelyezkedés</param>
        /// <param name="y">y elhelyezkedés</param>
        /// <param name="Width">megjelenítés szélessége</param>
        /// <param name="Height">megjelenítés magassága</param>
        public void NextCamera(int x, int y, int Width, int Height)
        {
            int i;
            for (i = 0; i < Cameras.Count && Cameras[i] != ActiveCamera; ++i) ;
            ActiveCamera = Cameras[(i + 1) % Cameras.Count];
            ActiveCamera.SetViewPort(x, y, Width, Height);
        }
    }
}
