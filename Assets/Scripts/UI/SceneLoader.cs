using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoader", menuName = "Scriptable Objects/SceneLoader")]
public class SceneLoader : ScriptableObject {
    public SceneReference mainMenuScene;
    public SceneReference gameScene;

    [Header("Wave Set Scenes")] 
    public SceneReference bomberScene;
    public SceneReference prototypeEnemyScene;
    public SceneReference homingMissileScene;
    public SceneReference fighterScene;
    public SceneReference eliteEnemyScene;

    public void DebugTest(string testString) {
        Debug.Log("test " + testString + " at " + Time.time);
    }

    public void LoadGameScene() {
        ResetTime();
        SceneManager.LoadScene(gameScene.ScenePath);
    }

    public void Restart()
    {
        ResetTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu() {
        ResetTime();
        SceneManager.LoadScene(mainMenuScene.ScenePath);
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ClearPlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }

    private void ResetTime() {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
    
    public void LoadBomberScene() {
        ResetTime();
        SceneManager.LoadScene(bomberScene.ScenePath);
    }
    public void LoadPrototypeEnemyScene() {
        ResetTime();
        SceneManager.LoadScene(prototypeEnemyScene.ScenePath);
    }
    public void LoadHomingMissileScene() {
        ResetTime();
        SceneManager.LoadScene(homingMissileScene.ScenePath);
    }
    public void LoadFighterScene() {
        ResetTime();
        SceneManager.LoadScene(fighterScene.ScenePath);
    }
    public void LoadEliteEnemyScene() {
        ResetTime();
        SceneManager.LoadScene(eliteEnemyScene.ScenePath);
    }
}