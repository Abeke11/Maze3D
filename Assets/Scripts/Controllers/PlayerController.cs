using UnityEngine;
using UniRx;
using Zenject;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;

    [Header("References")]
    [SerializeField] private Transform cameraHolder;

    private IInputService _input;
    private CharacterController _cc;
    private Animator _animator;

    private Vector2 _moveInput;
    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _isSprinting;

    [Inject]
    public void Construct(IInputService inputService)
    {
        _input = inputService;
    }

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _input.MoveStream
              .Subscribe(v => _moveInput = v)
              .AddTo(this);

        _input.LookStream
              .Subscribe(delta =>
              {
                  transform.Rotate(Vector3.up, delta.x * Time.deltaTime * 100f);
                  cameraHolder.Rotate(Vector3.right, -delta.y * Time.deltaTime * 100f);
              })
              .AddTo(this);
        _input.JumpStream
              .Subscribe(_ =>
              {
                  if (_isGrounded)
                      _velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
              })
              .AddTo(this);

        _input.SprintStart
              .Subscribe(_ => _isSprinting = true)
              .AddTo(this);
        _input.SprintCancel
              .Subscribe(_ => _isSprinting = false)
              .AddTo(this);
    }

    private void Update()
    {
        _isGrounded = _cc.isGrounded;
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        float targetSpeed = _isSprinting ? sprintSpeed : walkSpeed;

        Vector3 moveDir = transform.right * _moveInput.x + transform.forward * _moveInput.y;

        _cc.Move(moveDir * targetSpeed * Time.deltaTime);

        _velocity += Physics.gravity * Time.deltaTime;
        _cc.Move(_velocity * Time.deltaTime);

        float currentSpeed = moveDir.magnitude * targetSpeed;
        float normalized = Mathf.Clamp01(currentSpeed / sprintSpeed);

        _animator.SetFloat("Speed", normalized);
    }
}
