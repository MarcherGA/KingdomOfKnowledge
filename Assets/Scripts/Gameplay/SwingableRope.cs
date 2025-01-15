using System.Collections;
using UnityEngine;

public class SwingableRope : MonoBehaviour, IInteractableSurface
{
    public Transform HoldPoint => _holdPoint;
    [SerializeField] private float _pushForce = 10f;
    [SerializeField] private Transform _holdPoint;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private bool _disabled;


    public bool CanMoveOnSurface => false;

    public bool CanJumpOffSurface => true;

    public float JumpForce => 0f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public void Swing(bool isRight)
    {
        _rigidbody.linearVelocity = new Vector2(_pushForce * (isRight ? 1f : -1f), 0f);
    }

    public void DisableHold(float duration)
    {
        if (_disabled) return;
        _disabled = true;
        StartCoroutine(DisableHoldCoroutine(duration));
    }

    private IEnumerator DisableHoldCoroutine(float duration)
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(duration);
        _collider.enabled = true;
        _disabled = false;
    }
}
