using ManagedGL;
using ManagedGL.Cameras;
using ManagedGL.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example2.Components.PointCloudShader
{
    public class NormalsProgram : ShaderProgram
    {
        private Uniform modelViewProjectionMatrix;
        private Uniform normal_length;

        public NormalsProgram()
            : base()
        {
            const string path = "Components/PointCloudShader/";
            Attach(new Shader(ShaderType.GeometryShader, File.ReadAllText(path + "pc_normals.geom")));
            Attach(new Shader(ShaderType.VertexShader, File.ReadAllText(path + "pc_normals.vert")));
            Attach(new Shader(ShaderType.FragmentShader, File.ReadAllText(path + "pc_normals.frag")));

            Factory<VertexPositionNormalColor>.BindVertexAttributeLocations(this);
            
            Link();

            modelViewProjectionMatrix = GetUniform("mvp");
            normal_length = GetUniform("normal_length");
            //inverseTranspose = GetUniform("inverseTranspose");
        }

        public void SetUniforms(Camera camera)
        {
            var modelViewProj = Matrix4.Identity *camera.ViewProjection;

            var modelViewProjInverseTranspose = modelViewProj;
            modelViewProjInverseTranspose.Invert();
            modelViewProjInverseTranspose.Transpose();

            //Begin();
            {
                //inverseTranspose.SetValue(ref modelViewProjInverseTranspose);
                normal_length.SetValue(0.1f);
                modelViewProjectionMatrix.SetValue(ref modelViewProj);
            }
            //End();
        }
    }
}
