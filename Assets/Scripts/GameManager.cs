using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    
    private bool _paused;
    public bool GamePaused => _paused;

    private void Awake() {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.Instance == null) {
            Reload();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Reload();
        }
    }

    public void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Pause(bool pauseAudio = true) {
        _paused = true;

        TimeManager.Instance.PauseTime();

        if (pauseAudio)
        {
            AudioListener.pause = true;
        }

    }

    public void Resume(bool resumeAudio = true) {
        _paused = false;
        
        TimeManager.Instance.ResumeTime();

        if (resumeAudio)
        {
            AudioListener.pause = false;
        }
    }
}
