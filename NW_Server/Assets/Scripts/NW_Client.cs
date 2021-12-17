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

public class NW_Client
{
    public static int dataBufferSize = 4096;

    public int id;
    public NW_Player player;
    public TCP tcp;
    public UDP udp;

    public NW_Client(int _clientId)
    {
        id = _clientId;
        tcp = new TCP(id);
        udp = new UDP(id);
    }

    public class TCP
    {
        public TcpClient socket;

        private readonly int id;
        private NetworkStream stream;
        private NW_Packet receivedData;
        private byte[] receiveBuffer;

        public TCP(int _id)
        {
            id = _id;
        }

        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();

            receivedData = new NW_Packet();
            receiveBuffer = new byte[dataBufferSize];

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            NW_ServerSend.Welcome(id, "Welcome to the server!");
        }

        public void SendData(NW_Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to player {id} via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    NW_Server.clients[id].Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error receiving TCP data: {_ex}");
                    NW_Server.clients[id].Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                NW_ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (NW_Packet _packet = new NW_Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        NW_Server.packetHandlers[_packetId](id, _packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        public void Disconnect()
        {
            socket.Close();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    public class UDP
    {
        public IPEndPoint endPoint;

        private int id;

        public UDP(int _id)
        {
            id = _id;
        }

        public void Connect(IPEndPoint _endPoint)
        {
            endPoint = _endPoint;
        }

        public void SendData(NW_Packet _packet)
        {
            NW_Server.SendUDPData(endPoint, _packet);
        }

        public void HandleData(NW_Packet _packetData)
        {
            int _packetLength = _packetData.ReadInt();
            byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

            NW_ThreadManager.ExecuteOnMainThread(() =>
            {
                using (NW_Packet _packet = new NW_Packet(_packetBytes))
                {
                    int _packetId = _packet.ReadInt();
                    NW_Server.packetHandlers[_packetId](id, _packet);
                }
            });
        }

        public void Disconnect()
        {
            endPoint = null;
        }
    }

    public void SendIntoGame(string _playerName)
    {
        player = NW_NetworkManager.instance.InstantiatePlayer();
        player.Initialize(id, _playerName);

        foreach (NW_Client _client in NW_Server.clients.Values)
        {
            if (_client.player != null)
            {
                if (_client.id != id)
                {
                    NW_ServerSend.SpawnPlayer(id, _client.player);
                }
            }
        }

        foreach (NW_Client _client in NW_Server.clients.Values)
        {
            if (_client.player != null)
            {
                NW_ServerSend.SpawnPlayer(_client.id, player);
            }
        }
    }

    public void Disconnect()
    {
        Debug.Log($"{tcp.socket.Client.RemoteEndPoint} has disconnected.");

        UnityEngine.Object.Destroy(player.gameObject);
        player = null;

        tcp.Disconnect();
        udp.Disconnect();
    }
}
