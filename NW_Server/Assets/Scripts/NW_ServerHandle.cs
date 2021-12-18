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

public class NW_ServerHandle
{
    public static void WelcomeReceived(int _fromClient, NW_Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        //Debug.Log($"{NW_Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint}");
        //Debug.Log($"{_fromClient}.");
        //Debug.Log($"{NW_Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        NW_Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromClient, NW_Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();

        NW_Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
    }

    public static void PlayerMovement2(int _fromClient, NW_Packet _packet)
    {
        
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        NW_Server.clients[_fromClient].player.SetInput2(_position, _rotation);
    }
}
