using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class PlayDialogueInGame : MonoBehaviour {
    private DialogueRunner _dialogueRunner;

    public bool playDialogueBeforeWaves;
    public float delayOnDialogueBeforeWaves = 0.5f;
    public string dialogueBeforeWaves;
    [Space] 
    public bool playDialogueAfterWaves;
    public float delayOnDialogueAfterWaves = 0.5f;
    public string dialogueAfterWaves;

    private void Awake() {
        _dialogueRunner = FindObjectOfType<DialogueRunner>();
        if (_dialogueRunner == null) {
            Debug.LogError("No Dialogue Runner found in scene.");
        }
        else {
            _dialogueRunner.onDialogueComplete.AddListener(() => GameManager.Instance.Resume(false));
        }
    }
    
    private void Start() {
        if (playDialogueBeforeWaves) {
            GameManager.Instance.Pause(false);
            StartCoroutine(StartDialogueAfterDelay(dialogueBeforeWaves, delayOnDialogueBeforeWaves));
        }

        EnemySpawner.Instance.OnCompleteWaves += OnWavesCompleted;
    }
    
    private IEnumerator StartDialogueAfterDelay(string yarnNode, float delay) {
        if (_dialogueRunner == null || yarnNode == string.Empty) {
            yield break;
        }
        
        yield return new WaitForSeconds(delay);
        
        _dialogueRunner.StartDialogue(yarnNode);
    }

    private void OnWavesCompleted() {
        if (playDialogueAfterWaves) {
            GameManager.Instance.Pause(false);
            StartCoroutine(StartDialogueAfterDelay(dialogueAfterWaves, delayOnDialogueAfterWaves));
        }
    }
}