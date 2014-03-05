using OpenTK;

namespace ManagedGL
{
    /// <summary>
    /// Egy téglatest alakú térrészt kijelölő struktúra. Élei a tengelyekkel párhuzamosak (Axis-Aligned)
    /// Felhasználása: A feldolgozás gyorsításához és az Octree-hez.
    /// </summary>
    public struct BoundingBox
    {
        #region Fields

        public Vector3 Min;
        public Vector3 Max;
        public static readonly BoundingBox Nil = new BoundingBox(Vector3.One * float.MaxValue, Vector3.One * float.MinValue);
        public static readonly BoundingBox Universe = new BoundingBox(Vector3.One * float.MinValue, Vector3.One * float.MaxValue);

        #endregion

        #region Properties

        public Vector3 Size { get { return Max - Min; } }

        public Vector3 Center
        {
            get
            {
                return (Min + Max) / 2;
            }
            set
            {
                var side = (Max - Min) / 2;
                Max = value + side;
                Min = value - side;
            }
        }

        #endregion

        #region Constructors

        public BoundingBox(ref Vector3 min, ref Vector3 max)
        {
            Min = min;
            Max = max;
        }

        internal BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        #endregion

        #region Methods

        public void Merge(ref BoundingBox other)
        {
            Vector3.ComponentMin(ref this.Min, ref other.Min, out this.Min);
            Vector3.ComponentMax(ref this.Max, ref other.Max, out this.Max);
        }

        public void GetGrid(ref Vector3 Min, ref Vector3 ext, int p, out int i_0, out int j_0, out int k_0, out int i_n, out int j_n, out int k_n)
        {
            i_0 = (int)(this.Min.X / ext.X) - (int)(Min.X / ext.X);
            j_0 = (int)(this.Min.Y / ext.Y) - (int)(Min.Y / ext.Y);
            k_0 = (int)(this.Min.Z / ext.Z) - (int)(Min.Z / ext.Z);
            i_n = (int)(this.Max.X / ext.X) - (int)(Min.X / ext.X);
            j_n = (int)(this.Max.Y / ext.Y) - (int)(Min.Y / ext.Y);
            k_n = (int)(this.Max.Z / ext.Z) - (int)(Min.Z / ext.Z);
            if (i_n > p) i_n = p;
            if (j_n > p) j_n = p;
            if (k_n > p) k_n = p;
            if (i_0 < 0) i_0 = 0;
            if (j_0 < 0) j_0 = 0;
            if (k_0 < 0) k_0 = 0;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[BoundingBox: Size={0}, Center={1}]", Size, Center);
        }
    }
}
