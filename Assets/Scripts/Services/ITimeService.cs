using System;
using UniRx;
public interface ITimeService
{
    void StartCountdown(float seconds);

    void AddTime(float seconds);

    IObservable<float> OnTick { get; }

    IObservable<Unit> OnTimeUp { get; }

    float RemainingTime { get; }
}
