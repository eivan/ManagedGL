using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedGL.Vertices
{
    public class VertexElementAttribute : Attribute
    {
        public VertexElementAttribute(
            int usageIndex,
            string shaderAttributeName = null,
            bool normalized = false)
        {
            this.UsageIndex = usageIndex;
            this.Normalized = normalized;
            this.ShaderAttributeName = shaderAttributeName;
        }

        public int UsageIndex { get; set; }

        public bool Normalized { get; set; }

        public string ShaderAttributeName { get; set; }
    }
}
