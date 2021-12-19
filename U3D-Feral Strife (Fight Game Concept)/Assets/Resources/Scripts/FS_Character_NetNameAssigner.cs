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
using TMPro;

public class FS_Character_NetNameAssigner : MonoBehaviour
{
    // Public variables
    public TextMeshPro TMPReference;

    // Private variables

    // Reference variables
    private PlayerManager playerManager;


    void Start()
    {
        playerManager = gameObject.GetComponent<PlayerManager>();
        TMPReference.text = playerManager.username;
    }


    void Update()
    {
	
    }
}
