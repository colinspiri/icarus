using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomCursor : MonoBehaviour
{
    public static CustomCursor Instance;

    [SerializeField] private Texture2D reticleTexture;
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 cursorHotspot;
    [SerializeField] private SceneLoader sceneLoader;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetMenuCursor();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
/*        if (scene.path == sceneLoader.mainMenuScene.ScenePath || scene.path == 
            sceneLoader.dialogueScene.ScenePath)*/
        if (scene.path != sceneLoader.gameScene.ScenePath)
        {
            SetMenuCursor();
        }
        else
        {
            SetGameCursor();
        }
    }

    public void SetMenuCursor(Texture2D texture = null)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }

    public void SetGameCursor()
    {
        Cursor.SetCursor(reticleTexture, cursorHotspot, CursorMode.Auto);
    }
}
