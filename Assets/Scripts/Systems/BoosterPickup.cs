using UnityEngine;
using UniRx;
using Zenject;

[RequireComponent(typeof(Collider))]
public class BoosterPickup : MonoBehaviour
{
    [SerializeField] float timeBonus = 5f;
    ITimeService _timeService;

    [Inject]
    public void Construct(ITimeService timeService)
    {
        _timeService = timeService;
    }

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _timeService.AddTime(timeBonus);
        Destroy(gameObject);
    }
}
