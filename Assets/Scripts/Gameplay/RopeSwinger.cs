using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeSwinger : MonoBehaviour
{
    [SerializeField] private float _cooldown = 0.5f;
    [SerializeField] private Vector2 _offset = Vector2.zero;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _distanceToDurationRatio = 5f;
    private Rigidbody2D _rigidbody;

    private Transform _holdPoint;
    private bool _isConnected = false;
    private SwingableRope _rope;
    private Vector3 _targetPosition;
    private Vector3 _targetEulerRotation;
    private float _originalZRotation;
    private Vector2 _appliedOffset = Vector2.zero;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if(_animator == null) _animator = GetComponentInChildren<Animator>();

        _targetPosition = transform.position;
        _targetEulerRotation = transform.eulerAngles;
        _originalZRotation = transform.eulerAngles.z;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _rope = other.GetComponent<SwingableRope>();
        if(_rope != null && !_isConnected)
        {

            _holdPoint = _rope.HoldPoint;
            Debug.Log("" + _holdPoint.position);
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            StartCoroutine(SmoothConnectToHoldPoint());
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_isConnected && context.started)
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _targetEulerRotation.Set(transform.eulerAngles.x, transform.eulerAngles.y, _originalZRotation);
            transform.eulerAngles = _targetEulerRotation;
            _rope.DisableHold(_cooldown);
            _animator.SetBool(AnimationStrings.holdRope, false);
            _isConnected = false;
        }
    }

    void Update()
    {
        if (_isConnected)
        {
            _appliedOffset.Set(_offset.x * (-transform.localScale.x), _offset.y);
            _targetPosition = _holdPoint.TransformPoint(_appliedOffset);

            //_position.Set(_holdPoint.position.x, _holdPoint.position.y, transform.position.z);
            transform.position = _targetPosition;

            _targetEulerRotation.Set(transform.eulerAngles.x, transform.eulerAngles.y, _holdPoint.eulerAngles.z);
            transform.eulerAngles = _targetEulerRotation;
        }
    }

    private IEnumerator SmoothConnectToHoldPoint()
    {
        // Get the starting position and rotation
        Vector3 startPosition = transform.position;
        

        // Calculate the target position and rotation
        _appliedOffset.Set(_offset.x * (-transform.localScale.x), _offset.y);
        _targetPosition = _holdPoint.TransformPoint(_appliedOffset);
        _targetEulerRotation.Set(transform.eulerAngles.x, transform.eulerAngles.y, _holdPoint.eulerAngles.z);

        float transitionDuration = Vector3.Distance(startPosition, _targetPosition) / _distanceToDurationRatio;


        // Smoothly move and rotate over time
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            transitionDuration = Vector3.Distance(startPosition, _targetPosition) / _distanceToDurationRatio;
            _targetPosition = _holdPoint.TransformPoint(_appliedOffset);
            transform.position = Vector3.Lerp(startPosition, _targetPosition, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position and rotation are exact
        transform.position = _targetPosition;
        transform.eulerAngles = _targetEulerRotation;

        _animator.SetBool(AnimationStrings.holdRope, true);
        _rope.Swing(transform.localScale.x > 0);
        _isConnected = true;

    }
}
