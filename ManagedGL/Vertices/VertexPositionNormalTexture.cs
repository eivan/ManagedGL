using OpenTK;
using System.Runtime.InteropServices;

namespace ManagedGL.Vertices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct VertexPositionNormalTexture : IVertex
    {
        [VertexElement(0), FieldOffset(0)]
        public Vector3 position;

        [VertexElement(1), FieldOffset(12)]
        public Vector3 normal;

        [VertexElement(2), FieldOffset(24)]
        public Vector2 coords;
    }}
