using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds any player variables that he might have ex: movementSpeed, health, JumpForce etc.

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]

public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;
}
