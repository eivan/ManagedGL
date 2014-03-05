using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;

namespace ManagedGL.Shaders
{
    public class Shader : IDisposable
    {
        internal ShaderType shaderType;

        /// <summary>
        /// Shader mutatója [ideiglenes]
        /// </summary>
        internal readonly int Ptr;

        /// <summary>
        /// Új shader létrehozása
        /// </summary>
        /// <param name="shaderType">A shader típusa</param>
        /// <param name="code">a shader kódja</param>
        public Shader(ShaderType shaderType, string code)
        {
            this.shaderType = shaderType;

            Ptr = GL.CreateShader(shaderType);
            GL.ShaderSource(Ptr, code);
            GL.CompileShader(Ptr);

            string info;
            int status_code;

            GL.GetShaderInfoLog(Ptr, out info);
            GL.GetShader(Ptr, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                Trace.Fail("Shader hiba!", info);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (Ptr != 0)
                GL.DeleteShader(Ptr);
        }

        #endregion
    }

}
