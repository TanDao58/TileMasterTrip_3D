using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    [Tooltip("Multiple pawn in game ( * 3)")]
    [Range(1f, 10f)] public int level;
}

[CreateAssetMenu(menuName = "Data/Level Database", fileName = "LevelData")]
public class LevelDatabase : ScriptableObject
{
    public Level[] levels;
    public int GetLengthLevels() => levels.Length;
    public int GetLevelAtPos(int pos) => levels[pos].level;
}
