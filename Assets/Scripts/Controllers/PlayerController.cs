
using UnityEngine;
using UniRx;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] Transform cameraHolder; 

    IInputService _input;
    CharacterController _cc;
    Vector2 _moveInput;
    Vector3 _velocity;
    bool _isGrounded;
    bool _isSprinting;

    [Inject]
    public void Construct(IInputService inputService)
    {
        _input = inputService;
    }

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    void Start()
    {
        _input.MoveStream
              .Subscribe(v => _moveInput = v)
              .AddTo(this);

        _input.LookStream
              .Subscribe(delta => {
                  transform.Rotate(Vector3.up, delta.x * Time.deltaTime * 100f);
                  cameraHolder.Rotate(Vector3.right, -delta.y * Time.deltaTime * 100f);
              })
              .AddTo(this);

        _input.JumpStream
              .Subscribe(_ => {
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

    void Update()
    {
        _isGrounded = _cc.isGrounded;
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        float speed = _isSprinting ? sprintSpeed : walkSpeed;
        Vector3 moveDir = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        _cc.Move(moveDir * speed * Time.deltaTime);

        _velocity += Physics.gravity * Time.deltaTime;
        _cc.Move(_velocity * Time.deltaTime);
    }
}
