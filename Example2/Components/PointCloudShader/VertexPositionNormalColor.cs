using ManagedGL.Vertices;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Example2.Components.PointCloudShader
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct VertexPositionNormalColor : IVertex
    {
        [VertexElement(0, "position"), FieldOffset(0)]
        public Vector3 position;

        [VertexElement(1, "normal"), FieldOffset(12)]
        public Vector3 normal;

        [VertexElement(2, "color"), FieldOffset(24)]
        public Vector4 color;

        public VertexPositionNormalColor(Vector3 position, Vector3 normal, Vector3 color)
        {
            this.position = position;
            this.normal = normal;
            this.color = new Vector4(color, 1);
        }

        public VertexPositionNormalColor(VertexPositionNormal v, Vector3 color)
        {
            this.position = v.position;
            this.normal = v.normal;
            this.color = new Vector4(color, 1);
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct VertexPNTBC : IVertex
    {
        [VertexElement(0, "position"), FieldOffset(0)]
        public Vector3 position;

        [VertexElement(1, "normal"), FieldOffset(12)]
        public Vector3 normal;

        [VertexElement(2, "tangent"), FieldOffset(24)]
        public Vector3 tangent;

        [VertexElement(3, "binormal"), FieldOffset(36)]
        public Vector3 binormal;

        [VertexElement(4, "color"), FieldOffset(48)]
        public Vector4 color;
    }
}
