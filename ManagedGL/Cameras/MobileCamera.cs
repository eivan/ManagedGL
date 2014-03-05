using OpenTK;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ManagedGL.Cameras
{
    /// <summary>
    /// Mozgatható kamera.
    /// Kameramozgás RollPitch-al.
    /// </summary>
    [XmlInclude(typeof(FPSCamera))]
    public class MobileCamera : Camera
    {
        public MobileCamera()
            : base()
        {
            Velocity = 1.0f;
        }

        [Category("Mozgás (Movement)"), Description("A kamera mozgási sebessége")]
        public float Velocity { get; set; }

        public void MoveForward(float dt)
        {
            Position += Direction * Velocity * dt;
        }

        public void MoveBackward(float dt)
        {
            Position -= Direction * Velocity * dt;
        }

        public void MoveRight(float dt)
        {
            Position -= Side * Velocity * dt;
        }

        public void MoveLeft(float dt)
        {
            Position += Side * Velocity * dt;
        }

        public void MoveUp(float dt)
        {
            Position += Up * Velocity * dt;
        }

        public void MoveDown(float dt)
        {
            Position -= Up * Velocity * dt;
        }

        /// <summary>
        /// Forgatás
        /// </summary>
        /// <param name="roll">Lokális x-tengely körüli forgatás</param>
        /// <param name="pitch">Lokális y-tengely körüli forgatás</param>
        public void RollPitch(float roll, float pitch)
        {
            var qdir = new Quaternion(Direction, 0);
            var qdx = Quaternion.FromAxisAngle(Up, roll);
            var qdy = Quaternion.FromAxisAngle(Side, pitch);

            Direction = (qdir * qdx * qdy).Xyz;
        }

        /// <summary>
        /// Forgatás
        /// </summary>
        /// <param name="rp">Lokális x-tengely körüli forgatás, Lokális y-tengely körüli forgatás</param>
        public void RollPitch(Vector2 rp)
        {
            RollPitch(rp.X, rp.Y);
        }
    }
}
