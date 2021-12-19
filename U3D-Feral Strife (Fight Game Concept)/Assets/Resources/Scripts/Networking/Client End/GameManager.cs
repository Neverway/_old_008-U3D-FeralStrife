//=========== Written by Arthur W. Sheldon AKA Lizband_UCC ====================
//
// SID: 
// Purpose: 
// Applied to: 
// Editor script: 
// Notes: 
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Public variables
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public PlayerManager[] playerEntities;

    public GameObject localPlayerPrefab;
    public GameObject networkPlayerPrefab;

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
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position+new Vector3(_position.x,3,_position.z), _rotation);
        }
        else
        {
            _player = Instantiate(networkPlayerPrefab, _position+new Vector3(_position.x,3,_position.z), _rotation);
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void DisconnectPlayer(int _id)
    {
        playerEntities = FindObjectsOfType<PlayerManager>();
        
        for (int i = 0; i < playerEntities.Length; i++)
        {
            if (playerEntities[i].id == _id)
            {
                Debug.Log("Destroying game object: " + playerEntities[i] + " | Reason: Disconnected");
            }
        }
    }
}
