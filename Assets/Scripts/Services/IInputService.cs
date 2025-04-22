// IInputService.cs
using UnityEngine;
using UniRx;
using System;

public interface IInputService
{
    IObservable<Vector2> MoveStream { get; }
    IObservable<Vector2> LookStream { get; }
    IObservable<Unit> JumpStream { get; }
    IObservable<Unit> SprintStart { get; }
    IObservable<Unit> SprintCancel { get; }
}
