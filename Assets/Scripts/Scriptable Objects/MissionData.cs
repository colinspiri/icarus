using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MissionData", menuName = "MissionData", order = 0)]
public class MissionData : ScriptableObject {
    public List<SceneType> scenes;

    // state
    [ShowInInspector] public int CurrentSceneIndex { get; private set; }
    
    // references
    public SceneType CurrentScene => (CurrentSceneIndex < scenes.Count) ? scenes[CurrentSceneIndex] : null;

    // returns true if there is a next scene. if current scene was the last, it resets and returns false
    public bool NextScene() {
        CurrentSceneIndex++;

        if (CurrentSceneIndex >= scenes.Count) {
            CurrentSceneIndex = 0;
            return false;
        }
        return true;
    }
}