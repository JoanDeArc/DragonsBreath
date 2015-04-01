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

namespace _3Dworld
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Terrain terrain;
        Effect effect;
        Camera camera;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            //this.IsMouseVisible = true;

            // Warning: don't set camera position to (0, 0, 0), use ones instead
            camera = new Camera(this.Window.ClientBounds, new Vector3(1000, 1100, 1000), new Vector3(0, 0, 200), Vector3.Up);
            camera.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            terrain = new Terrain(
                GraphicsDevice,
                Content.Load<Texture2D>(@"Textures\hmap512"),
                Content.Load<Texture2D>(@"Textures\grass"),
                16f,
                512,
                512,
                200f,
                10f);

            effect = Content.Load<Effect>(@"Effects/Terrain");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            camera.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            terrain.Draw(camera, effect);

            base.Draw(gameTime);
        }
    }
}
