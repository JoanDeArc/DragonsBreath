using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dragon_s_Breath
{
    class Player
    {
        public string name;
        public Matrix worldMatrix;
        public Vector3 remotePosition;
        public Model model;

        static KeyboardState ActualKeyState;

        public Player(String name, Model model)
        {
            this.name = name;
            this.model = model;
            worldMatrix = Matrix.Identity * Matrix.CreateTranslation(1000f, 1100f, 1000f);
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

        public void Draw(Camera camera)
        {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect shader in mesh.Effects)
                    {
                        shader.EnableDefaultLighting(); // Behövs inte
                        shader.World = worldMatrix;
                        shader.View = camera.View;
                        shader.Projection = camera.Projection;
                        shader.DirectionalLight0.DiffuseColor = Color.Blue.ToVector3();
                    }
                    mesh.Draw();
                }
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
