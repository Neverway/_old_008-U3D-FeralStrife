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

public class NW_NetworkManager : MonoBehaviour
{
    public static NW_NetworkManager instance;

    public GameObject playerPrefab;

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

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        //#if UNITY_EDITOR
        //Debug.LogWarning("The project needs to be built to launch the server!");
        //#else
        NW_Server.Start(4, 25568);
        //#endif
    }

    private void OnApplicationQuit()
    {
        NW_Server.Stop();
    }

    public NW_Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<NW_Player>();
    }
}