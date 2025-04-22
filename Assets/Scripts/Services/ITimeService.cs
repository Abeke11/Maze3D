using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface ITimeService
{
    void StartCountdown(float seconds);
    void AddTime(float seconds);
    IObservable<float> OnTick { get; }
    IObservable<Unit> OnTimeUp { get; }
}
