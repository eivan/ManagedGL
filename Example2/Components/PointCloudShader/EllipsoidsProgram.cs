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
    public class EllipsoidsProgram : ShaderProgram
    {
        private Uniform modelViewProjectionMatrix;
        private Uniform normal_length;

        public EllipsoidsProgram()
            : base()
        {
            const string path = "Components/PointCloudShader/";
            Attach(new Shader(ShaderType.GeometryShader, File.ReadAllText(path + "pc_ellipsoids.geom")));
            Attach(new Shader(ShaderType.VertexShader, File.ReadAllText(path + "pc_ellipsoids.vert")));
            Attach(new Shader(ShaderType.FragmentShader, File.ReadAllText(path + "pc_normals.frag")));

            Factory<VertexPositionNormalColor>.BindVertexAttributeLocations(this);
            
            Link();

            modelViewProjectionMatrix = GetUniform("mvp");
            normal_length = GetUniform("normal_length");
        }

        public void SetUniforms(Camera camera)
        {
            var modelViewProj = Matrix4.Identity *camera.ViewProjection;

            var modelViewProjInverseTranspose = modelViewProj;
            modelViewProjInverseTranspose.Invert();
            modelViewProjInverseTranspose.Transpose();

            //Begin();
            {
                normal_length.SetValue(0.5f);
                modelViewProjectionMatrix.SetValue(ref modelViewProj);
            }
            //End();
        }
    }
}
