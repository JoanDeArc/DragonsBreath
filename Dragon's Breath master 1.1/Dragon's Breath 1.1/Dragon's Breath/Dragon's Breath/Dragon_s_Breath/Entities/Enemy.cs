using Dragon_s_Breath.Assets;
using Dragon_s_Breath.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dragon_s_Breath
{
    /// <summary>
    /// Denna klass innehåller logik avsedd för att tolka 
    /// och rita ut andra spelares handlingar.
    /// </summary>
    class Enemy : Aircraft
    {
        static double threshold = 3;
        static float interpolationConstant = 0.01f;
        public List<Player> players = new List<Player>();

        //Temp
<<<<<<< HEAD
        public Vector3 newPosition;
        public Vector3 newForward;

        //Temp
        public void SetWorldMatrix(Matrix worldMatrix, Vector3 forward)
        {
            this.worldMatrix = worldMatrix;
            this.worldMatrix.Forward = forward;
=======
        public Vector3 remotePosition;

        //Temp
        public void SetModelOrientation(Matrix orientation)
        {
            worldMatrix = orientation;
>>>>>>> origin/master
        }

        public Enemy(String name, int model, Vector3 position) :
            base(name, model, position)
        {

        }

        /// <summary>
        /// Denna metod används för att få motspelarnas plan 
        /// att reagera som de gör för motspelaren i fråga.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            float delta = gameTime.ElapsedGameTime.Milliseconds;
<<<<<<< HEAD
            Vector3 v3r = newPosition;
            Vector3 v3t = worldMatrix.Translation;


            double difference = Math.Abs(Math.Sqrt(v3r.X * v3r.X + v3r.Y * v3r.Y + v3r.Z * v3r.Z) - Math.Sqrt(v3t.X * v3t.X + v3t.Y * v3t.Y + v3t.Z * v3t.Z));
            
            if (difference < threshold)
            {
                worldMatrix.Translation = v3r;
            }
            else
            {
                //worldMatrix.Translation = v3r;
                float differenceX = v3r.X - v3t.X;
                float differenceY = v3r.Y - v3t.Y;
                float differenceZ = v3r.Z - v3t.Z;
                
                worldMatrix.Translation += new Vector3(differenceX * delta * interpolationConstant, differenceY * delta * interpolationConstant, differenceZ * delta * interpolationConstant);
            }

            if (Math.Acos(Vector3.Dot(WorldMatrix.Forward, newForward)) < MathHelper.ToRadians(5))
            {
                float differenceX = newForward.X - worldMatrix.Forward.X;
                float differenceY = newForward.Y - worldMatrix.Forward.Y;
                float differenceZ = newForward.Z - worldMatrix.Forward.Z;

                worldMatrix.Forward += new Vector3(differenceX * delta * interpolationConstant, differenceY * delta * interpolationConstant, differenceZ * delta * interpolationConstant);
            }
            else
            {
                worldMatrix.Forward = newForward;
            }
=======
                Vector3 v3r = remotePosition;
                Vector3 v3t = worldMatrix.Translation;

                double difference = Math.Abs(Math.Sqrt(v3r.X * v3r.X + v3r.Y * v3r.Y + v3r.Z * v3r.Z) - Math.Sqrt(v3t.X * v3t.X + v3t.Y * v3t.Y + v3t.Z * v3t.Z));

                if (difference < threshold)
                {
                    worldMatrix.Translation = v3r;
                }
                else
                {
                    //worldMatrix.Translation = v3r;
                    float differenceX = v3r.X - v3t.X;
                    float differenceY = v3r.Y - v3t.Y;
                    float differenceZ = v3r.Z - v3t.Z;

                    worldMatrix.Translation += new Vector3(differenceX * delta * interpolationConstant, differenceY * delta * interpolationConstant, differenceZ * delta * interpolationConstant);
                }
>>>>>>> origin/master
        }
    }
}