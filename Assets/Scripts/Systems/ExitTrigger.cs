using System;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExitTrigger : MonoBehaviour
{
    public IObservable<Unit> OnPlayerExit => _playerExit;
    readonly Subject<Unit> _playerExit = new Subject<Unit>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerExit.OnNext(Unit.Default);
            Debug.Log("Dalban");
        }
    }
}
