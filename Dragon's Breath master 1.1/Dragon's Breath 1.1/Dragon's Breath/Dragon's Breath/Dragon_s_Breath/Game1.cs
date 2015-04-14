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
using Lidgren.Network;
using Dragon_s_Breath.Assets;

namespace Dragon_s_Breath
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            // *Nätverksanslutning - Joanna Gladh och Daniel Aili*
            // Sätter upp vad som behövs för att starta klienten
            // Ansluter till servern och skickar sedan ett meddelande
            // till servern för att berätta var den befinner sig
            // och att den nu är ansluten.
            Network.Config = new NetPeerConfiguration("DragonsBreath");
            Network.Client = new NetClient(Network.Config);

            Network.Client.Start();
            Network.Client.Connect(Constants.ip, Constants.port);

            System.Threading.Thread.Sleep(300);

            Network.outmsg = Network.Client.CreateMessage();
            Network.outmsg.Write("connect");
            Network.outmsg.Write(Constants.name);
            Network.outmsg.Write(1000);
            Network.outmsg.Write(1100);
            Network.outmsg.Write(1000);
            Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.ReliableOrdered);

            System.Threading.Thread.Sleep(300);
            // *Nätverksanslutning />
            EnemyManager.Initialize();

            base.Initialize();
        }

        // *Override utav XNAs egna exitfunktion - Joanna Gladh och Daniel Aili*
        // Detta görs för att berätta för servern att spelaren avslutat sitt spel.
        protected override void OnExiting(Object sender, EventArgs args)
        {
            Network.outmsg = Network.Client.CreateMessage();
            Network.outmsg.Write("disconnect");
            Network.outmsg.Write(Constants.name);
            Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.ReliableOrdered);

            System.Threading.Thread.Sleep(300);

            base.OnExiting(sender, args);
        }


        protected override void LoadContent()
        {
            player = new Player(Constants.name, 0, Window.ClientBounds, new Vector3(1000f, 1100f, 1000f));
            ModelManager.LoadContent(Content);
            Terrain.LoadContent(GraphicsDevice, Content, 16f, 512, 512, 200f, 10f);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            player.Update(gameTime);
            EnemyManager.Update(gameTime);
            Network.Update();

            foreach (Enemy e in EnemyManager.Enemies)
            {
                Console.WriteLine(e.WorldMatrix.Translation);
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            player.Draw();
            EnemyManager.Draw(player.Camera);
            
            base.Draw(gameTime);
        }
    }
}
