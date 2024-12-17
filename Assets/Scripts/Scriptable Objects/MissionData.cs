using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionX", menuName = "MissionData", order = 0)]
public class MissionData : ScriptableObject {
    public List<SceneType> scenes;

    // state
    [ShowInInspector] 
    private int _currentSceneIndex;

    private string SaveKey => name + "CurrentSceneIndex";

    public SceneType GetCurrentScene() {
        LoadData();
        return (_currentSceneIndex < scenes.Count) 
            ? scenes[_currentSceneIndex] : null;
    }

    public void NextScene() {
        _currentSceneIndex++;
        SaveData();
    }
    public void ResetState() {
        _currentSceneIndex = 0;
        ClearSaveData();
    }
    
    // saving/loading 
    private void SaveData() {
        PlayerPrefs.SetInt(SaveKey, _currentSceneIndex);
    }
    private void LoadData() {
        _currentSceneIndex = PlayerPrefs.GetInt(SaveKey, 0);
    }
    private void ClearSaveData() {
        PlayerPrefs.DeleteKey(SaveKey);
    }
}