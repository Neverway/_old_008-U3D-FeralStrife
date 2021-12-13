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
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class FS_Client : MonoBehaviour
{
    // Public variables
    public static FS_Client instance;
    public static int dataBufferSize = 4096; // 4mb of packet data

    public string ip = "127.0.0.1";
    public int port = 25568;
    public int myId = 0;
    public TCP tcp;

    // Private variables

    // Reference variables


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Destroying duplicate instance of FS_Client.cs");
            Destroy(this);
        }
    }


    private void Start()
    {
        tcp = new TCP();
    }


    public void ConnectToServer()
    {
        tcp.Connect();
    }


    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private byte[] receiveBuffer;


        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }
        

        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if(!socket.Connected)
            {
                return;
            }
            stream = socket.GetStream();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }


        private void ReceiveCallback(IAsyncResult _result)
        {
            try 
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    return;
                }
                
                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch //(Exception _ex)
            {
                //Console.WriteLine($"Error receiving TCP data: {_ex}");
            }
        }
    }
}
