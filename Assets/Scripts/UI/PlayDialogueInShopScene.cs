using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class PlayDialogueInShopScene : MonoBehaviour {
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private MissionState missionState;
    [Space] 
    [SerializeField] private Button nextButton;

    private string _dialogueAfterShop;
        
    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner.onDialogueStart.AddListener(() => {
            nextButton.gameObject.SetActive(false);
        });
        dialogueRunner.onDialogueComplete.AddListener(() => {
            nextButton.gameObject.SetActive(true);
        });
        
        if (missionState.CurrentScene != null && missionState.CurrentScene is ShopScene shopScene) {
            _dialogueAfterShop = shopScene.dialogueAfterShop;
            
            if (shopScene.dialogueBeforeShop != String.Empty) {
                dialogueRunner.StartDialogue(shopScene.dialogueBeforeShop);
            }
        }
    }

    public void NextButton() {
        if (_dialogueAfterShop == String.Empty) {
            missionState.LoadNextScene();
        }
        else {
            dialogueRunner.StartDialogue(_dialogueAfterShop);
            dialogueRunner.onDialogueComplete.AddListener(missionState.LoadNextScene);
        }
    }
}