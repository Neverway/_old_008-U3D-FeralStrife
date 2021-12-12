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

public class FS_Character_TriggerInstantiator : MonoBehaviour
{
    // Public variables
    public bool instantiate;
    [Range (0,1)]
    public int triggerType;
    public float damage;
    public float framesActive;
    public Vector3 scale = new Vector3(1, 1, 1);
    public int refpointID;

    // Private variables
    private bool allowingActivation = true;

    // Reference variables
    public GameObject[] refpointIDs;
    public GameObject HurtboxPrefab;
    public GameObject WindboxPrefab;


    private void Update()
    {
        if(instantiate && allowingActivation)
        {
            if (triggerType == 0)
            {
                allowingActivation = false;
                InstantiateHurtbox();
                instantiate = false;
            }
            
        }
    }

    public void InstantiateHurtbox()
    {
        GameObject triggerIntantiated = Instantiate(HurtboxPrefab, refpointIDs[refpointID].transform.position, refpointIDs[refpointID].transform.rotation);
        triggerIntantiated.GetComponent<FS_Trigger_Hurtbox>().whitelistedTargets.Add(gameObject);
        triggerIntantiated.GetComponent<FS_Trigger_Hurtbox>().damage = damage;
        triggerIntantiated.GetComponent<FS_Trigger_Hurtbox>().framesActive = framesActive;
        triggerIntantiated.transform.localScale = scale;
        allowingActivation = true;
    }
}
