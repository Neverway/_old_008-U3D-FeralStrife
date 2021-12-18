//=========== Written by Arthur W. Sheldon AKA Lizband_UCC ====================
//
// SID: 
// Purpose: 
// Applied to: 
// Editor script: 
// Notes: 
//
//=============================================================================

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NW_Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, NW_Client> clients = new Dictionary<int, NW_Client>();
        public delegate void PacketHandler(int _fromClient, NW_Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static void Start(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;

            Debug.Log("   ");
            Debug.Log(@"             :zRDz:             ");
            Debug.Log(@"         ~iO@@Qa6Q@@pi~         ");
            Debug.Log(@"     ,ip@@Qo+`  . '/6Q@@9i,     ");
            Debug.Log(@"  ?EQ@Qe*.      @Qe*. ,i9Q@Qw;  ");
            Debug.Log(@"  @@o-          ,/6Q@Qw?' ,Q@e  ");
            Debug.Log(@"  @@\ ~a;    '    '`,/6y:  W@e  ");
            Debug.Log(@"  @@\ ;@@'  ;@p`  Q^       W@e  ");
            Debug.Log(@"  @@\ ;@@'  ;@?N' Q^       W@e  ");
            Debug.Log(@"  @@\ ;@@'  ;@ ,#*Q^       W@e  ");
            Debug.Log(@"  @@\ ;@@'  ;@  `p@^       W@e  ");
            Debug.Log(@"  @@\ :Er   `:    ~`~i9y;  W@e  ");
            Debug.Log(@"  @@j`          ,/6Q@Qe*. .Q@e  ");
            Debug.Log(@"  tW@@NF;`      @Qar. .*eQ@Qw;  ");
            Debug.Log(@"    `^yB@@R];`  - -*eQ@@9i,     ");
            Debug.Log(@"        `^oQ@@NjeQ@@p7~         ");
            Debug.Log(@"            .*eQDz:             ");
            Debug.Log("   ");
            Debug.Log("***************************************************");
            Debug.Log("* Neverway Server Tools v1.0 | Feral Strife ID007 *");
            Debug.Log("***************************************************");
            Debug.Log("   ");
            Debug.Log("Starting server...");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            udpListener = new UdpClient(Port);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            Debug.Log($"Server started on port {Port}");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Debug.Log($"Incoming connection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            Debug.Log($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallback, null);

                if (_data.Length < 4)
                {
                    return;
                }

                using (NW_Packet _packet = new NW_Packet(_data))
                {
                    int _clientId = _packet.ReadInt();

                    if (_clientId == 0)
                    {
                        return;
                    }

                    if (clients[_clientId].udp.endPoint == null)
                    {
                        clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        clients[_clientId].udp.HandleData(_packet);
                    }
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error receiving UDP data: {_ex}");
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, NW_Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
            }
        }

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new NW_Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, NW_ServerHandle.WelcomeReceived },
                { (int)ClientPackets.playerMovement, NW_ServerHandle.PlayerMovement },
                { (int)ClientPackets.playerMovement2, NW_ServerHandle.PlayerMovement2 },
            };
            Debug.Log("Initialized packets.");
        }

        public static void Stop()
        {
            tcpListener.Stop();
            udpListener.Close();
        }
    }
