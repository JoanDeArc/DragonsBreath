using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _3D_basics
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        #region Modellen
        Model model;
        Matrix modelMatrix;
        #endregion

        #region Kameran
        Vector3 position; // Kamerans position.
        Vector3 lookAt;   // Punkten kameran tittar mot.

        Matrix view;    // Inneh�ller informationen kameran beh�ver f�r att fungera.
        Matrix projection;  // Fungerar som kamerans lens.
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            #region Projektionsmatris
            // Projektionsmatrisen �r ungef�r som en kameras l�ns och beh�ver 4 argument;
            // synvinkeln f�r kameran (vanligtvis 45 grader), sk�rmuppl�sningen av 
            // sk�rmen, avst�ndet till kameran ett objekt m�ste ha f�r att ritas ut
            // och avst�ndet som best�mmer hur l�ngt bort ett objekt kan vara f�r 
            // att synas (ritas ut). Man �ndrar n�stan bara denna om man �ndrar
            // uppl�sningen, s� oftast s�tts den bara i initialize och l�mnas sedan.

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),
                GraphicsDevice.Adapter.CurrentDisplayMode.AspectRatio, 0.1f, 10000f);
            #endregion 

            #region Look at punkt
            // Kameran beh�ver en punkt som den ska titta mot. Denna punkt defineras
            // som position + en "framm�tvektor" (0, 0, -1).

            lookAt = position + Vector3.Forward; 


            // f�r att kunna rotera kameran beh�vs ett lite anorlunda s�tt att 
            // definera kamerans punkt att titta mot. f�rst r�knar man ut
            // rotationsmatrisen, sedan multiplicerar man den med vad som �r
            // framm�t. d� f�r man en ny punkt, placerad en enhet fr�n mitten (0, 0, 0).
            // kamerans punkt att titta p� blir d� positionen + den roterade punkten.

            //Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(0)); // Bevis - l�gg till decimaler
            //Vector3 lookAtOffset = Vector3.Transform(Vector3.Forward, rotationMatrix);
            //Console.WriteLine(lookAtOffset);
            //lookAt = position + lookAtOffset;
            #endregion

            #region Viewmatris
            // Viewmatrisen �r sj�lva kameran. Denna beh�ver veta kamerans position, 
            // punkten den ska titta mot och vad som �r upp. Den beh�ver veta vad 
            // som �r upp eftersom �ven om man st�r p� en position och tittar mot
            // en punkt, �r det exakt likadana v�rden om man st�r upp och ner
            // p� samma st�lle och tittar mot samma punkt.
            view = Matrix.CreateLookAt(position, lookAt, Vector3.Up);
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            #region Modellen f�r v�rden
            model = Content.Load<Model>("thing2");
            modelMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(20)) * Matrix.CreateTranslation(0f, 0f, -10f);
            #endregion

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            #region Rotation av modell
            //Vector3 oldPos = modelMatrix.Translation;
            //modelMatrix *= Matrix.CreateTranslation(-modelMatrix.Translation);
            //modelMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(1));
            //modelMatrix *= Matrix.CreateTranslation(oldPos);
            
            //modelMatrix *= Matrix.CreateTranslation(-modelMatrix.Translation) * Matrix.CreateRotationY(MathHelper.ToRadians(1)) * Matrix.CreateTranslation(modelMatrix.Translation);
            #endregion

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Varje modell �r gjord utav meshes, och varje mesh har effekter som kallas shaders.
            // N�r man ritar ut en modell m�ste man loopa igenom varje mesh, och den meshens
            // shaders, och ber�tta hur meshen ska ritas ut - vilka "effekter" som ska p�verka 
            // den. Det finns m�nga olika saker man kan l�gga till, men man beh�ver minst
            // 3 (eller 4). Meshen beh�ver veta var den ska ritas ut, d.v.s. modellens
            // position, rotation och skala, vilket �r definerat i modellens matris. Den
            // beh�ver ocks� veta var vad kameran g�r, och hur v�r projektionsmatris ser 
            // ut. Dessa 3 saker m�ste man ber�tta f�r varje shader i varje mesh. 
            // Det finns m�nga saker man kan justera f�r �vrigt, men en sak man ofta vill ha
            // �r EnableDefaultLightning, vilket aktiverar modellens ljusinst�llningar. 
            // Utan ljuset blir modellens kanter inte synliga.

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect shader in mesh.Effects)
                {
                    //shader.EnableDefaultLighting(); // Beh�vs inte
                    shader.World = modelMatrix;
                    shader.View = view;
                    shader.Projection = projection;
                }
                mesh.Draw();
            }
            
            base.Draw(gameTime);
        }
    }
}
