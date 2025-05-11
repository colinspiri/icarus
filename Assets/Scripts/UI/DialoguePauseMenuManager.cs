using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class DialoguePauseMenuManager : PauseMenuManager
{
    public bool GamePaused { get; private set; }

    private float _fixedDeltaTime;
    private float _previousTimeScale = 1;

    protected override void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);

        for (int i = 0; i < objectsToDisableOnPause.Count; i++)
        {
            _wereObjectsActiveBeforePause[i] = objectsToDisableOnPause[i].activeSelf;
            objectsToDisableOnPause[i].SetActive(false);
        }

        _gamePausedBeforePause = GamePaused;
        Pause();
    }

    public override void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);

        for (int i = 0; i < objectsToDisableOnPause.Count; i++)
        {
            objectsToDisableOnPause[i].SetActive(_wereObjectsActiveBeforePause[i]);
        }

        if (_gamePausedBeforePause)
        {
            ResumeTimeOnly();
        }
        else
        {
            Resume();
        }
    }

    public void Pause(bool pauseTime = true)
    {
        GamePaused = true;

        if (pauseTime)
        {
            PauseTime();
            AudioListener.pause = true;
        }
    }

    public void Resume(bool resumeTime = true)
    {
        GamePaused = false;

        if (resumeTime)
        {
            ResumeTime();
            AudioListener.pause = false;
        }
    }

    public void ResumeTimeOnly()
    {
        ResumeTime();
        AudioListener.pause = false;
    }

    public void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
        Time.fixedDeltaTime = this._fixedDeltaTime * Time.timeScale;
    }

    public void PauseTime()
    {
        _previousTimeScale = Time.timeScale;
        SetTimeScale(0);
    }
    public void ResumeTime()
    {
        SetTimeScale(_previousTimeScale);
    }

    private void OnDestroy()
    {
        SetTimeScale(1);
    }
}
