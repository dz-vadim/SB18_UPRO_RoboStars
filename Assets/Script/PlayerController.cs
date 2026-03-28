using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5f;
    private PlayerInput _inputActions;
    private CharacterController _controller;
    private Animator _animator;
    private Vector2 _movementInput;
    private Vector3 _currentMovement;
    private Quaternion _rotateDirection;
    private bool _isRun;
    private bool _isWalk;
    [SerializeField] private float _speed = 1, _runSpeed = 2;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _inputActions = new PlayerInput();
        _inputActions.CharacterControls.Movement.started += OnMoveAction;
        _inputActions.CharacterControls.Movement.performed += OnMoveAction;
        _inputActions.CharacterControls.Movement.canceled += OnMoveAction;
        
        _inputActions.CharacterControls.Run.started += OnRunAction;
        _inputActions.CharacterControls.Run.canceled += OnRunAction;
    }

    private void OnEnable()
    {
        _inputActions.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _inputActions.CharacterControls.Disable();
    }
    
    private void OnMoveAction(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _movementInput.x;
        _currentMovement.z = _movementInput.y;
        _isWalk = _movementInput.x != 0 || _movementInput.y != 0;
    }
    private void OnRunAction(InputAction.CallbackContext context)
    {
        _isRun = context.ReadValueAsButton();
    }
    private void PlayerRotate()
    {
        if (_isWalk)
        {
            _rotateDirection = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(_currentMovement),
                Time.deltaTime * rotateSpeed);
            
            transform.rotation = _rotateDirection;
        }
    }

    private void AnimateControl()
    {
        _animator.SetBool("isWalk", _isWalk);
        _animator.SetBool("isRun", _isRun);
    }
    private void Update()
    {
        AnimateControl();
        PlayerRotate();
    }
    private void FixedUpdate()
    {
        float moveSpeed = (_isRun) ? _runSpeed : _speed;
        _controller.Move( moveSpeed * _currentMovement * Time.fixedDeltaTime);
    }
}