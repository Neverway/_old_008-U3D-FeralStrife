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

public class NW_ServerSend
{
    private static void SendTCPData(int _toClient, NW_Packet _packet)
    {
        _packet.WriteLength();
        NW_Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendUDPData(int _toClient, NW_Packet _packet)
    {
        _packet.WriteLength();
        NW_Server.clients[_toClient].udp.SendData(_packet);
    }

    private static void SendTCPDataToAll(NW_Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= NW_Server.MaxPlayers; i++)
        {
            NW_Server.clients[i].tcp.SendData(_packet);
        }
    }
    private static void SendTCPDataToAll(int _exceptClient, NW_Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= NW_Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                NW_Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    private static void SendUDPDataToAll(NW_Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= NW_Server.MaxPlayers; i++)
        {
            NW_Server.clients[i].udp.SendData(_packet);
        }
    }
    private static void SendUDPDataToAll(int _exceptClient, NW_Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= NW_Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                NW_Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets
    public static void Welcome(int _toClient, string _msg)
    {
        using (NW_Packet _packet = new NW_Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void SpawnPlayer(int _toClient, NW_Player _player)
    {
        using (NW_Packet _packet = new NW_Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void PlayerPosition(NW_Player _player)
    {
        using (NW_Packet _packet = new NW_Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.netpos);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerRotation(NW_Player _player)
    {
        using (NW_Packet _packet = new NW_Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAll(_player.id, _packet);
        }
    }
    #endregion
}
