using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dragon_s_Breath.Assets
{
    /// <summary>
    /// Denna klass håller reda på alla modeller 
    /// och delar upp dessa i plan och objekt.
    /// </summary>
    static class ModelManager
    {
        private static Model[] aircrafts;
        private static Model[] objects;

        public static Model[] Aircrafts { get { return aircrafts; } }
        public static Model[] Objects { get { return objects; } }

        /// <summary>
        /// Denna metod gör anrop till de metoder 
        /// som laddar in modellerna i spelet.
        /// </summary>
        /// <param name="content">Content managern från Game1</param>
        public static void LoadContent(ContentManager content)
        {
            LoadAircrafts(content);
            LoadObjects(content);
        }

        /// <summary>
        /// Denna metod laddar in alla plan i spelet.
        /// </summary>
        /// <param name="content">Content managern som skickas in till LoadContent i denna klass från Game1.</param>
        public static void LoadAircrafts(ContentManager content)
        {
            List<Model> models = new List<Model>();

            //Lägg till alla plan i listan
            models.Add(content.Load<Model>(@"Models\OldStylePlane"));

            aircrafts = GenerateArray(models);
        }

        /// <summary>
        /// Denna metod laddar in alla objekt som inte är plan i spelet.
        /// </summary>
        /// <param name="content">Content managern som skickas in till LoadContent i denna klass från Game1.</param>
        public static void LoadObjects(ContentManager content)
        {
            List<Model> models = new List<Model>();

            //Lägg till alla modeller som inte är plan i listan

            objects = GenerateArray(models);
        }

        /// <summary>
        /// Denna metod gör om en lista med modeller till en array med samma modeller.
        /// </summary>
        /// <param name="models">Listan med modeller som ska göras om till en array.</param>
        /// <returns></returns>
        public static Model[] GenerateArray(List<Model> models)
        {
            Model[] modelArray = new Model[models.Count];

            for (int i = 0; i < modelArray.Length; ++i)
                modelArray[i] = models[i];

            return modelArray;
        }
    }
}
