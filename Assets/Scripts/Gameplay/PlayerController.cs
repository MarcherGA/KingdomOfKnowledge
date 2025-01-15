using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
    
[RequireComponent(typeof(Rigidbody2D), typeof(CollisionDirections))]
public class PlayerController : MonoBehaviour
{
    public bool IsMoving { get 
        { 
            return _isMoving;
        } 
        private set
        {
            _isMoving = value;
            _animator?.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool IsRunning { get 
        { 
            return _isRunning;
        } 
        private set
        {
            _isRunning = value;
            _animator?.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public float CurrentMoveSpeed{ get
        {
            if(!CanMove || !CanWalk)
            {
                return 0;
            }

            if(IsMoving && !_collisionDirections.IsOnWall)
            {
                
                return _collisionDirections.IsGrounded ? (IsRunning ? _runSpeed : _walkSpeed) : _airWalkSpeed;
            }
            return 0;
        }
    }

    public bool IsFacingRight { get
        {
            return _isFacingRight;
        }
        private set
        {
            if(_isFacingRight != value)
            {
                _isFacingRight = value;
                transform.localScale *= _facingDirectionScaler;

            }
        }
    }

    public bool CanMove{ get
        {
            return _animator.GetBool(AnimationStrings.canMove);
        }
        set
        {
            _animator.SetBool(AnimationStrings.canMove, value);
        }
    }

    public bool CanWalk{ get
        {
            return _animator.GetBool(AnimationStrings.canWalk);
        }
        protected set
        {
            _animator.SetBool(AnimationStrings.canWalk, value);
        }
    }

    public bool IsAlive
    {
        get => _animator.GetBool(AnimationStrings.isAlive);
    }

    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private float _airWalkSpeed = 3f;

    [SerializeField] private float _jumpImpulse = 10f;

    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isRunning = false;

    private CollisionDirections _collisionDirections;
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private Vector2 _moveInput;
    private Vector2 _currentVelocity;
    private Vector3 _additionalVelocity = Vector3.zero; // Additional velocity to apply from external sources
    private bool _isFacingRight = true;
    private bool _isGroundedByExternalSource = false;

    private static Vector2 _facingDirectionScaler = new Vector2(-1,1);

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _collisionDirections = GetComponent<CollisionDirections>();
        _currentVelocity = new Vector2();
        CanWalk = true;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {
        SetVelocity(_moveInput.x * CurrentMoveSpeed + _additionalVelocity.x, _rigidBody.linearVelocity.y + _additionalVelocity.y);
        _animator.SetFloat(AnimationStrings.yVelocity, _rigidBody.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleVelocityChanger(collision.gameObject);
        HandleInteractableSurfaceEnter(collision.gameObject);
        
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleVelocityChanger(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        HandleVelocityChangerExit(collision.gameObject);
        HandleInteractableSurfaceExit(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleInteractableSurfaceEnter(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        HandleInteractableSurfaceExit(other.gameObject);
    }

    private void HandleVelocityChanger(GameObject objectToHandle)
    {
        IVelocityChanger velocityChanger = objectToHandle.GetComponent<IVelocityChanger>();
        if (velocityChanger != null)
        {
            _additionalVelocity = velocityChanger.GetVelocityToAdd();
        }
    }

    private void HandleVelocityChangerExit(GameObject objectToHandle)
    {
        IVelocityChanger velocityChanger = objectToHandle.GetComponent<IVelocityChanger>();
        if (velocityChanger != null)
        {
            _additionalVelocity = Vector3.zero;
        }
    }


    private void HandleInteractableSurfaceEnter(GameObject objectToHandle)
    {
        IInteractableSurface interactableSurface = objectToHandle.GetComponent<IInteractableSurface>();
        if (interactableSurface != null)
        {
            CanWalk = interactableSurface.CanMoveOnSurface;
            _isGroundedByExternalSource = interactableSurface.CanJumpOffSurface;
            if(interactableSurface.JumpForce != 0f)
            {
                Jump(interactableSurface.JumpForce);
            }
        }
    }

    private void HandleInteractableSurfaceExit(GameObject objectToHandle)
    {
        IInteractableSurface interactableSurface = objectToHandle.GetComponent<IInteractableSurface>();
        if (interactableSurface != null)
        {
            CanWalk = true;
            _isGroundedByExternalSource = false;
        }
    }

    private void SetVelocity(float xVelocity, float yVelocity)
    {
        _currentVelocity.Set(xVelocity, yVelocity);
        if (_rigidBody.linearVelocity != _currentVelocity)
        {
            _rigidBody.linearVelocity = _currentVelocity;
            
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Move(context.ReadValue<Vector2>());
    }

    public void Move(Vector2 moveInput)
    {
        _moveInput = moveInput;

        if(IsAlive && CanMove)
        {
            IsMoving = _moveInput != Vector2.zero;

            SetFacingDirection(_moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {   
        if(moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && CanMove && (_collisionDirections.IsGrounded || _isGroundedByExternalSource))
        {
            _animator.SetTrigger(AnimationStrings.jumpTrigger);
            Jump();
        }
    }


    private void Jump(float jumpForce)
    {
        SetVelocity(_rigidBody.linearVelocity.x, jumpForce);
    }

    private void Jump()
    {
        Jump(_jumpImpulse);
    }


}

