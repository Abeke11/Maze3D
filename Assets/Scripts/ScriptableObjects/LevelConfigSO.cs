using UnityEngine;

[CreateAssetMenu(menuName = "Maze/Level Config", fileName = "LevelConfigSO")]
public class LevelConfigSO : ScriptableObject
{
    [Header("����� ��� ������ ���������")]
    public GameObject labyrinthPrefab;

    [Header("����� �� ����������� (���)")]
    public float timeLimit = 60f;

    [Header("������ �������")]
    public GameObject boosterPrefab;
    [Header("������ ������")]
    public GameObject playerPrefab;

    [Header("��� ������ �������� ������ ���������")]
    public string boosterSpawnTag = "BoosterSpawn";

    [Header("��� ������ ������ ���������")]
    public string exitTag = "ExitSpawn";
}
