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
                        {
                            string headStringMessage = incmsg.ReadString();

                            switch (headStringMessage)
                            {
                                case "connect":
                                    {
                                        string name = incmsg.ReadString();
                                        int x = incmsg.ReadInt32();
                                        int y = incmsg.ReadInt32();
                                        int z = incmsg.ReadInt32();

                                        if (name != Constants.name)
                                            Enemy.players.Add(new Player(name, Constants.model));

                                        for (int i1 = 0; i1 < Enemy.players.Count; i1++)
                                        {
                                            for (int i2 = /*0*/i1 + 1; i2 < Enemy.players.Count; i2++)
                                            {
                                                if (i1 != i2 && Enemy.players[i1].name.Equals(Enemy.players[i2].name))
                                                {
                                                    Enemy.players.RemoveAt(i1);
                                                    i1--;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case "move":
                                    {
                                        try
                                        {
                                            string name = incmsg.ReadString();
                                            int x = incmsg.ReadInt32();
                                            int y = incmsg.ReadInt32();
                                            int z = incmsg.ReadInt32();

                                            for (int i = 0; i < Enemy.players.Count; i++)
                                            {
                                                if (Enemy.players[i].name.Equals(name) && Enemy.players[i].name != Constants.name)
                                                {
                                                    Enemy.players[i].remotePosition = new Vector3(x, y, z);
                                                    //Enemy.players[i].SetWorldMatrix(Matrix.CreateTranslation(x, y, z));
                                                    break;
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                    break;

                                case "disconnect":
                                    {
                                        string name = incmsg.ReadString();

                                        for (int i = 0; i < Enemy.players.Count; i++)
                                        {
                                            if (Enemy.players[i].name.Equals(name))
                                            {
                                                Enemy.players.RemoveAt(i);
                                                i--;
                                                break;
                                            }
                                        }
                                    }
                                    break;

                                case "name taken":
                                    {
                                    }
                                    break;
                            }
                        }
                        break;
                }
                Client.Recycle(incmsg);
            }
        }
    }
}