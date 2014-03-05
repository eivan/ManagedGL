using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedGL
{
    public static class MatrixExtensions
    {
        public static Matrix3 GetTop3x3(this Matrix4 matrix)
        {
            return new Matrix3(matrix);
        }
    }
}
