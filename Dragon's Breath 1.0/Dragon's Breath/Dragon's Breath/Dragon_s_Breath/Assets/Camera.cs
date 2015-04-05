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
        public Camera(Rectangle clientBounds)
        {
            this.clientBounds = clientBounds;

            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)clientBounds.Width / (float)clientBounds.Height,
                1,
                20000);

            // Set mouse position and do initial get state
            Mouse.SetPosition(clientBounds.Width / 2, clientBounds.Height / 2);
            prevMouseState = Mouse.GetState();
        }
        #endregion

        private void CreateLookAt()
        {
            View = Matrix.CreateLookAt(CameraPosition,
                CameraPosition + cameraDirection,
                cameraUp);
        }

        private void SetCameraPosition(Matrix playerOrientation)
        {
            cameraUp = playerOrientation.Up;
            CameraPosition = playerOrientation.Translation + playerOrientation.Backward * 10 + cameraUp * 2;
            cameraDirection = playerOrientation.Forward;
            CreateLookAt();
        }

        public void Update(GameTime gameTime, Matrix playerOrientation)
        {
            SetCameraPosition(playerOrientation);

            // Reset prevMouseState
            prevMouseState = Mouse.GetState();
        }
    }
}
