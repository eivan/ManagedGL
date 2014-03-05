using ManagedGL.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedGL
{
    public static class VertexExtensions
    {
        public static IVertexArray ToVertexArray<V>(this V[] arrayOfVertices)
            where V: struct, IVertex
        {
            return Factory<V>.CreateVertexArray(arrayOfVertices);
        }
    }
}
