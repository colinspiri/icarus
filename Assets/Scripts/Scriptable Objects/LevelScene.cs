using UnityEngine;

[CreateAssetMenu(fileName = "LevelScene", menuName = "Missions/LevelScene", order = 0)]
public class LevelScene : SceneType {
    [Header("Level Scene Parameters")] 
    [Tooltip("Overrides money on level complete in MoneyConstants if != -1")]
    public int moneyOnComplete = -1;
    public WaveSet waveSet;

    [Header("Dialogue")] 
    public string dialogueBeforeWaves;
    public float delayOnDialogueBeforeWaves;
    [Space] 
    public string dialogueAfterWaves;
    public float delayOnDialogueAfterWaves;
}