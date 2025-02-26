using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayDialogueInDialogueScene : MonoBehaviour {
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private MissionState missionState;
    
    [Header("If No Current Mission")]
    [SerializeField] private string yarnNode;
        
    // Start is called before the first frame update
    void Start()
    {
        if (missionState.CurrentScene != null && missionState.CurrentScene is DialogueScene dialogueScene) {
            if (dialogueScene.yarnNode == String.Empty) {
                missionState.LoadNextScene();
                return;
            }
            
            dialogueRunner.StartDialogue(dialogueScene.yarnNode);
            dialogueRunner.onDialogueComplete.AddListener(missionState.LoadNextScene);
        }
        else {
            dialogueRunner.StartDialogue(yarnNode);
        }
    }
}
