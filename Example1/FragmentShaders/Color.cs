using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedGL;

namespace Example1.FragmentShaders
{
    public class Color : ManagedGL.Shaders.Shader
    {
        private static string code =
            @"#version 130

            in vec3 vs_out_col;
            in vec3 vs_out_pos;
            out vec4 fs_out_col;

            void main()
            {
	            fs_out_col = vec4(vs_out_col, 1);
            }"; 


        public Color()
            : base(OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader, code)
        {
        }
    }
}
