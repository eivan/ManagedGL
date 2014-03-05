using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedGL.Shaders
{
    public class ShaderProgram : IDisposable
    {
#if DEBUG
        internal static ShaderProgram actual;
#endif
        Dictionary<string, Uniform> uniformLocations;

        /// <summary>
        /// Mutató a shader programhoz [ideiglenes]s
        /// </summary>
        internal readonly int Ptr;

        /// <summary>
        /// Felvett shaderek
        /// </summary>
        public List<Shader> ShadersAttached { get; internal set; }

        /// <summary>
        /// Új shader program létrehozása
        /// </summary>
        public ShaderProgram()
        {
            Ptr = GL.CreateProgram();
            ShadersAttached = new List<Shader>();
            uniformLocations = new Dictionary<string, Uniform>();
        }

        /// <summary>
        /// lefordított Shader hozzávételezése
        /// </summary>
        /// <param name="shader">az új, lefordított shader a programban</param>
        public void Attach(Shader shader)
        {
            GL.AttachShader(Ptr, shader.Ptr);
            ShadersAttached.Add(shader);
        }

        /// <summary>
        /// Lefordított kód összelinkelése
        /// </summary>
        public void Link(bool clear_shaders = false)
        {
            GL.LinkProgram(Ptr);

            String info;
            int result;
            GL.GetProgram(Ptr, GetProgramParameterName.LinkStatus, out result);
            GL.GetProgramInfoLog(Ptr, out info);
            if (result == -1)
                Trace.TraceError(info);
            else
                Trace.Write(info);

            if (clear_shaders)
                ShadersAttached.Clear();
        }

        /// <summary>
        /// Shader kijelölése használatra:
        /// a GPU olyan állapotba kerül, hogy ezen a shaderen veszi a módosításokat,
        /// illetve a shader segítségével jelenít meg.
        /// </summary>
        public void Begin()
        {
#if DEBUG
            if (actual == this)
                throw new InvalidOperationException("ShaderProgram already in use!");
            actual = this;
#endif
            GL.UseProgram(Ptr);
        }

        public void End()
        {
#if DEBUG
            if (actual != this)
                throw new InvalidOperationException("ShaderProgram already usused!");
            actual = null;
#endif
            GL.UseProgram(0);
        }

        public Uniform GetUniform(string uniformName)
        {
            Begin();

            if (!uniformLocations.ContainsKey(uniformName))
            {
                int location = GL.GetUniformLocation(Ptr, uniformName);

                //if ( location == 0 )
                //return null;

                uniformLocations[uniformName] = new Uniform(location, this);
            }

            End();

            return uniformLocations[uniformName];
        }

        #region IDisposable Members

        public void Dispose()
        {
#if DEBUG
            if (actual == this)
                actual = null;
#endif
            GL.DeleteProgram(Ptr);
        }

        #endregion

        /*protected void BindAttribs(params string[] pszAttribs)
        {
            for (int i = 0; i < pszAttribs.Length; ++i)
                GL.BindAttribLocation(Ptr, i, pszAttribs[i]);
        }*/
    }
}
