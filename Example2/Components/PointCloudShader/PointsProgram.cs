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
    public class PointsProgram : ShaderProgram
    {
        private Uniform modelViewProjectionMatrix;
        private Uniform modelViewProjectionMatrixInverseTransposed;
        private Uniform normal_length;

        public PointsProgram()
            : base()
        {
            const string path = "Components/PointCloudShader/";
            Attach(new Shader(ShaderType.GeometryShader, File.ReadAllText(path + "pc_points.geom")));
            Attach(new Shader(ShaderType.VertexShader, File.ReadAllText(path + "pc_normals.vert")));
            Attach(new Shader(ShaderType.FragmentShader, File.ReadAllText(path + "pc_normals.frag")));

            Factory<VertexPositionNormalColor>.BindVertexAttributeLocations(this);
            
            Link();

            modelViewProjectionMatrix = GetUniform("mvp");
            modelViewProjectionMatrixInverseTransposed = GetUniform("mvpit");
        }

        public void SetUniforms(Camera camera)
        {
            var modelViewProj = Matrix4.Identity *camera.ViewProjection;
            var modelViewProjInverseTranspose = modelViewProj;

            modelViewProjInverseTranspose.Invert();
            modelViewProjInverseTranspose.Transpose();

            //Begin();
            {
                modelViewProjectionMatrixInverseTransposed.SetValue(ref modelViewProjInverseTranspose);
                modelViewProjectionMatrix.SetValue(ref modelViewProj);
            }
            //End();
        }
    }
}
