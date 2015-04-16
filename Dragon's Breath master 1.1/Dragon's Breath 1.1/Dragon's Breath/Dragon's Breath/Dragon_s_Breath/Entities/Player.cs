using Dragon_s_Breath.Assets;
using Dragon_s_Breath.Entities;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dragon_s_Breath
{
    /// <summary>
    /// Denna klass innehåller logik avsedd för spelaren 
    /// såsom kamera och hur spelarens plan reagerar.
    /// </summary>
    class Player : Aircraft
    {
        private readonly Camera camera_;

        static KeyboardState ActualKeyState;

        public Camera Camera { get { return camera_; } }

        public Player(String name, int model, Rectangle clientBounds, Vector3 position) :
            base(name, model, position)
        {
            camera_ = new Camera(clientBounds);
        }

        /// <summary>
        /// Denna metod används för att göra så att spelarens 
        /// plan reagerar när spelaren trycker på rätt knappar, 
        /// att information skickas till servern angående 
        /// spelarens position, namn, etc.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            ActualKeyState = Keyboard.GetState();

<<<<<<< HEAD
            if (ActualKeyState.IsKeyDown(Keys.W))
                Pitch(MathHelper.ToRadians(-1));
            if (ActualKeyState.IsKeyDown(Keys.S))
                Pitch(MathHelper.ToRadians(1));
            if (ActualKeyState.IsKeyDown(Keys.A))
                Yaw(MathHelper.ToRadians(1));
            if (ActualKeyState.IsKeyDown(Keys.D))
                Yaw(MathHelper.ToRadians(-1));
            if (ActualKeyState.IsKeyDown(Keys.Q))
                Roll(MathHelper.ToRadians(-1));
            if (ActualKeyState.IsKeyDown(Keys.E))
                Roll(MathHelper.ToRadians(1));
=======
            if (ActualKeyState.IsKeyDown(Keys.Up))
                Pitch(MathHelper.ToRadians(1));
            if (ActualKeyState.IsKeyDown(Keys.Down))
                Pitch(MathHelper.ToRadians(-1));
            if (ActualKeyState.IsKeyDown(Keys.Left))
                Yaw(MathHelper.ToRadians(-1));
            if (ActualKeyState.IsKeyDown(Keys.Right))
                Yaw(MathHelper.ToRadians(1));
>>>>>>> origin/master
            if (ActualKeyState.IsKeyDown(Keys.Z))
                if (++velocity > maxSpeed)
                    velocity = maxSpeed;
            if (ActualKeyState.IsKeyDown(Keys.X))
                if (--velocity < minSpeed)
                    velocity = minSpeed;

            base.Update(gameTime);
<<<<<<< HEAD
            SendDataToServer();
            camera_.Update(gameTime, worldMatrix);
        }

        /// <summary>
        /// Denna metod roterar planet längs sin egen axel åt antingen höger eller vänster.
        /// </summary>
        /// <param name="radians">Hur många radianer planet ska roteras.</param>
        private void Yaw(float radians)
        {
            worldMatrix *= Matrix.CreateTranslation(-worldMatrix.Translation) * Matrix.CreateFromAxisAngle(worldMatrix.Up, radians) * Matrix.CreateTranslation(worldMatrix.Translation);
        }

        /// <summary>
        /// Denna metod roterar planet längs sin egel axel antingen uppåt eller nedåt.
        /// </summary>
        /// <param name="radians">Hur många radianer planet ska roteras.</param>
        private void Pitch(float radians)
        {
            worldMatrix *= Matrix.CreateTranslation(-worldMatrix.Translation) * Matrix.CreateFromAxisAngle(worldMatrix.Right, radians) * Matrix.CreateTranslation(worldMatrix.Translation);
        }

        /// <summary>
        /// Denna metod roterar planet längs sin egel axel antingen så att planet rullar.
        /// </summary>
        /// <param name="radians">Hur många radianer planet ska roteras.</param>
        private void Roll(float radians)
        {
            worldMatrix *= Matrix.CreateTranslation(-worldMatrix.Translation) * Matrix.CreateFromAxisAngle(worldMatrix.Forward, radians) * Matrix.CreateTranslation(worldMatrix.Translation);
        }

        private void SendDataToServer()
        {
=======
>>>>>>> origin/master
            Network.outmsg = Network.Client.CreateMessage();
            Network.outmsg.Write("move");
            Network.outmsg.Write(Constants.name);

<<<<<<< HEAD
            Network.outmsg.Write((int)worldMatrix.Translation.X);
            Network.outmsg.Write((int)worldMatrix.Translation.Y);
            Network.outmsg.Write((int)worldMatrix.Translation.Z);

            Network.outmsg.Write(worldMatrix.Forward.X);
            Network.outmsg.Write(worldMatrix.Forward.Y);
            Network.outmsg.Write(worldMatrix.Forward.Z);

            Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.Unreliable);
=======
            Quaternion rotation;
            Vector3 scale, position;
            worldMatrix.Decompose(out scale, out rotation, out position);


            Network.outmsg.Write((int)position.X);
            Network.outmsg.Write((int)position.Y);
            Network.outmsg.Write((int)position.Z);

            Network.outmsg.Write(rotation.X);
            Network.outmsg.Write(rotation.Y);
            Network.outmsg.Write(rotation.Z);
            Network.outmsg.Write(rotation.W);

            Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.Unreliable);
            camera_.Update(gameTime, worldMatrix);
>>>>>>> origin/master
        }

        /// <summary>
        /// Denna metod kallas när planet ska ritas ut och 
        /// ritar också ut andra spelares plan.
        /// </summary>
        public void Draw()
        {
            base.Draw(camera_);
            EnemyManager.Draw(camera_);
        }
    }
}
