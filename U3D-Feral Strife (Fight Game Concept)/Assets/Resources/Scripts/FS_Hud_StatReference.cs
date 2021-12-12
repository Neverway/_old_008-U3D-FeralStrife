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

public class FS_Hud_StatReference : MonoBehaviour
{
    // Public variables
    public string variableID;
    public string preappend;
    public string postappend;
    public int playerID;
    public bool useFunctionToUpdate;
    public Text UIReference;
    public TextMeshPro TMPReference;
    public InputField InputReference;

    // Private variables

    // Reference variables
    private FS_Character_Controller[] playerTargets;
    private FS_Character_Controller playerTarget;
    public FS_System_Config configManager;


    void Start()
    {
        print(InputReference);
        playerTargets = FindObjectsOfType<FS_Character_Controller>();
        configManager = FindObjectOfType<FS_System_Config>();
        for (int i = 0; i < playerTargets.Length; i++)
        {
            if (playerTargets[i].playerID == playerID)
            {
                playerTarget = playerTargets[i];
            }
        }
    }


    IEnumerator tick()
    {
        yield return new WaitForSeconds(0.1f);
        ReferenceStat();
    }


    void Update()
    {
        configManager = FindObjectOfType<FS_System_Config>();
        if (!useFunctionToUpdate)
        {
            ReferenceStat();
        }
    }

    public void DelayedReferenceStat()
    {
        StartCoroutine(tick());
    }

    public void ReferenceStat()
    {
        if (variableID == "percent")
        {
            if (UIReference != null)
            {
                UIReference.text = playerTarget.damage.ToString() + "%";
            }
            else if (TMPReference != null)
            {
                TMPReference.text = playerTarget.damage.ToString() + "%";
            }
        }
        else if (variableID == "fighter")
        {
            if (UIReference != null)
            {
                UIReference.text = preappend+playerTarget.entityName+postappend; 
            }
            else if (TMPReference != null)
            {
                TMPReference.text = preappend+playerTarget.entityName+postappend; 
            }
        }
        else if (variableID == "username")
        {
            if (UIReference != null)
            {
                UIReference.text = preappend+configManager.username+postappend; 
            }
            else if (TMPReference != null)
            {
                TMPReference.text = preappend+configManager.username+postappend; 
            }
            else if (InputReference != null)
            {
                InputReference.text = preappend+configManager.username+postappend; 
            }
        }
        else if (variableID == "lobbyName")
        {
            if (UIReference != null)
            {
                UIReference.text = preappend+configManager.lobbyName+postappend; 
            }
            else if (TMPReference != null)
            {
                TMPReference.text = preappend+configManager.lobbyName+postappend; 
            }
            else if (InputReference != null)
            {
                InputReference.text = preappend+configManager.lobbyName+postappend; 
            }
        }
    }
}
