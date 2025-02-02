using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MissionState", menuName = "Scriptable Objects/MissionState", order = 0)]
public class MissionState : ScriptableObject {
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private MoneyConstants moneyConstants;
    [SerializeField] private IntVariable currentMoney;

    [Space] 
    [SerializeField] private MissionData currentMission;
    [SerializeField] private SceneType currentScene;
    public SceneType CurrentScene => currentScene;
    
    public void SetCurrentMission(MissionData mission) {
        currentMission = mission;
    }
    
    public void UnbindCurrentMission() {
        currentMission = null;
        currentScene = null;
    }

    public void LoadCurrentScene() {
        currentScene = currentMission.GetCurrentScene();

        if (currentScene == null) {
            currentMission.ResetState();
            sceneLoader.LoadMainMenu();
        }
        else {
            SceneManager.LoadScene(currentScene.unityScene.ScenePath);
        }
    }
    private void CompleteCurrentScene() {
        // gain money
        if (currentScene is LevelScene level) {
            int moneyGained = (level.moneyOnComplete != -1)
                ? level.moneyOnComplete
                : moneyConstants.moneyOnCompleteLevel;
            currentMoney.Value += moneyGained;
        }
        
        currentMission.NextScene();
    }
    public void LoadNextScene() {
        if (currentMission == null) return;
        CompleteCurrentScene();
        LoadCurrentScene();
    }
}