using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;

namespace ManagedGL.Shaders
{
    /// <summary>
    /// Shader programoktól név-string alapján (GetUniform) lekérdezhetjük egy változóját
    /// Ezeket utána ennek a class-nak a segítségével használhatjuk
    /// </summary>
    public class Uniform
    {
        internal readonly int Ptr;
        internal ShaderProgram parent;

        internal Uniform(int location, ShaderProgram parent)
        {
            this.Ptr = location;
            this.parent = parent;
        }

        #warning TODO: add mooooooore!
        // Probléma: Kényelmes használat vs hatékonyság:
        // Legyen itt referencia a programról, ami alapján ellenőrizhető legyen, hogy Use-olva van e?
        // Megoldás: Debug módban sírjon, hogy épp többször van use-olva!

        public void SetValue(int value)
        {
            //TODO parent.Use();
            GL.Uniform1(Ptr, value);
        }

        public void SetValue(float value)
        {
            //TODO parent.Use();
            GL.Uniform1(Ptr, value);
        }

        public void SetValue(bool value)
        {
            //parent.Use();
            GL.Uniform1(Ptr, value ? 1 : 0);// TODO
        }

        public void SetValue(OpenTK.Vector3 value)
        {
            //parent.Use();
            GL.Uniform3(Ptr, value);
        }

        public void SetValue(ref OpenTK.Matrix4 value)
        {
            //parent.Use();
            GL.UniformMatrix4(Ptr, false, ref value);
            //Trace.TraceInformation(Enum.GetName(typeof(ErrorCode), GL.GetError()));
        }

        /*public void SetValue(OpenTK.Matrix4[] value)
        {
            GL.UniformMatrix4(ptr, value.Length, false, ref value[0]); // maybe doesnt work
        }*/
    }
}
