using OpenTK;
using System.Drawing;

namespace ManagedGL
{
    public static class ColorExtensions
    {
        public static int ToRgba32( this Color c )
        {
            return (int)((c.A << 24) | (c.B << 16) | (c.G << 8) | c.R);
        }

        public static Vector4 ToVector4( this Color c )
        {
            return (new Vector4( c.A, c.B, c.G, c.R )) / 255f;
        }

        public static float[] ToFloatArray( this Color c )
        {
            return new float[] { c.A / 255f, c.B / 255f, c.G / 255f, c.R / 255f };
        }
    }
}
