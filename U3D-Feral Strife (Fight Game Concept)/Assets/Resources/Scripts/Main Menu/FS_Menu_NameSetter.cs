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

public class FS_Menu_NameSetter : MonoBehaviour
{
    // Public variables
    public Text nameField;

    // Private variables


    // Reference variables
    private FS_System_Config configManager;


    void Start()
    {
        configManager = FindObjectOfType<FS_System_Config>();
    }


    void Update()
    {
        
    }
    
    public void SetUsername()
    {
        if (nameField.text.Length >= 3)
        {
            configManager.username = nameField.text;
        }
    }
    
    public void SetLobbyName()
    {
        if (nameField.text.Length >= 3)
        {
            configManager.lobbyName = nameField.text;
        }
    }
}
