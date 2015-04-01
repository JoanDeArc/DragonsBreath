using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework; 

namespace DBserver
{
    public partial class Form2 : Form
    {
        Form form1;
        public Form2(string ip, int port, Form form1)
        {
            this.form1 = form1;
            InitializeComponent();

            label1.Text = "Server ip: " + ip;
            label2.Text = "Server port: " + port.ToString();

            Network.Config = new NetPeerConfiguration("DragonsBreath");  //"Kanalen" spelet körs på, måste vara samma i klienten och servern

            Network.Config.Port = port;
            Network.Config.LocalAddress = System.Net.IPAddress.Parse(ip);

            Network.Server = new NetServer(Network.Config);
            Network.Server.Start();

            textBox1.AppendText("Server started!" + "\r\n");
            textBox1.AppendText("Waiting for connections..." + "\r\n" + "\r\n");
        }

        private void timer1_Tick_1(object sender, EventArgs e) //Upddaterar nätverk och spelare varje tick (Tick interval 16 ≈ 60FPS)
        {
            Network.Update();
            Player.Update();
        }

        protected override void WndProc(ref Message m) //Körs om man försökter stänga fönstret, används mest för att också kunna stänga form1.
        {
            if (m.Msg == 0x10)
            {
                if (MessageBox.Show("Do you really want to exit?", "Close server", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                form1.Close();
            }
            base.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
    
    class Network
    {
        public static NetServer Server;             //Servern
        public static NetPeerConfiguration Config;  //Configurationen till servern

        static NetIncomingMessage incmsg;           //Inkommande meddelande
        public static NetOutgoingMessage outmsg;    //Utgående meddelande

        static bool playerRefresh;                  //Används för att inte kunna ansluta med samma namn som en annan spelare

        public static void Update()
        {
            while ((incmsg = Server.ReadMessage()) != null)   //Om vi får ett meddelande från en klient som inte är ingenting
            {
                switch (incmsg.MessageType)                   //Ändrar beroende på vilket sorts meddelande vi får, men vi använder bara Data typ just nu
                {
                    case NetIncomingMessageType.Data:
                        {
                            string message = incmsg.ReadString();  //Läser första delen av meddelandet

                            switch (message)                     //Ändrar beroende på vad första delen är, och på så sätt vet vi vad det handlar om
                            {
                                case "connect":
                                    {
                                        string name = incmsg.ReadString();  //Läser nästa del av meddelandet

                                        int x = incmsg.ReadInt32();     //Läser nästa del...
                                        int y = incmsg.ReadInt32(); 
                                        int z = incmsg.ReadInt32();

                                        playerRefresh = true;

                                        for (int i = 0; i < Player.players.Count; i++)
                                        {
                                            if (Player.players[i].name.Equals(name))
                                            {
                                                outmsg = Server.CreateMessage();  //Skapar ett meddelande
                                                outmsg.Write("name taken");       //Första delen av meddelandet är "name taken"

                                                Server.SendMessage(outmsg, incmsg.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);  //Skickar det till klienten

                                                incmsg.SenderConnection.Disconnect("Name taken");  //Disconnectar klienten
                                                System.Threading.Thread.Sleep(100);  //Liten paus för att se till att klienten hinner disconnecta
                                                playerRefresh = false;
                                                break;
                                            }
                                        }

                                        if (playerRefresh == true)
                                        {
                                            System.Threading.Thread.Sleep(100);
                                            Player.players.Add(new Player(name, Matrix.CreateTranslation(x, y, z), 0));

                                            Form2.textBox1.AppendText(name + " connected." + "\r\n");

                                            for (int i = 0; i < Player.players.Count; i++)
                                            {
                                                outmsg = Server.CreateMessage();

                                                outmsg.Write("connect");
                                                outmsg.Write(Player.players[i].name);
                                                outmsg.Write((int)Player.players[i].worldMatrix.Translation.X);
                                                outmsg.Write((int)Player.players[i].worldMatrix.Translation.Y);
                                                outmsg.Write((int)Player.players[i].worldMatrix.Translation.Z);
                                     
                                                Server.SendMessage(Network.outmsg, Network.Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);  //Skickar till alla anslutna klienter
                                            }
                                        }
                                    }
                                    break;

                                case "move": 
                                    {
                                        //Try catch används ifall meddelande inte är mottaget helt, utan bara en liten del. I så fall skulle programmet krasha
                                        //eftersom man försöker göra en translation med null.
                                        try
                                        {
                                            string name = incmsg.ReadString();
                                            int x = incmsg.ReadInt32();
                                            int y = incmsg.ReadInt32();
                                            int z = incmsg.ReadInt32();
                                            for (int i = 0; i < Player.players.Count; i++)
                                            {
                                                if (Player.players[i].name.Equals(name))
                                                {
                                                    Player.players[i].worldMatrix = Matrix.CreateTranslation(x, y, z);
                                                    Player.players[i].timeOut = 0; 
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

                                        for (int i = 0; i < Player.players.Count; i++)
                                        {
                                            if (Player.players[i].name.Equals(name))
                                            {
                                                Server.Connections[i].Disconnect("Bye");
                                                System.Threading.Thread.Sleep(100);

                                                Form2.textBox1.AppendText(name + " disconnected." + "\r\n");

                                                if (Server.ConnectionsCount != 0)
                                                {
                                                    outmsg = Server.CreateMessage();
                                                    outmsg.Write("disconnect");
                                                    outmsg.Write(name);
                                                    Server.SendMessage(Network.outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                                }

                                                Player.players.RemoveAt(i);
                                                i--;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
                //Alla meddelande till null
                Server.Recycle(incmsg);  
            }
        }
    }

    class Player
    {
        public string name;
        public Matrix worldMatrix;
        public int timeOut;

        public static List<Player> players = new List<Player>();

        public Player(string name, Matrix worldMatrix, int timeOut)
        {
            this.name = name;
            this.worldMatrix = worldMatrix;
            this.timeOut = timeOut;
        }

        public static void Update()
        {
            if (Network.Server.ConnectionsCount == players.Count)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    //Räkna hela tiden upp spelarens timeout.
                    players[i].timeOut++; 

                    Network.outmsg = Network.Server.CreateMessage();  //Skicka hela tiden meddelanden med alla spelares positioner till alla klienter

                    Network.outmsg.Write("move");
                    Network.outmsg.Write(players[i].name);
                    Network.outmsg.Write((int)Player.players[i].worldMatrix.Translation.X);
                    Network.outmsg.Write((int)Player.players[i].worldMatrix.Translation.Y);
                    Network.outmsg.Write((int)Player.players[i].worldMatrix.Translation.Z);

                    Network.Server.SendMessage(Network.outmsg, Network.Server.Connections, NetDeliveryMethod.Unreliable, 0);

                    if (players[i].timeOut > 1000) //Om timeout når 1000 så diconnectas spelaren, AFK
                    {
                        Network.Server.Connections[i].Disconnect("Bye");
                        Form2.textBox1.AppendText(players[i].name +  " timed out." + "\r\n");
                        System.Threading.Thread.Sleep(100);

                        if (Network.Server.ConnectionsCount != 0)
                        {
                            Network.outmsg = Network.Server.CreateMessage();

                            Network.outmsg.Write("disconnect");
                            Network.outmsg.Write(players[i].name);

                            Network.Server.SendMessage(Network.outmsg, Network.Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                        }

                        players.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
        }
    }
}
