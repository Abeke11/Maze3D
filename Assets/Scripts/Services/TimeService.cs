using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TimeService : ITimeService
{
    public IObservable<float> OnTick => throw new NotImplementedException();

    public IObservable<Unit> OnTimeUp => throw new NotImplementedException();

    public void AddTime(float seconds)
    {
        throw new NotImplementedException();
    }

    public void StartCountdown(float seconds)
    {
        throw new NotImplementedException();
    }
}