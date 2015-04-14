using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBclient
{
    class Player
    {
        public string name;
        public Matrix worldMatrix;

        static KeyboardState ActualKeyState;

        public Player(String name)
        {
            this.name = name;
            worldMatrix = Matrix.Identity * Matrix.CreateTranslation(0f, 0f, -10f);
        }

        public void Update()
        {
            ActualKeyState = Keyboard.GetState();

            if (ActualKeyState.IsKeyDown(Keys.Up))
                worldMatrix *= Matrix.CreateTranslation(0, 0, -1);

            if (ActualKeyState.IsKeyDown(Keys.Down))
                worldMatrix *= Matrix.CreateTranslation(0, 0, 1);
            if (ActualKeyState.IsKeyDown(Keys.Left))
                worldMatrix *= Matrix.CreateTranslation(-1, 0, 0);
            if (ActualKeyState.IsKeyDown(Keys.Right))
                worldMatrix *= Matrix.CreateTranslation(1, 0, 0);

            Network.outmsg = Network.Client.CreateMessage();
            Network.outmsg.Write("move");
            Network.outmsg.Write(Constants.name);
            Network.outmsg.Write((int)this.worldMatrix.Translation.X);
            Network.outmsg.Write((int)worldMatrix.Translation.Y);
            Network.outmsg.Write((int)worldMatrix.Translation.Z);
            Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.Unreliable);      
        }

        public void SetWorldMatrix(Matrix worldMatrix2)
        {
            worldMatrix = worldMatrix2;
        }
        public Matrix GetWorldMatrix()
        {
            return worldMatrix;
        }

    }
}
