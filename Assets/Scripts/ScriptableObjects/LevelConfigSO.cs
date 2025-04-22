using UnityEngine;

[CreateAssetMenu(menuName = "Maze/Level Config", fileName = "LevelConfigSO")]
public class LevelConfigSO : ScriptableObject
{
    [Header("Сцена или префаб лабиринта")]
    public GameObject labyrinthPrefab;

    [Header("Время на прохождение (сек)")]
    public float timeLimit = 60f;

    [Header("Префаб бустера")]
    public GameObject boosterPrefab;
    [Header("Префаб игрока")]
    public GameObject playerPrefab;

    [Header("Тег спавна бустеров внутри лабиринта")]
    public string boosterSpawnTag = "BoosterSpawn";

    [Header("Тег выхода внутри лабиринта")]
    public string exitTag = "ExitSpawn";
}
