using Example2.Components;
using Example2.Components.PointCloudShader;
using ManagedGL;
using ManagedGL.Cameras;
using ManagedGL.Helpers;
using ManagedGL.Vertices;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example2
{
    public class ExampleScene : Scene
    {
        private PointCloud pointCloud;
        private FPSCamera camera;

        public ExampleScene(GameWindow window)
            : base(typeof(PointCloud))
        {
            this.Cameras.Add(camera = new FPSCamera() { Velocity = 3 });
            this.Components.Add(pointCloud = new PointCloud());

            camera.Initialize(window);
        }

        public override void OnLoad()
        {
            base.OnLoad();

            NextCamera();
            
            camera.Position = new Vector3(4, 4, 4);
            camera.Target = Vector3.Zero;

            var vertices = new[]{
                new VertexPositionNormalColor(new Vector3(-1, -1, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 0)),
                new VertexPositionNormalColor(new Vector3( 1, -1, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0)),
                new VertexPositionNormalColor(new Vector3(-1,  1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1)),
                new VertexPositionNormalColor(new Vector3( 1,  1, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 0))
            };
            //pointCloud.SetVertices(vertices);

            VertexPositionNormal[] temp_verts;
            Geometries.Ellipsoid(10, 10, 2, 1, 1, out temp_verts);   
            pointCloud.SetVertices(temp_verts.Select(v => new VertexPositionNormalColor(v, Vector3.UnitX)).ToArray());
        }

        public override void Update(double t)
        {
            base.Update(t);

            (ActiveCamera as FPSCamera).Update(t);
        }

        public override void Render(double t)
        {
            base.Render(t);
        }
    }
}
