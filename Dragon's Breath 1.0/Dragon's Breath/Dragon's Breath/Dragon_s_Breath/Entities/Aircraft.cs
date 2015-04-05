using Dragon_s_Breath.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dragon_s_Breath.Entities
{
    /// <summary>
    /// Denna klass är avsedd att hålla de variabler och funktioner som är gemensamma för 
    /// alla plantyper, oavsett om det är spelarens eller en fiendes plan.
    /// </summary>
    abstract class Aircraft : GameObject
    {
        private readonly string name_;
        protected static float minSpeed = 10f, maxSpeed = 50f;
        protected float velocity;

        public string Name { get { return name_; } }

        public Aircraft(String name, int model, Vector3 position) :
            base(model, position)
        {
            name_ = name;
            velocity = minSpeed;
        }

        /// <summary>
        /// Används för att planet ska röra sig, kolla kollisioner, etc.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            float time = gameTime.ElapsedGameTime.Milliseconds * (float)Math.Pow(10, -3);

            modelOrientation *= Matrix.CreateTranslation(modelOrientation.Forward * velocity * time);
        }

        /// <summary>
        /// Denna metod roterar planet längs sin egen axel åt antingen höger eller vänster.
        /// </summary>
        /// <param name="radians">Hur många radianer planet ska roteras.</param>
        protected void Yaw(float radians)
        {
            modelOrientation *= Matrix.CreateTranslation(-modelOrientation.Translation) * Matrix.CreateFromAxisAngle(modelOrientation.Down, radians) * Matrix.CreateTranslation(modelOrientation.Translation);
        }

        /// <summary>
        /// Denna metod roterar planet längs sin egel axel antingen uppåt eller nedåt.
        /// </summary>
        /// <param name="radians">Hur många radianer planet ska roteras.</param>
        protected void Pitch(float radians)
        {
            modelOrientation *= Matrix.CreateTranslation(-modelOrientation.Translation) * Matrix.CreateFromAxisAngle(modelOrientation.Right, radians) * Matrix.CreateTranslation(modelOrientation.Translation);
        }

        /// <summary>
        /// Denna metod kallas när planet ska ritas ut.
        /// </summary>
        /// <param name="camera">Spelarens kamera.</param>
        public override void Draw(Camera camera)
        {
            Matrix rotation = Matrix.CreateRotationY(MathHelper.Pi / 2);
            foreach (ModelMesh mesh in ModelManager.Aircrafts[model_].Meshes)
            {
                foreach (BasicEffect shader in mesh.Effects)
                {
                    shader.EnableDefaultLighting(); // Behövs inte
                    shader.World = modelOrientation;
                    shader.View = camera.View;
                    shader.Projection = camera.Projection;
                    shader.DirectionalLight0.DiffuseColor = Color.Blue.ToVector3();
                }
                mesh.Draw();
            }
            Terrain.Draw(camera);
        }
    }
}
