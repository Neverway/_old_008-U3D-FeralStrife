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
using UnityEngine.UI;

public class FS_UIManager : MonoBehaviour
{
    // Public variables
    public static FS_UIManager instance;

    //public GameObject startMenu; 
    //public InputField usernameField; // Replace with configManager.username

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


    public void ConnectToServer()
    {
        FS_Client.instance.ConnectToServer();
    }
}
