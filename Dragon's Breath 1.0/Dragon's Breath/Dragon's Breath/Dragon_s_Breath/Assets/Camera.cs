using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dragon_s_Breath
{
    class Camera
    {
        #region Fields
        MouseState prevMouseState;
        Rectangle clientBounds;
        Vector3 cameraDirection, cameraUp;
        float speed = 10;
        #endregion

        #region Properties
        public Vector3 CameraPosition { get; protected set; }
        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }
        #endregion

        #region Constructor
        public Camera(Rectangle clientBounds, Vector3 pos, Vector3 target, Vector3 up)
        {
            this.clientBounds = clientBounds;
            // Build camera view matrix
            CameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            CreateLookAt();

            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)clientBounds.Width / (float)clientBounds.Height,
                1,
                20000);
        }
        #endregion

        private void CreateLookAt()
        {
            View = Matrix.CreateLookAt(CameraPosition,
                CameraPosition + cameraDirection,
                cameraUp);
        }
        public void Initialize()
        {
            // Set mouse position and do initial get state
            Mouse.SetPosition(clientBounds.Width / 2, clientBounds.Height / 2);
            prevMouseState = Mouse.GetState();
        }
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                CameraPosition += new Vector3(0, 10, 0);
            }

            // Move forward & backward
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                CameraPosition += cameraDirection * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                CameraPosition -= cameraDirection * speed;
            }
            // Move side-to-side
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                CameraPosition += Vector3.Cross(cameraUp, cameraDirection) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                CameraPosition -= Vector3.Cross(cameraUp, cameraDirection) * speed;
            }

            // Yaw rotation
            //cameraDirection = Vector3.Transform(cameraDirection,
              //  Matrix.CreateFromAxisAngle(cameraUp,
              //  -(MathHelper.PiOver4 / 100) * (Mouse.GetState().X - prevMouseState.X)));

            // Roll rotation
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                cameraUp = Vector3.Transform(cameraUp,
                Matrix.CreateFromAxisAngle(cameraDirection,
                MathHelper.PiOver4 / 45));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                cameraUp = Vector3.Transform(cameraUp,
                Matrix.CreateFromAxisAngle(cameraDirection,
                -MathHelper.PiOver4 / 45));
            }

            // Pitch rotation
            //cameraDirection = Vector3.Transform(cameraDirection,
             //  Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection),
              //  (MathHelper.PiOver4 / 100) * (Mouse.GetState().Y - prevMouseState.Y)));

            //cameraUp = Vector3.Transform(cameraUp,
            //    Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection),
            //    (MathHelper.PiOver4 / 100) * (Mouse.GetState().Y - prevMouseState.Y)));

            // Reset prevMouseState
            prevMouseState = Mouse.GetState();
            // Apply changes to view matrix
            CreateLookAt();
        }
    }
}
