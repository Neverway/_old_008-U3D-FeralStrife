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

public class FS_Trigger_Hurtbox : MonoBehaviour
{
    // Public variables
    public float damage;
    public float framesActive;
    public float knockbackMultiplier=1.2f;
    public float baseKnockback=1;
    public Transform knockbackDirection;
    public List<GameObject> whitelistedTargets = new List<GameObject>();

    // Private variables
    public float calculatedKnockback;
    private GameObject intersectedObject;
    private FS_Character_Controller intersectedEntity;

    // Reference variables
    private void Start()
    {
        StartCoroutine(existanceCountdown());
    }


    IEnumerator existanceCountdown()
    {
        yield return new WaitForSeconds(framesActive);
        Destroy(gameObject);
    }



    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<FS_Character_RootReferencer>())
        {
            // Locate the player root
            intersectedObject = other.GetComponent<FS_Character_RootReferencer>().playerCharacterRoot;

            if (!whitelistedTargets.Contains(intersectedObject))
            {
                intersectedEntity = intersectedObject.GetComponent<FS_Character_Controller>();
                KnockbackCalculation();
                intersectedEntity.damage += damage;
                intersectedEntity.playerRigidbody.AddForce(knockbackDirection.forward * calculatedKnockback, ForceMode.Impulse);
                whitelistedTargets.Add(intersectedObject);
            }
        }
    }


    private void OnTriggerExit(Collider other) 
    {
        if (other.GetComponent<FS_Character_RootReferencer>())
        {
            intersectedObject = null;
        }
    }


    private void KnockbackCalculation()
    {
        /*
        ( ( ( ( (p / 10 + p * d / 20) * 200 / w + 100 * 1.3) + 18) * s) + b) * r 
        p = pre-damage percentage
        d = damage to be delt by hurt box
        w = weight
        s = knockback multiplier
        b = base knockback of hurtbox
        r = I don't think this is important so let's ignore it :/
        */
        calculatedKnockback = (((((intersectedEntity.damage / 10 + intersectedEntity.damage * damage / 20) * 200 / intersectedEntity.weight + 100 * 1.4f) + 18) * knockbackMultiplier) + baseKnockback);
    }
}
