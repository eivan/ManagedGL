
using OpenTK;
using System.Runtime.InteropServices;
namespace ManagedGL.Vertices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct VertexPositionColor : IVertex
    {
        [VertexElement(0, "vs_in_pos"), FieldOffset(0)]
        public Vector3 position;

        [VertexElement(1, "vs_in_col"), FieldOffset(12)]
        public Vector3 color;

        public VertexPositionColor(Vector3 position, Vector3 color)
        {
            this.position = position;
            this.color = color;
        }
    }
}
