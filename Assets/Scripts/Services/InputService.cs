// InputService.cs
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using Game.Input;

public class InputService : IInputService, IDisposable
{
    readonly PlayerInputActions _actions;

    readonly Subject<Vector2> _moveSubject = new Subject<Vector2>();
    readonly Subject<Vector2> _lookSubject = new Subject<Vector2>();
    readonly Subject<Unit> _jumpSubject = new Subject<Unit>();
    readonly Subject<Unit> _sprintStartSubject = new Subject<Unit>();
    readonly Subject<Unit> _sprintEndSubject = new Subject<Unit>();

    public IObservable<Vector2> MoveStream => _moveSubject;
    public IObservable<Vector2> LookStream => _lookSubject;
    public IObservable<Unit> JumpStream => _jumpSubject;
    public IObservable<Unit> SprintStart => _sprintStartSubject;
    public IObservable<Unit> SprintCancel => _sprintEndSubject;

    public InputService()
    {
        _actions = new PlayerInputActions();

        // Move
        _actions.Player.Move.performed += ctx => _moveSubject.OnNext(ctx.ReadValue<Vector2>());
        _actions.Player.Move.canceled += ctx => _moveSubject.OnNext(Vector2.zero);

        // Look
        _actions.Player.Look.performed += ctx => _lookSubject.OnNext(ctx.ReadValue<Vector2>());

        // Jump
        _actions.Player.Jump.performed += ctx => _jumpSubject.OnNext(Unit.Default);

        // Sprint
        _actions.Player.Sprint.started += ctx => _sprintStartSubject.OnNext(Unit.Default);
        _actions.Player.Sprint.canceled += ctx => _sprintEndSubject.OnNext(Unit.Default);

        _actions.Enable();
    }

    public void Dispose()
    {
        _actions.Dispose();
        _moveSubject.OnCompleted();
        _lookSubject.OnCompleted();
        _jumpSubject.OnCompleted();
        _sprintStartSubject.OnCompleted();
        _sprintEndSubject.OnCompleted();
    }
}
