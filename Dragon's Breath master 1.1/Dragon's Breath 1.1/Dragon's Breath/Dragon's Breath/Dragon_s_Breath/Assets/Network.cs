using Dragon_s_Breath.Assets;
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
<<<<<<< HEAD
                float fx = incmsg.ReadFloat();
                float fy = incmsg.ReadFloat();
                float fz = incmsg.ReadFloat();
=======

                //float qx = incmsg.ReadFloat();
                //float qy = incmsg.ReadFloat();
                //float qz = incmsg.ReadFloat();
                //float qw = incmsg.ReadFloat();
                Quaternion q = new Quaternion(incmsg.ReadFloat(), incmsg.ReadFloat(), incmsg.ReadFloat(), incmsg.ReadFloat());
>>>>>>> origin/master

                for (int i = 0; i < EnemyManager.Enemies.Count; i++)
                {
                    if (EnemyManager.Enemies[i].Name.Equals(name) && EnemyManager.Enemies[i].Name != Constants.name)
                    {
<<<<<<< HEAD
                        EnemyManager.Enemies[i].newPosition = new Vector3(x, y, z);
                        EnemyManager.Enemies[i].newForward = new Vector3(fx, fy, fz);
                        
                        //EnemyManager.Enemies[i].SetWorldMatrix(EnemyManager.Enemies[i].WorldMatrix * Matrix.CreateTranslation(x, y, z), new Vector3(fx, fy, fz));
=======
                        EnemyManager.Enemies[i].remotePosition = new Vector3(x, y, z);
                        
                        //Måste göras om. måste flyttas till origo innan rotation sker och rotation ska ske endast med så mycket som rotationen ändrats sedan föregående paket.
                        EnemyManager.Enemies[i].SetModelOrientation(EnemyManager.Enemies[i].WorldMatrix * Matrix.CreateFromQuaternion(q));
                        //EnemyManager.Enemies[i].SetModelOrientation(EnemyManager.Enemies[i].WorldMatrix * Matrix.CreateTranslation(x, y, z));
>>>>>>> origin/master
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