using System;
using System.Windows.Forms;

namespace Dragon_s_Breath
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Application.Run(new Launcher());

            if (Constants.playing)
            {
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            }
        }
    }
#endif
}

