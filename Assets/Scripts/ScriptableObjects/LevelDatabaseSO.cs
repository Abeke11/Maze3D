using UnityEngine;

[CreateAssetMenu(menuName = "Maze/Level Database", fileName = "LevelDatabaseSO")]
public class LevelDatabaseSO : ScriptableObject
{
    public LevelConfigSO[] levels;
}

