using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "CurrentMission", menuName = "CurrentMission", order = 0)]
public class CurrentMission : ScriptableObject {
    [SerializeField] private SceneLoader sceneLoader;
    
    [Space]
    [SerializeField] public MissionData currentMission;
    [SerializeField] public SceneType currentScene;

    public void SetCurrentMission(MissionData mission) {
        currentMission = mission;
    }

    public void LoadCurrentScene() {
        currentScene = currentMission.CurrentScene;
        if (currentScene != null) {
            SceneManager.LoadScene(currentScene.unityScene.ScenePath);
        }
        else {
            currentMission.currentSceneIndex = 0;
            sceneLoader.LoadMainMenu();
        }
    }
    public void CompleteCurrentScene() {
        currentMission.currentSceneIndex++;
    }
    public void LoadNextScene() {
        CompleteCurrentScene();
        LoadCurrentScene();
    }
}