using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dragon_s_Breath.Assets
{
    /// <summary>
    /// Denna klass är avsedd som en grundklass för alla objekt och innehåller endast 
    /// saker som är gemensamma för alla objekt såsom position, rotation och modell.
    /// </summary>
    abstract class GameObject
    {
        protected Matrix worldMatrix;
        protected readonly int model_;

        public Matrix WorldMatrix { get { return worldMatrix; } }

        public GameObject(int model, Vector3 position)
        {
            model_ = model;
            worldMatrix = Matrix.Identity * Matrix.CreateTranslation(position);
        }

        public abstract void Draw(Camera camera);
    }
}
