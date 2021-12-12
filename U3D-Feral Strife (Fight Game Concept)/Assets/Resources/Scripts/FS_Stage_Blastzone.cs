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

public class FS_Stage_Blastzone : MonoBehaviour
{
    // Public variables
    public Transform respawnPoint;
    public GameObject KOParticlePrefab;

    // Private variables
    private GameObject intersectedObject;

    // Reference variables


    private void OnTriggerExit(Collider other) 
    {
        if (other.GetComponent<FS_Character_RootReferencer>())
        {
            // Locate the player root
            intersectedObject = other.GetComponent<FS_Character_RootReferencer>().playerCharacterRoot;
            
            // Spawn the KO effect
            Instantiate(KOParticlePrefab, intersectedObject.transform.position, Quaternion.LookRotation((respawnPoint.position - intersectedObject.transform.position).normalized));
            
            // Respawn the player
            intersectedObject.transform.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y + intersectedObject.GetComponent<FS_Character_Controller>().playerHeight, respawnPoint.position.z);
            intersectedObject.GetComponent<FS_Character_Controller>().playerRigidbody.velocity = new Vector3(0,0,0);
            intersectedObject.GetComponent<FS_Character_Controller>().damage = 0;
        }
    }
}
