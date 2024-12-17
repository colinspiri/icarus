using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class PlayDialogueInGame : MonoBehaviour {
    public static PlayDialogueInGame Instance;
    
    private DialogueRunner _dialogueRunner;

    [SerializeField] private CurrentMission currentMission;
    [Space]

    public string dialogueBeforeWaves;
    public float delayOnDialogueBeforeWaves = 0.5f;
    [Space] 
    public string dialogueAfterWaves;
    public float delayOnDialogueAfterWaves = 0.5f;

    
    // events 
    public event Action OnCompleteDialogueAfterWaves;

    private void Awake() {
        Instance = this;
        
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
        
        // if current scene in mission exists, use its settings
        if (currentMission.currentScene != null && currentMission.currentScene is LevelScene levelScene) {
            SetUpDialogueBeforeWaves(levelScene.dialogueBeforeWaves, levelScene.delayOnDialogueBeforeWaves);
            SetUpDialogueAfterWaves(levelScene.dialogueAfterWaves, levelScene.delayOnDialogueAfterWaves);
        }
        // otherwise use settings on the BASEGAME prefab
        else {
            SetUpDialogueBeforeWaves(dialogueBeforeWaves, delayOnDialogueBeforeWaves);
            SetUpDialogueAfterWaves(dialogueAfterWaves, delayOnDialogueAfterWaves);
        }
    }

    private void SetUpDialogueBeforeWaves(string dialogue, float delay) {
        if (dialogue == string.Empty) {
            EnemySpawner.Instance.StartWaveSpawning();
        }
        else {
            HUDManager.Instance.SetHUDEnabled(false);
            StartCoroutine(StartDialogueAfterDelay(dialogue, delay, EnemySpawner.Instance.StartWaveSpawning));
        }
    }

    private void SetUpDialogueAfterWaves(string dialogue, float delay) {
        if (dialogue != string.Empty) {
            EnemySpawner.Instance.OnCompleteWaves += () => {
                HUDManager.Instance.SetHUDEnabled(false);
                StartCoroutine(StartDialogueAfterDelay(dialogue, delay, OnCompleteDialogueAfterWaves));
            };
        }
        else {
            EnemySpawner.Instance.OnCompleteWaves += () => {
                OnCompleteDialogueAfterWaves?.Invoke();
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