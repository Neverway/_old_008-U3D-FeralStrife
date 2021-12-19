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

public class FS_StageChooser : MonoBehaviour
{
    // Public variables
    public string stageName;
    public string stageSceneID;
    public Sprite stageImage;

    // Private variables

    // Reference variables
    private FS_StagePuller stagePuller;


    void Start()
    {
        stagePuller = FindObjectOfType<FS_StagePuller>();
    }


    void Update()
    {
	
    }

    public void SendStageInfoToPuller()
    {
        stagePuller.stageName.text = stageName;
        stagePuller.stageSceneID = stageSceneID;
        stagePuller.stageImage.sprite = stageImage;
    }
}
