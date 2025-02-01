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
    [SerializeField] private List<GameObject> disableDuringDialogue;

    private string _dialogueAfterShop;
    private bool _continueButtonClicked;
        
    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner.onDialogueStart.AddListener(() => {
            foreach (var obj in disableDuringDialogue) {
                obj.SetActive(false);
            }
        });
        dialogueRunner.onDialogueComplete.AddListener(() => {
            if (_continueButtonClicked) return; // don't re-enable things if next scene is loading
            foreach (var obj in disableDuringDialogue) {
                obj.SetActive(true);
            }
        });
        
        if (missionState.CurrentScene != null && missionState.CurrentScene is ShopScene shopScene) {
            _dialogueAfterShop = shopScene.dialogueAfterShop;
            
            if (shopScene.dialogueBeforeShop != String.Empty) {
                dialogueRunner.StartDialogue(shopScene.dialogueBeforeShop);
            }
        }
    }

    public void ContinueButton() {
        if (_dialogueAfterShop == String.Empty) {
            missionState.LoadNextScene();
        }
        else {
            dialogueRunner.StartDialogue(_dialogueAfterShop);
            dialogueRunner.onDialogueComplete.AddListener(missionState.LoadNextScene);
        }

        _continueButtonClicked = true;
    }
}