using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MissionState", menuName = "Scriptable Objects/MissionState", order = 0)]
public class MissionState : ScriptableObject {
    [SerializeField] private SceneLoader sceneLoader;

    [Space] 
    [SerializeField] private MissionData currentMission;
    [SerializeField] private SceneType currentScene;
    public SceneType CurrentScene => currentScene;
    
    public void UnbindCurrentMission() {
        currentMission = null;
        currentScene = null;
    }

    public void LoadCurrentScene() {
        currentScene = currentMission.GetCurrentScene();
        
        if (CurrentScene != null) {
            SceneManager.LoadScene(CurrentScene.unityScene.ScenePath);
        }
        else {
            currentMission.ResetState();
            sceneLoader.LoadMainMenu();
        }
    }
    public void CompleteCurrentScene() {
        currentMission.NextScene();
    }
    public void LoadNextScene() {
        CompleteCurrentScene();
        LoadCurrentScene();
    }
}