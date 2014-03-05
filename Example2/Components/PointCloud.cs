using Example2.Components.PointCloudShader;
using ManagedGL;
using ManagedGL.Cameras;
using ManagedGL.Shaders;
using ManagedGL.Vertices;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example2.Components
{
    public class PointCloud : Component
    {
        protected int length = 0;
        protected IVertexArray vao;
        protected NormalsProgram normals_shader;
        protected PointsProgram points_shader;
        protected EllipsoidsProgram ellipsoids_shader;

        public PointCloud()
            : base("Point Cloud")
        {
        }

        public void SetVertices(params VertexPositionNormalColor[] vertices)
        {
            length = vertices.Length;
            (vao as VertexArray<VertexPositionNormalColor>).BufferData(vertices);
        }

        public override void OnLoad()
        {
            vao = Factory<VertexPositionNormalColor>.CreateVertexArray();
            normals_shader = new NormalsProgram();
            points_shader = new PointsProgram();
            ellipsoids_shader = new EllipsoidsProgram();
        }

        public override void OnUnload()
        {
            vao.Dispose();
            normals_shader.Dispose();
        }

        public override void Update(Camera camera, double t)
        {
        }

        public override void Render(Camera camera, double t)
        {
            vao.Begin();
            
            normals_shader.Begin();
            normals_shader.SetUniforms(camera);
            GL.DrawArrays(PrimitiveType.Points, 0, length);
            normals_shader.End();

            points_shader.Begin();
            points_shader.SetUniforms(camera);
            GL.DrawArrays(PrimitiveType.Points, 0, length);
            points_shader.End();

            vao.End();
        }
    }
}
