using System.Linq;
using UnityEngine;
using UniRx;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelDatabaseSO levelDatabase;
    [SerializeField] Transform rootContainer;

    ITimeService _timeService;
    IGameStateService _gameState;
    [Inject] DiContainer _container; 

    [Inject]
    public void Construct(ITimeService ts, IGameStateService gs)
    {
        _timeService = ts;
        _gameState = gs;
    }

    void Start()
    {
        var config = levelDatabase.levels[LevelLoader.SelectedIndex];

        var labyrinth = Instantiate(
            config.labyrinthPrefab,
            rootContainer.position,
            rootContainer.rotation,
            rootContainer
        );

        var playerSpawn = FindWithTagIn(labyrinth, "PlayerSpawn");
        _container.InstantiatePrefab(
            config.playerPrefab,
            playerSpawn.position,
            playerSpawn.rotation,
            labyrinth.transform
        );

        _timeService.StartCountdown(config.timeLimit);

        foreach (var spawn in FindAllWithTagIn(labyrinth, "BoosterSpawn"))
        {
            _container.InstantiatePrefab(
                config.boosterPrefab,
                spawn.position,
                Quaternion.identity,
                labyrinth.transform
            );
        }

        var exitSpawn = FindWithTagIn(labyrinth, "ExitSpawn");
        var exitObj = new GameObject("ExitTrigger");
        exitObj.transform.SetParent(labyrinth.transform);
        exitObj.transform.position = exitSpawn.position;
        exitObj.transform.rotation = exitSpawn.rotation;

        var col = exitObj.AddComponent<BoxCollider>();
        col.isTrigger = true;
        var trigger = exitObj.AddComponent<ExitTrigger>();
        trigger.OnPlayerExit
               .Subscribe(_ => _gameState.EndGame(true))
               .AddTo(this);

        _timeService.OnTimeUp
                    .Take(1)
                    .Subscribe(_ => _gameState.EndGame(false))
                    .AddTo(this);
    }

    Transform FindWithTagIn(GameObject root, string tag) =>
        root.GetComponentsInChildren<Transform>()
            .FirstOrDefault(t => t.CompareTag(tag))
        ?? root.transform;

    System.Collections.Generic.IEnumerable<Transform> FindAllWithTagIn(GameObject root, string tag) =>
        root.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag(tag));
}
