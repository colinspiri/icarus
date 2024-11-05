using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Scriptable Objects/Player Info")]
public class PlayerInfo : ScriptableObject
{
    public float maxHealth;
    public float currentHealthPercentage;
}
