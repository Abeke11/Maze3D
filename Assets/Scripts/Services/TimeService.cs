using System;
using UniRx;
using UnityEngine;

public class TimeService : ITimeService, IDisposable
{
    readonly Subject<float> _tickSubject = new Subject<float>();
    readonly Subject<Unit> _timeUpSubject = new Subject<Unit>();
    CompositeDisposable _disposables = new CompositeDisposable();

    public IObservable<float> OnTick => _tickSubject;
    public IObservable<Unit> OnTimeUp => _timeUpSubject;
    public float RemainingTime { get; private set; }

    public void StartCountdown(float seconds)
    {
        _disposables.Clear();
        RemainingTime = seconds;
        _tickSubject.OnNext(RemainingTime);

        Observable
            .Interval(TimeSpan.FromSeconds(1))
            .TakeWhile(_ => RemainingTime > 0)
            .Subscribe(_ =>
            {
                RemainingTime = Mathf.Max(0, RemainingTime - 1);
                _tickSubject.OnNext(RemainingTime);

                if (RemainingTime <= 0)
                    _timeUpSubject.OnNext(Unit.Default);
            })
            .AddTo(_disposables);
    }


    public void AddTime(float seconds)
    {
        RemainingTime = Mathf.Max(0, RemainingTime + seconds);
        _tickSubject.OnNext(RemainingTime);
    }

    public void Dispose()
    {
        _disposables.Dispose();
        _tickSubject.OnCompleted();
        _timeUpSubject.OnCompleted();
    }
}
