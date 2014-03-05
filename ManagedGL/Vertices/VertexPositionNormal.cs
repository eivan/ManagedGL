using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ManagedGL.Vertices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct VertexPositionNormal : IVertex
    {
        [VertexElement(0), FieldOffset(0)]
        public Vector3 position;

        [VertexElement(1), FieldOffset(12)]
        public Vector3 normal;
    }
}
