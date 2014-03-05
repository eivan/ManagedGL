using ManagedGL;
using ManagedGL.Shaders;
using ManagedGL.Vertices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;

namespace Example2
{
    public class ExampleWindow : GameWindow
    {
        protected IVertexArray vao;
        protected ShaderProgram shader;
        private ExampleScene scene;
        
        
        public ExampleWindow()
            : base(640, 480)
        {
            Trace.Listeners.Add(new ConsoleTraceListener(true));

            Trace.WriteLine("Megjelenítő: \t" + GL.GetString(StringName.Renderer));
            Trace.WriteLine("OpenGL verzió: \t" + GL.GetString(StringName.Version));
            Trace.WriteLine("Shader verzió: \t" + GL.GetString(StringName.ShadingLanguageVersion));

            int[] temp = new int[1];
            GL.GetInteger(GetPName.MaxTextureImageUnits, out temp[0]);
            Trace.WriteLine(temp[0] + " textúrázó egységet találtam.");

            GL.GetInteger(GetPName.MaxVaryingFloats, out temp[0]);
            Trace.WriteLine(temp[0] + " VS és FS közötti lebegőpontos \"varying\" változó engedélyezett.");

            GL.GetInteger(GetPName.MaxVertexUniformComponents, out temp[0]);
            Trace.WriteLine(temp[0] + " \"uniform\" komponens engedélyezett VS-ben.");

            GL.GetInteger(GetPName.MaxFragmentUniformComponents, out temp[0]);
            Trace.WriteLine(temp[0] + " \"uniform\" komponens engedélyezett FS-ben.");

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.125f, 0.25f, 0.5f, 1.0f);
        }

        public Vector3 velocity = Vector3.Zero;

        protected override void OnLoad(System.EventArgs e)
        {
            GL.PointSize(10);
            GL.LineWidth(2);
            base.OnLoad(e);

            scene = new ExampleScene(this);
            scene.OnLoad();

            Keyboard.KeyDown += Keyboard_KeyDown;
        }
        
        void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Escape)
                Close();
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, this.Width, this.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            scene.Update(e.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit |
                     ClearBufferMask.DepthBufferBit);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            scene.Render(e.Time);

            //GL.Flush();            
            SwapBuffers();
        }
    }
}
