using ManagedGL.Buffers;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedGL.Vertices
{
    public class VertexArray<V> : IVertexArray
        where V : struct,IVertex
    {
#if DEBUG
        static IVertexArray actual;
#endif
        internal readonly int Ptr;

        protected GenericGPUBuffer<V> vbo;

        public VertexArray(params V[] vertices)
        {
            GL.GenVertexArrays(1, out Ptr);
            Begin();

            vbo = new GenericGPUBuffer<V>(BufferTarget.ArrayBuffer);

            vbo.Begin();

            vbo.BufferData(vertices);

            var vertexDefinition = Factory<V>.VertexAttributes;
            foreach (var attribP in vertexDefinition)
                attribP.RegisterVertexAttribPointer();

            vbo.End();
            End();
        }

        public void Allocate(int count)
        {
            vbo.Allocate(count);
        }

        public void BufferData(params V[] array)
        {
            vbo.Begin();
            vbo.BufferData(array);
            vbo.End();
        }

        public void BufferSubData(int offset, params V[] array)
        {
            vbo.BufferSubData(offset, array);
        }

        public void Begin()
        {
#if DEBUG
            if (actual == this)
                throw new InvalidOperationException("VAO already in use!");
            actual = this;
#endif
            GL.BindVertexArray(Ptr);
        }

        public void End()
        {
#if DEBUG
            if (actual != this)
                throw new InvalidOperationException("VAO not in use anymore!");
            actual = null;
#endif
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            vbo.Dispose();
            GL.DeleteVertexArray(Ptr);
        }
    }
}
