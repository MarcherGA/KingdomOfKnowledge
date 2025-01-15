using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomScrollRect : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Scroll Settings")]
    [SerializeField] private RectTransform _content;       // The content to scroll
    [SerializeField] private RectTransform _viewport;      // The viewport that holds the content
    [SerializeField] private float _scrollSpeed = 0.5f;    // Speed of scrolling
    [SerializeField] private bool _horizontalScroll = true; // Whether horizontal scroll is allowed
    [SerializeField] private bool _verticalScroll = true;   // Whether vertical scroll is allowed

    private Vector2 _dragStartPosition;   // The starting position of the drag
    private Vector2 _lastContentPosition; // The content position when dragging started
    private Vector2 _velocity;            // The scroll velocity (for smooth scrolling)

    private void Start()
    {
        // Initializing values
        _lastContentPosition = _content.anchoredPosition;
    }

    // OnBeginDrag captures the starting position of the drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Store the current position of the content when dragging begins
        _dragStartPosition = eventData.position;
        _lastContentPosition = _content.anchoredPosition;
    }

    // OnDrag updates the content position based on the dragging movement
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - _dragStartPosition;

        // Calculate the new position based on the drag delta
        Vector2 newPosition = _lastContentPosition + delta * _scrollSpeed;

        // Clamp the position to avoid content going out of bounds
        if (_horizontalScroll)
        {
            newPosition.x = Mathf.Clamp(newPosition.x, 0, _content.rect.width - _viewport.rect.width);
        }
        if (_verticalScroll)
        {
            newPosition.y = Mathf.Clamp(newPosition.y, 0, _content.rect.height - _viewport.rect.height);
        }

        // Set the new position
        _content.anchoredPosition = newPosition;
    }

    // OnEndDrag applies any final momentum or smooth scrolling when the drag ends
    public void OnEndDrag(PointerEventData eventData)
    {
        // Optionally, apply momentum by storing the velocity from the drag
        Vector2 dragDelta = eventData.position - _dragStartPosition;
        _velocity = dragDelta / Time.deltaTime;

        // Apply smooth scrolling
        StartCoroutine(SmoothScroll());
    }

    // Smooth scrolling (optional) to slow down the content after a drag
    private IEnumerator SmoothScroll()
    {
        float damping = 0.9f; // Apply some damping to the velocity

        // Reduce the velocity over time for smooth deceleration
        while (_velocity.magnitude > 0.1f)
        {
            Vector2 movement = _velocity * Time.deltaTime;

            // Apply the movement to the content position
            _content.anchoredPosition += movement;

            // Apply damping to the velocity
            _velocity *= damping;

            // Optionally, clamp the position after each frame
            if (_horizontalScroll)
            {
                _content.anchoredPosition = new Vector2(Mathf.Clamp(_content.anchoredPosition.x, 0, _content.rect.width - _viewport.rect.width), _content.anchoredPosition.y);
            }
            if (_verticalScroll)
            {
                _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, Mathf.Clamp(_content.anchoredPosition.y, 0, _content.rect.height - _viewport.rect.height));
            }

            yield return null;
        }
    }
}
