using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dragon_s_Breath
{
    class Enemy
    {
        static double threshold = 1;
        static float interpolationConstant = 0.01f;
        public static List<Player> players = new List<Player>();

        public static void Update(float delta)
        {
            foreach (Player p in players)
            {
                Vector3 v3r = p.remotePosition;
                Vector3 v3t = p.worldMatrix.Translation;

                double difference = Math.Abs(Math.Sqrt(v3r.X * v3r.X + v3r.Y * v3r.Y + v3r.Z * v3r.Z) - Math.Sqrt(v3t.X * v3t.X + v3t.Y * v3t.Y + v3t.Z * v3t.Z));

                if (difference < threshold)
                {
                    p.worldMatrix.Translation = v3r;
                }
                else
                {
                    //p.worldMatrix.Translation = v3r;
                    float differenceX = v3r.X - v3t.X;
                    float differenceY = v3r.Y - v3t.Y;
                    float differenceZ = v3r.Z - v3t.Z;

                    p.worldMatrix.Translation += new Vector3(differenceX * delta * interpolationConstant, differenceY * delta * interpolationConstant, differenceZ * delta * interpolationConstant);
                }

            }
        }

        public static void Draw(Camera camera)
        {
            foreach (Player p in players)
            {
                foreach (ModelMesh mesh in p.model.Meshes)
                {
                    foreach (BasicEffect shader in mesh.Effects)
                    {
                        shader.World = p.GetWorldMatrix();
                        shader.View = camera.View;
                        shader.Projection = camera.Projection;
                        shader.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
                    }
                    mesh.Draw();
                }
            }
        }
    }
}