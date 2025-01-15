using UnityEngine;

public class JumpPad : MonoBehaviour, IInteractableSurface
{
    [SerializeField] private float _force = 20f;
    
    private Animator _animator;

    public bool CanMoveOnSurface => true;

    public bool CanJumpOffSurface => false;

    public float JumpForce => _force;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _animator.SetTrigger(AnimationStrings.squeezeTrigger);
    }
}
