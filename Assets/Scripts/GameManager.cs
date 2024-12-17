using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    
    [SerializeField] private CurrentMission currentMission;
    
    public bool GamePaused { get; private set; }
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        PlayDialogueInGame.Instance.OnCompleteDialogueAfterWaves += LevelComplete;
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Pause(bool pauseTime = true) {
        GamePaused = true;

        if (pauseTime) {
            TimeManager.Instance.PauseTime();
            AudioListener.pause = true;
        }
    }

    public void Resume(bool resumeTime = true) {
        GamePaused = false;

        if (resumeTime) {
            TimeManager.Instance.ResumeTime();
            AudioListener.pause = false;
        }
    }

    public void ResumeTimeOnly() {
        TimeManager.Instance.ResumeTime();
        AudioListener.pause = false;
    }

    private void LevelComplete() {
        currentMission.LoadNextScene();
    }
}
