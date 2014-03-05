using System;
namespace ManagedGL.Vertices
{
    public interface IVertexArray : IDisposable
    {
        void Allocate(int count);
        void Begin();
        void End();
    }
}
