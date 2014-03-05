using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ManagedGL.Buffers
{
    public class GPUBuffer : IDisposable
    {
        internal static object actual;

        #region Fields
        public readonly int TypeSize;

        internal readonly BufferTarget bufferTarget;

        protected int buffer_size = 0;

        protected BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw;

        #endregion

        internal readonly int Ptr;

        #region Constructors

        public GPUBuffer(int typeSize, BufferTarget bufferTarget, BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
            : this(bufferTarget, bufferUsageHint)
        {
            this.TypeSize = typeSize;
        }

        public GPUBuffer(BufferTarget bufferTarget, BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
        {
            this.bufferTarget = bufferTarget;
            this.bufferUsageHint = bufferUsageHint;

            GL.GenBuffers(1, out Ptr);

            Trace.TraceInformation("{0} new({1}, {2})", ToString(), bufferTarget, bufferUsageHint);
        }

        #endregion

        #region Methods - Buffer

        public void Allocate(int count)
        {
#if DEBUG
            if (actual != this)
                throw new InvalidOperationException("Buffer not already bound!");
            actual = this;
#endif

            int addedSize = count * TypeSize;
#if DEBUG
            int desiredSize = GetBufferSize() + addedSize;
#endif
            GL.BufferData(bufferTarget, (IntPtr)(addedSize), IntPtr.Zero, bufferUsageHint);

            buffer_size = GetBufferSize();
#if DEBUG
            if (buffer_size != desiredSize)
                throw new ApplicationException(String.Format(
                    "Hiba a buffer feltöltésénél. Feltölteni kívánt mennyiség: {0} byte, feltöltött: {1} byte.",
                    desiredSize, buffer_size));
#endif

#if TRACE_BUFFER
            Trace.TraceInformation("{0} BufferData", ToString());
            Trace.Indent();
            {
                Trace.WriteLine(string.Format("addedSize: {0} byte", addedSize));
#if DEBUG
                Trace.WriteLine(string.Format("GL.GetError(): {0} byte", GL.GetError()));
#endif
            }
            Trace.Unindent();
#endif
        }

        public void GBufferData<V>(params V[] array) where V : struct
        {
#if DEBUG
            if (actual != this)
                throw new InvalidOperationException("Buffer is not bound!!");
#endif

            int addedSize = array.Length * Marshal.SizeOf(typeof(V));
#if DEBUG
            int desiredSize = /*GetBufferSize() + */addedSize;
#endif
            GL.BufferData(bufferTarget, (IntPtr)(addedSize), array, bufferUsageHint);

            buffer_size = GetBufferSize();
#if DEBUG
            if (buffer_size != desiredSize)
                throw new ApplicationException(String.Format(
                    "Hiba a buffer feltöltésénél. Feltölteni kívánt mennyiség: {0} byte, feltöltött: {1} byte.",
                    desiredSize, buffer_size));
#endif
        }

        public void GBufferSubData<V>(int offset, params V[] array) where V : struct
        {
#if DEBUG
            if (actual != this)
                throw new InvalidOperationException("Buffer is not bound!!");
#endif

            int addedSize = array.Length * Marshal.SizeOf(typeof(V));
#if DEBUG
            int desiredSize = GetBufferSize() + addedSize;
#endif
            GL.BufferSubData<V>(bufferTarget, (IntPtr)offset, (IntPtr)(addedSize), array);

            buffer_size = GetBufferSize();
#if DEBUG
            if (buffer_size != desiredSize)
                throw new ApplicationException(String.Format(
                    "Hiba a buffer feltöltésénél. Feltölteni kívánt mennyiség: {0} byte, feltöltött: {1} byte.",
                    desiredSize, buffer_size));
#endif
        }

        public void Begin()
        {
#if DEBUG
            if (actual == this)
                throw new InvalidOperationException("Buffer already bound!");
            actual = this;
#endif
            GL.BindBuffer(bufferTarget, Ptr);
        }

        public void End()
        {
#if DEBUG
            if (actual != this)
                throw new InvalidOperationException("Buffer already unbound!");
            actual = null;
#endif
            GL.BindBuffer(bufferTarget, Ptr);
        }

        public int GetBufferSize()
        {
            int size;
            GL.GetBufferParameter(bufferTarget, BufferParameterName.BufferSize, out size);
            return size;
        }

        #endregion

        #region Properties

        public int Size
        {
            get { return buffer_size; }
        }

        public int Length
        {
            get { return buffer_size / TypeSize; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (actual == this)
                actual = null;

            if (Ptr != 0)
                GL.DeleteBuffer(Ptr);
        }

        #endregion
    }
}
