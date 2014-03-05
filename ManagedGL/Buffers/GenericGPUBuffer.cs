using OpenTK.Graphics.OpenGL4;
using System;
using System.Runtime.InteropServices;

namespace ManagedGL.Buffers
{
    public class GenericGPUBuffer<T> : GPUBuffer where T : struct
    {
        #region Constructors

        /// <summary>
        /// Létrehoz egy új buffert
        /// </summary>
        /// <param name="bufferTarget">A buffer típusa, amibe a BufferData-val tölthetünk</param>
        public GenericGPUBuffer(BufferTarget bufferTarget, T[] array)
            : base(Marshal.SizeOf(typeof(T)), bufferTarget)
        {
            BufferData(array);
        }

        /// <summary>
        /// Létrehoz egy új buffert
        /// </summary>
        /// <param name="bufferTarget">A buffer típusa, amibe a BufferData-val tölthetünk</param>
        public GenericGPUBuffer(BufferTarget bufferTarget, BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
            : base(Marshal.SizeOf(typeof(T)), bufferTarget, bufferUsageHint)
        {
        }

        #endregion

        #region Methods - Buffer

        /// <summary>
        /// Bizonyos mennyiségű adat a bufferbe irányítása
        /// </summary>
        /// <param name="array">adatok</param>
        public void BufferData(params T[] array)
        {
#if DEBUG
            if (actual != this)
                throw new InvalidOperationException("Buffer is not bound!!");
#endif

            int addedSize = array.Length * TypeSize;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset">in bytes</param>
        /// <param name="array"></param>
        public void BufferSubData(int offset, params T[] array)
        {
#if DEBUG
            if (actual != this)
                throw new InvalidOperationException("Buffer is not bound!!");
#endif

            int addedSize = array.Length * TypeSize;
#if DEBUG
            int desiredSize = GetBufferSize() + addedSize;
#endif
            GL.BufferSubData<T>(bufferTarget, (IntPtr)offset, (IntPtr)(addedSize), array);

            buffer_size = GetBufferSize();
#if DEBUG
            if (buffer_size != desiredSize)
                throw new ApplicationException(String.Format(
                    "Hiba a buffer feltöltésénél. Feltölteni kívánt mennyiség: {0} byte, feltöltött: {1} byte.",
                    desiredSize, buffer_size));
#endif
        }

        #endregion
    }
}
