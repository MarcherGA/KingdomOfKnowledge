using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour, IVelocityChanger
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 _direction = Vector3.right; // Direction of movement
    [SerializeField] private float _distance = 5f; // Distance to move in the given direction
    [SerializeField] private float _speed = 2f; // Speed of movement (units per second)

    [Header("Pause Settings")]
    [SerializeField] private float _pauseDuration = 1f; // Pause duration at each edge

    private Rigidbody2D _rigidBody;
    private Vector3 _startPosition; // Starting position of the platform
    private Vector3 _endPosition; // End position of the platform
    private float _moveDuration; // Time to complete one move (one way)
    private float _cycleDuration; // Total time for a full cycle (move + pause)
    private float _offsetTime; // Tracks time offset for proper positioning after re-enable

    private Vector3 _lastPosition; // Previous position to calculate velocity
    private Vector3 _velocity; // Calculated velocity

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.bodyType = RigidbodyType2D.Kinematic; // Ensure the platform is not affected by physics forces
    }

    private void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition + _direction.normalized * _distance;
        _moveDuration = _distance / _speed;
        _cycleDuration = 2 * _moveDuration + 2 * _pauseDuration;

        _offsetTime = 0f;
        _lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Determine elapsed time in the current cycle, factoring in the offset
        float elapsedTime = (Time.time + _offsetTime) % _cycleDuration;

        Vector3 targetPosition = _startPosition; // Default position
        if (elapsedTime < _moveDuration) // Moving to end
        {
            float t = elapsedTime / _moveDuration;
            targetPosition = Vector3.Lerp(_startPosition, _endPosition, t);
        }
        else if (elapsedTime < _moveDuration + _pauseDuration) // Pausing at end
        {
            targetPosition = _endPosition;
        }
        else if (elapsedTime < 2 * _moveDuration + _pauseDuration) // Moving back to start
        {
            float t = (elapsedTime - _moveDuration - _pauseDuration) / _moveDuration;
            targetPosition = Vector3.Lerp(_endPosition, _startPosition, t);
        }
        else // Pausing at start
        {
            targetPosition = _startPosition;
        }

        // Move the platform using Rigidbody for smooth physics interaction
        _rigidBody.MovePosition(targetPosition);

        // Calculate velocity
        _velocity = (targetPosition - _lastPosition) / Time.fixedDeltaTime;
        _lastPosition = targetPosition;
    }

    private void OnEnable()
    {
        // Calculate offset so the platform resumes seamlessly
        _offsetTime = (Time.time % _cycleDuration - _offsetTime + _cycleDuration) % _cycleDuration;
    }

    private void OnDisable()
    {
        // Record the current offset for resuming later
        _offsetTime = (Time.time % _cycleDuration - _offsetTime + _cycleDuration) % _cycleDuration;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Vector3 endPos = transform.position + _direction.normalized * _distance;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, endPos);
            Gizmos.DrawSphere(endPos, 0.1f);
        }
    }

    public Vector3 GetVelocityToAdd()
    {
        return _velocity;
    }
}
