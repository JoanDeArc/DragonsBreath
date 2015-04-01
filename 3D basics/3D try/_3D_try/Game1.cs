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

        Matrix view;    // Innehåller informationen kameran behöver för att fungera.
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
            // Projektionsmatrisen är ungefär som en kameras läns och behöver 4 argument;
            // synvinkeln för kameran (vanligtvis 45 grader), skärmupplösningen av 
            // skärmen, avståndet till kameran ett objekt måste ha för att ritas ut
            // och avståndet som bestämmer hur långt bort ett objekt kan vara för 
            // att synas (ritas ut). Man ändrar nästan bara denna om man ändrar
            // upplösningen, så oftast sätts den bara i initialize och lämnas sedan.

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),
                GraphicsDevice.Adapter.CurrentDisplayMode.AspectRatio, 0.1f, 10000f);
            #endregion 

            #region Look at punkt
            // Kameran behöver en punkt som den ska titta mot. Denna punkt defineras
            // som position + en "frammåtvektor" (0, 0, -1).

            lookAt = position + Vector3.Forward; 


            // för att kunna rotera kameran behövs ett lite anorlunda sätt att 
            // definera kamerans punkt att titta mot. först räknar man ut
            // rotationsmatrisen, sedan multiplicerar man den med vad som är
            // frammåt. då får man en ny punkt, placerad en enhet från mitten (0, 0, 0).
            // kamerans punkt att titta på blir då positionen + den roterade punkten.

            //Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(0)); // Bevis - lägg till decimaler
            //Vector3 lookAtOffset = Vector3.Transform(Vector3.Forward, rotationMatrix);
            //Console.WriteLine(lookAtOffset);
            //lookAt = position + lookAtOffset;
            #endregion

            #region Viewmatris
            // Viewmatrisen är själva kameran. Denna behöver veta kamerans position, 
            // punkten den ska titta mot och vad som är upp. Den behöver veta vad 
            // som är upp eftersom även om man står på en position och tittar mot
            // en punkt, är det exakt likadana värden om man står upp och ner
            // på samma ställe och tittar mot samma punkt.
            view = Matrix.CreateLookAt(position, lookAt, Vector3.Up);
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            #region Modellen får värden
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

            // Varje modell är gjord utav meshes, och varje mesh har effekter som kallas shaders.
            // När man ritar ut en modell måste man loopa igenom varje mesh, och den meshens
            // shaders, och berätta hur meshen ska ritas ut - vilka "effekter" som ska påverka 
            // den. Det finns många olika saker man kan lägga till, men man behöver minst
            // 3 (eller 4). Meshen behöver veta var den ska ritas ut, d.v.s. modellens
            // position, rotation och skala, vilket är definerat i modellens matris. Den
            // behöver också veta var vad kameran gör, och hur vår projektionsmatris ser 
            // ut. Dessa 3 saker måste man berätta för varje shader i varje mesh. 
            // Det finns många saker man kan justera för övrigt, men en sak man ofta vill ha
            // är EnableDefaultLightning, vilket aktiverar modellens ljusinställningar. 
            // Utan ljuset blir modellens kanter inte synliga.

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect shader in mesh.Effects)
                {
                    //shader.EnableDefaultLighting(); // Behövs inte
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
