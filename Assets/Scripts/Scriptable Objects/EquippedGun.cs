using UnityEngine;

[CreateAssetMenu(fileName = "EquippedGun", menuName = "Scriptable Objects/EquippedGun", order = 0)]
public class EquippedGun : ScriptableObject {
    [SerializeField] private GunConstants currentGun;
    public GunConstants CurrentGun => currentGun;
}