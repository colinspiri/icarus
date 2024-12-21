using UnityEngine;

[CreateAssetMenu(fileName = "LevelScene", menuName = "Missions/LevelScene", order = 0)]
public class LevelScene : SceneType {
    [Header("Level Scene Parameters")]
    public WaveSet waveSet;

    [Header("Dialogue")] 
    public string dialogueBeforeWaves;
    public float delayOnDialogueBeforeWaves;
    [Space] 
    public string dialogueAfterWaves;
    public float delayOnDialogueAfterWaves;
}