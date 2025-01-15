using System.Collections;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    [SerializeField] private float _shakeDuration = 1f;
    [SerializeField] private float _breakDuration = 1f;
    [SerializeField] private float _resetDelay = 2f;
    [SerializeField] private float _fadeInDuration = 1f;
    [SerializeField] private float _fallDistance = 2f;
    
    private Animator _animator;
    private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private float _originalY;
    private float _timer = 0f;
    private bool _isBreaking = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _originalY = transform.position.y;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (_isBreaking) return;
        _isBreaking = true;
        _animator.SetTrigger(AnimationStrings.shakeTrigger);
        StartCoroutine(BreakCoroutine());
    }

    private IEnumerator BreakCoroutine()
    {
        yield return new WaitForSeconds(_shakeDuration);
        _animator.SetTrigger(AnimationStrings.breakTrigger);
        _collider2D.enabled = false;
        _timer = 0f;
        while (_timer < _breakDuration)
        {
            _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, Mathf.Lerp(1f, 0f, _timer / _breakDuration));
            transform.position = new Vector3(transform.position.x, _originalY - _fallDistance * _timer / _breakDuration, transform.position.z);
            yield return null;
            _timer += Time.deltaTime;
        }
        StartCoroutine(ResetCoroutine());
    }

    private IEnumerator ResetCoroutine()
    {
        yield return new WaitForSeconds(_resetDelay);
        _timer = 0f;
        transform.position = new Vector3(transform.position.x, _originalY, transform.position.z);
        _animator.SetTrigger(AnimationStrings.resetTrigger);
        while (_timer < _fadeInDuration)
        {
            _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, Mathf.Lerp(0f, 1f, _timer / _fadeInDuration));
            yield return null;
            _timer += Time.deltaTime;
        }
        _spriteRenderer.color = _originalColor;
        _collider2D.enabled = true;
        _isBreaking = false;
    }


}
