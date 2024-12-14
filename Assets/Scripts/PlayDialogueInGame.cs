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
    }
    
    private void Start() {
        // set up callbacks on dialogue start & complete
        if (_dialogueRunner) {
            _dialogueRunner.onDialogueStart.AddListener(() => {
                HUDManager.Instance.SetHUDEnabled(false);
                GameManager.Instance.Pause(false);
            });
            _dialogueRunner.onDialogueComplete.AddListener(() => {
                HUDManager.Instance.SetHUDEnabled(true);
                GameManager.Instance.Resume(false);
            });
        }

        // either play dialogue before waves or activate enemy spawner
        if (playDialogueBeforeWaves) {
            HUDManager.Instance.SetHUDEnabled(false);
            StartCoroutine(StartDialogueAfterDelay(dialogueBeforeWaves, delayOnDialogueBeforeWaves, EnemySpawner.Instance.StartWaveSpawning));
        }
        else {
            EnemySpawner.Instance.StartWaveSpawning();
        }

        // set up callback for dialogue after waves
        if (playDialogueAfterWaves) {
            EnemySpawner.Instance.OnCompleteWaves += () => {
                HUDManager.Instance.SetHUDEnabled(false);
                StartCoroutine(StartDialogueAfterDelay(dialogueAfterWaves, delayOnDialogueAfterWaves));
            };
        }
    }
    
    private IEnumerator StartDialogueAfterDelay(string yarnNode, float delay, Action actionOnDialogueComplete = null) {
        if (_dialogueRunner == null || yarnNode == string.Empty) {
            yield break;
        }
        
        yield return new WaitForSeconds(delay);
        
        _dialogueRunner.StartDialogue(yarnNode);
        _dialogueRunner.onDialogueComplete.AddListener(OnDialogueCompleteWrapper);

        void OnDialogueCompleteWrapper() {
            actionOnDialogueComplete?.Invoke();
            _dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueCompleteWrapper);
        }
    }
}