using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour {
    public static PauseMenuManager Instance;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private List<GameObject> objectsToDisableOnPause;
    private List<bool> _wereObjectsActiveBeforePause = new List<bool>();
    
    private PlayerInputActions _inputActions;

    private bool _gamePausedBeforePause;

    private void Awake() {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
        
        foreach (var t in objectsToDisableOnPause) {
            _wereObjectsActiveBeforePause.Add(t.activeSelf);
        }
        pauseMenu.SetActive(false);
    }
    
    private void Update()
    {
        if (_inputActions.UI.Cancel.triggered && !pauseMenu.activeSelf)
        {
            OpenPauseMenu();
        }
    }

    private void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        
        for (int i = 0; i < objectsToDisableOnPause.Count; i++) {
            _wereObjectsActiveBeforePause[i] = objectsToDisableOnPause[i].activeSelf;
            objectsToDisableOnPause[i].SetActive(false);
        }

        _gamePausedBeforePause = GameManager.Instance.GamePaused;
        GameManager.Instance.Pause();
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        
        for (int i = 0; i < objectsToDisableOnPause.Count; i++) {
            objectsToDisableOnPause[i].SetActive(_wereObjectsActiveBeforePause[i]);
        }

        if (_gamePausedBeforePause) {
            GameManager.Instance.ResumeTimeOnly();
        }
        else {
            GameManager.Instance.Resume();
        }
    }
}