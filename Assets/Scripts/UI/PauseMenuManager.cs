using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour {
    private PlayerInputActions _inputActions;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private HUDManager hud;
    [SerializeField] private List<GameObject> objectsToDisableOnPause;
    private List<bool> _wereObjectsActiveBeforePause = new List<bool>();

    // state
    private bool _gamePausedBeforePause;
    private bool _hudEnabledBeforePause;

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

        _hudEnabledBeforePause = hud.HUDEnabled;
        hud.SetHUDEnabled(false);
        
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
        
        hud.SetHUDEnabled(_hudEnabledBeforePause);

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