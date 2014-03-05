using OpenTK;
using System.Runtime.InteropServices;

namespace ManagedGL.Vertices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct VertexPosition : IVertex
    {
        [VertexElement(0), FieldOffset(0)]
        public Vector3 position;
    }
}
