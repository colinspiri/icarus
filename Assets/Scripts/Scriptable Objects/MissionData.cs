using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MissionData", menuName = "MissionData", order = 0)]
public class MissionData : ScriptableObject {
    public List<SceneType> scenes;

    // state
    [ShowInInspector] public int currentSceneIndex;
    
    // references
    public SceneType CurrentScene => (currentSceneIndex < scenes.Count) ? scenes[currentSceneIndex] : null;
}