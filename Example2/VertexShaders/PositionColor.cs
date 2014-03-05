using ManagedGL.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example2.VertexShaders
{
    public class PositionColor : Shader
    {
        private static string code =
            @"#version 130

            // VBO-ból érkező változók
            in vec3 vs_in_pos;
            in vec3 vs_in_col;

            // a pipeline-ban tovább adandó értékek
            out vec3 vs_out_pos;
            out vec3 vs_out_col;

            void main()
            {
	            gl_Position = vec4( vs_in_pos, 1 );
	            vs_out_pos = vs_in_pos;
	            vs_out_col = vs_in_col;
            }";

        public PositionColor()
            : base(OpenTK.Graphics.OpenGL4.ShaderType.VertexShader, code)
        {
        }
    }
}
