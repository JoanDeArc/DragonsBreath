using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dragon_s_Breath.Assets
{
    /// <summary>
    /// Denna klass håller reda på, updaterar 
    /// och ritar ut alla motspelare.
    /// </summary>
    static class EnemyManager
    {
        private static List<Enemy> enemies;

        public static List<Enemy> Enemies { get { return enemies; } }

        /// <summary>
        /// Denna klass initierar litan med 
        /// motspelarnas plan.
        /// </summary>
        public static void Initialize()
        {
            enemies = new List<Enemy>();
        }

        /// <summary>
        /// Denna metod updaterar motspelarnas 
        /// plan så att de beter sig på samma 
        /// sätt på alla spelares datorer.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            foreach (Enemy e in enemies)
            {
                e.Update(gameTime);
            }
        }

        /// <summary>
        /// Denna metod ritar ut motspelarnas plan.
        /// </summary>
        /// <param name="camera"></param>
        public static void Draw(Camera camera)
        {
            foreach (Enemy e in enemies)
                e.Draw(camera);
        }
    }
}
