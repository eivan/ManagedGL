using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedGL.Vertices
{
    public class VertexAttributePointer
    {
        internal VertexAttributePointer(
            int usageIndex,
            int stride,
            int offset,
            int size,
            //VertexElementUsage vertexElementUsage,
            string name, //fur shaderz
            bool normalized = false,
            VertexAttribPointerType attribPointerType = VertexAttribPointerType.Float)
        {
            this.Stride = stride;
            this.Offset = offset;
            this.Size = size;
            //this.VertexElementUsage = vertexElementUsage;
            this.UsageIndex = usageIndex;
            this.VertexAttribPointerType = attribPointerType;
            this.Normalized = normalized;
            this.Name = name;
        }

        //public readonly VertexElementUsage VertexElementUsage;
        public readonly int Size;
        public readonly VertexAttribPointerType VertexAttribPointerType;
        public readonly bool Normalized;
        public readonly int UsageIndex;
        public readonly int Offset;
        public readonly int Stride;
        public readonly string Name;

        internal void RegisterVertexAttribPointer(){
            GL.EnableVertexAttribArray(UsageIndex);
            GL.VertexAttribPointer(
                UsageIndex,
                Size,//szamitott, component count
                VertexAttribPointerType,
                Normalized,
                Stride,//calculated in bytes
                Offset);//calculated (sizeof(V))
        }

        internal void BindAttribLocation(Shaders.ShaderProgram program)
        {
            GL.BindAttribLocation(program.Ptr, UsageIndex, Name);
        }
    }
}
