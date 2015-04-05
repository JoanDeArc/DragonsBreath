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

            if (ActualKeyState.IsKeyDown(Keys.Up))
                Pitch(MathHelper.ToRadians(1));
            if (ActualKeyState.IsKeyDown(Keys.Down))
                Pitch(MathHelper.ToRadians(-1));
            if (ActualKeyState.IsKeyDown(Keys.Left))
                Yaw(MathHelper.ToRadians(-1));
            if (ActualKeyState.IsKeyDown(Keys.Right))
                Yaw(MathHelper.ToRadians(1));
            if (ActualKeyState.IsKeyDown(Keys.Z))
                if (++velocity > maxSpeed)
                    velocity = maxSpeed;
            if (ActualKeyState.IsKeyDown(Keys.X))
                if (--velocity < minSpeed)
                    velocity = minSpeed;

            base.Update(gameTime);
            Network.outmsg = Network.Client.CreateMessage();
            Network.outmsg.Write("move");
            Network.outmsg.Write(Constants.name);
            Network.outmsg.Write((int)this.modelOrientation.Translation.X);
            Network.outmsg.Write((int)modelOrientation.Translation.Y);
            Network.outmsg.Write((int)modelOrientation.Translation.Z);
            Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.Unreliable);
            camera_.Update(gameTime, modelOrientation);
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
