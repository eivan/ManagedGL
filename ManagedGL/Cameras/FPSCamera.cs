using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedGL.Cameras
{
    /// <summary>
    /// Egérrel irányítható kamera
    /// </summary>
    public class FPSCamera : MobileCamera
    {
        /// <summary>
        /// Egérmozgás
        /// </summary>
        public void MouseMovement(int dx, int dy, float dt)
        {
            RollPitch(dx / 10f * dt, -dy / 10f * dt);
        }

        Point screenCenter;
        Point windowCenter;
        public void Initialize(GameWindow window)
        {
            screenCenter = new Point(window.Bounds.Left + (window.Bounds.Width / 2), window.Bounds.Top + (window.Bounds.Height / 2));
            windowCenter = new Point(window.Width / 2, window.Height / 2);

            window.Mouse.Move += Mouse_Move;
            window.Mouse.ButtonDown += (_, e) => { if (e.Button == OpenTK.Input.MouseButton.Right) bRightMouseButton = true; };
            window.Mouse.ButtonUp += (_, e) => { if (e.Button == OpenTK.Input.MouseButton.Right) bRightMouseButton = false; };
            window.KeyDown += window_KeyDown;
            window.KeyUp += window_KeyUp;
        }

        private bool[] keys = new bool[4];
        private bool bRightMouseButton;
        
        void window_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case OpenTK.Input.Key.W:
                    keys[0] = false;
                    break;

                case OpenTK.Input.Key.S:
                    keys[1] = false;
                    break;

                case OpenTK.Input.Key.A:
                    keys[2] = false;
                    break;

                case OpenTK.Input.Key.D:
                    keys[3] = false;
                    break;
            }
        }
        void window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case OpenTK.Input.Key.W:
                    keys[0] = true;
                    break;

                case OpenTK.Input.Key.S:
                    keys[1] = true;
                    break;

                case OpenTK.Input.Key.A:
                    keys[2] = true;
                    break;

                case OpenTK.Input.Key.D:
                    keys[3] = true;
                    break;
            }
        }

        public void Update(double t)
        {
            float time = (float)t;
            if (keys[0]) MoveForward(time);
            if (keys[1]) MoveBackward(time);
            if (keys[2]) MoveLeft(time);
            if (keys[3]) MoveRight(time);
        }

        void Mouse_Move(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            if(bRightMouseButton)
                MouseMovement(e.XDelta, e.YDelta, 0.025f);
        }
    }
}
