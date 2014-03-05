using ManagedGL;
using ManagedGL.Shaders;
using ManagedGL.Vertices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;

namespace Example1
{
    public class ExampleWindow : GameWindow
    {
        protected IVertexArray vao;
        protected ShaderProgram shader;
        
        
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

            Initialize();

            Keyboard.KeyDown += Keyboard_KeyDown;
        }

        private void Initialize()
        {
            var vertices = new[]{
                new VertexPositionColor(new Vector3(-1, -1, 0), new Vector3(1, 0, 0)),
                new VertexPositionColor(new Vector3( 1, -1, 0), new Vector3(0, 1, 0)),
                new VertexPositionColor(new Vector3(-1,  1, 0), new Vector3(0, 0, 1)),
                new VertexPositionColor(new Vector3( 1,  1, 0), new Vector3(1, 1, 1))
            };

            vao = vertices.ToVertexArray();
            //vao = Factory<VertexPositionColor>.CreateVertexArray(vertices);

            shader = Factory<VertexPositionColor>.CreateShaderProgram(
                new VertexShaders.PositionColor(),
                new FragmentShaders.Color());
            
            shader.Begin();
            vao.Begin();
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
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit |
                     ClearBufferMask.DepthBufferBit);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            //GL.Flush();            
            SwapBuffers();
        }
    }
}
