using UnityEngine;

[CreateAssetMenu(fileName = "ShopScene", menuName = "Missions/ShopScene", order = 0)]
public class ShopScene : SceneType {
    [Header("Shop Scene Parameters")]
    public string dialogueBeforeShop;
    public string dialogueAfterShop;
}