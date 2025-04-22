using System.Linq;
using UnityEngine;
using UniRx;
using Zenject;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelDatabaseSO levelDatabase;
    [SerializeField] Transform rootContainer;

    ITimeService _timeService;
    IGameStateService _gameState;
    IProgressService _progress;
    [Inject] DiContainer _container;

    [Inject]
    public void Construct(
        ITimeService ts,
        IGameStateService gs,
        IProgressService ps)
    {
        _timeService = ts;
        _gameState = gs;
        _progress = ps;
    }

    void Start()
    {
        int idx = LevelLoader.SelectedIndex;
        var config = levelDatabase.levels[idx];

        var labyrinth = Instantiate(
            config.labyrinthPrefab,
            rootContainer.position,
            rootContainer.rotation,
            rootContainer
        );

        var pSpawn = FindWithTag(labyrinth, "PlayerSpawn");
        _container.InstantiatePrefab(
            config.playerPrefab,
            pSpawn.position,
            pSpawn.rotation,
            labyrinth.transform
        );

        _timeService.StartCountdown(config.timeLimit);

        foreach (var bSpawn in FindAllWithTag(labyrinth, "BoosterSpawn"))
        {
            _container.InstantiatePrefab(
                config.boosterPrefab,
                bSpawn.position,
                Quaternion.identity,
                labyrinth.transform
            );
        }

        var eSpawn = FindWithTag(labyrinth, "ExitSpawn");
        var exitInstance = _container.InstantiatePrefab(
            config.exitPrefab,
            eSpawn.position,
            eSpawn.rotation,
            labyrinth.transform
        );

        var exitTrigger = exitInstance.GetComponent<ExitTrigger>();
        if (exitTrigger != null)
        {
            exitTrigger.OnPlayerExit
                       .First()  
                       .Subscribe(_ =>
                       {
                           float elapsed = config.timeLimit - _timeService.RemainingTime;
                           _progress.RecordResult(idx, elapsed);
                           
                           SceneManager.LoadScene("LevelSelect");
                       })
                       .AddTo(this);
        }

        _timeService.OnTimeUp
                    .First()
                    .Subscribe(_ =>
                    {
                        SceneManager.LoadScene("LevelSelect");
                    })
                    .AddTo(this);
    }

    Transform FindWithTag(GameObject root, string tag) =>
        root.GetComponentsInChildren<Transform>()
            .FirstOrDefault(t => t.CompareTag(tag))
        ?? root.transform;

    System.Collections.Generic.IEnumerable<Transform> FindAllWithTag(GameObject root, string tag) =>
        root.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag(tag));
}
