﻿using Dragon_s_Breath.Assets;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dragon_s_Breath
{
    class Network
    {
        public static NetClient Client;
        public static NetPeerConfiguration Config;

        static NetIncomingMessage incmsg;
        public static NetOutgoingMessage outmsg;

        public static void Update()
        {
            while ((incmsg = Client.ReadMessage()) != null)
            {
                switch (incmsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        switch (incmsg.ReadString())
                        {
                            case "connect":
                                AddPlayer();
                                break;

                            case "move":
                                MovePlayer();
                                break;

                            case "disconnect":
                                RemovePlayer();
                                break;

                            case "name taken":
                                break;
                        }
                        break;
                }
                Client.Recycle(incmsg);
            }
        }

        private static void AddPlayer()
        {
            string name = incmsg.ReadString();
            int x = incmsg.ReadInt32();
            int y = incmsg.ReadInt32();
            int z = incmsg.ReadInt32();

            if (name != Constants.name)
                EnemyManager.Enemies.Add(new Enemy(name, 0, new Vector3(1000, 1100, 1000))); //Måste ändras så att korrekt modell och position skickas in

            for (int i = 0; i < EnemyManager.Enemies.Count; i++)
            {
                for (int j = /*0*/i + 1; j < EnemyManager.Enemies.Count; j++)
                {
                    if (i != j && EnemyManager.Enemies[i].Name.Equals(EnemyManager.Enemies[j].Name))
                    {
                        EnemyManager.Enemies.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
        }

        private static void MovePlayer()
        {
            try
            {
                string name = incmsg.ReadString();
                int x = incmsg.ReadInt32();
                int y = incmsg.ReadInt32();
                int z = incmsg.ReadInt32();

                for (int i = 0; i < EnemyManager.Enemies.Count; i++)
                {
                    if (EnemyManager.Enemies[i].Name.Equals(name) && EnemyManager.Enemies[i].Name != Constants.name)
                    {
                        EnemyManager.Enemies[i].remotePosition = new Vector3(x, y, z);
                        EnemyManager.Enemies[i].SetModelOrientation(EnemyManager.Enemies[i].ModelOrientation * Matrix.CreateTranslation(x, y, z));
                        break;
                    }
                }
            }
            catch
            {

            }
        }

        private static void RemovePlayer()
        {
            string name = incmsg.ReadString();

            for (int i = 0; i < EnemyManager.Enemies.Count; i++)
            {
                if (EnemyManager.Enemies[i].Name.Equals(name))
                {
                    EnemyManager.Enemies.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }
    }
}