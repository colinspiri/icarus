using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MissionState", menuName = "Scriptable Objects/MissionState", order = 0)]
public class MissionState : ScriptableObject {
    [SerializeField] private SceneLoader sceneLoader;
    
    [Space]
    [SerializeField] public MissionData currentMission;
    [SerializeField] public SceneType currentScene;

    public void SetCurrentMission(MissionData mission) {
        currentMission = mission;
    }
    public void UnbindCurrentMission() {
        currentMission = null;
        currentScene = null;
    }

    public void LoadCurrentScene() {
        currentScene = currentMission.GetCurrentScene();
        
        if (currentScene != null) {
            SceneManager.LoadScene(currentScene.unityScene.ScenePath);
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