using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCurrentMission : MonoBehaviour {
    private static ResetCurrentMission _instance;
    
    [SerializeField] private MissionState missionState;

    private void Awake() {
        missionState.UnbindCurrentMission();

        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    
    private void OnApplicationQuit() {
        missionState.UnbindCurrentMission();
    }
}
