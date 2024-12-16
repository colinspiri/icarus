using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MissionData", menuName = "MissionData", order = 0)]
public class MissionData : ScriptableObject {
    public List<SceneType> scenes;

    // references
    public SceneType CurrentScene => (CurrentSceneIndex < scenes.Count) ? scenes[CurrentSceneIndex] : null;
    
    // state
    [ShowInInspector] public int CurrentSceneIndex { get; private set; }

    public void PlayNextScene() {
        CurrentSceneIndex++;
    }

    public void ResetMission() {
        CurrentSceneIndex = 0;
    }
}