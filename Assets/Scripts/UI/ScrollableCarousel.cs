using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ScrollableCarousel : MonoBehaviour, IDragHandler
{
    [Header("Carousel Settings")]
    [SerializeField] private GameObject[] _items;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    [Header("Carousel Parameters")]
    [SerializeField] private float _speed = 10f;        // Speed of scrolling
    [SerializeField] private float _spacing = 150f;      // Spacing between items
    [SerializeField] private float _itemWidth = 200f;    // Width of each item
    [SerializeField] private float _itemHeight = 200f;   // Height of each item


    [Header("Scale Parameters")]
    [SerializeField] private float _maxScale = 1.5f;  // Maximum scale for the centered item (width & height)
    [SerializeField] private float _minScale = 1f;     // Minimum scale for the centered item (width & height)
    [SerializeField] private float _scaleSpeed = 5f;    // Speed of width/height change when moving
    [SerializeField] private float _scaleLerpSpeed = 5f;  // Speed of the resizing animation



    private int _selectedIndex = 2;  // Start from the middle item (index 2)
    private float _initialYPosition; // Store the initial Y position of the content panel
    private float _targetXPosition;  // The target X position to scroll to
    private Coroutine _currentCoroutine; // To keep track of the currently running coroutine

    private void Start()
    {
        PositionItems();
        _initialYPosition = _content.anchoredPosition.y;

        _leftButton.onClick.AddListener(MoveLeft);
        _rightButton.onClick.AddListener(MoveRight);
    }

    // Move the carousel to the right
    private void MoveRight()
    {
        if (_selectedIndex < _items.Length - 1)
        {
            _selectedIndex++;
            StartCarouselMove();
        }
    }

    // Move the carousel to the left
    private void MoveLeft()
    {
        if (_selectedIndex > 0)
        {
            _selectedIndex--;
            StartCarouselMove();
        }
    }

    // Start or restart the coroutine for moving the content
    private void StartCarouselMove()
    {
        // Stop any existing coroutines to avoid overlap
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        // Start the new movement coroutine
        _currentCoroutine = StartCoroutine(MoveContent());
    }

    // Position all the carousel items inside the content panel
    private void PositionItems()
    {
        // Calculate the total content width based on the number of items and the width of each item
        float contentWidth = (_items.Length * _itemWidth) + (_items.Length - 1) * _spacing;

        // Set the content panel's width dynamically based on the number of items
        _content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, contentWidth);

        // Set the spacing in the HorizontalLayoutGroup
        _content.GetComponent<HorizontalLayoutGroup>().spacing = _spacing;

        // Position each item based on its index
        for (int i = 0; i < _items.Length; i++)
        {
            RectTransform item = _items[i].GetComponent<RectTransform>();
            item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _itemWidth);  // Set width of each item
            item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _itemHeight);   // Set height of each item
            item.localScale = Vector3.one;  // Reset scale to default
        }
    }

    // Coroutine to smoothly move the content to the target position
    private IEnumerator MoveContent()
    {
        // Calculate the target X position based on the selected index
        _targetXPosition = -(_itemWidth / 2 + (_itemWidth + _spacing) * _selectedIndex);

        // Smoothly move the content to the target position
        while (Mathf.Abs(_content.anchoredPosition.x - _targetXPosition) > 1f)
        {
            // Linearly interpolate the current X position to the target position
            float newXPosition = Mathf.Lerp(_content.anchoredPosition.x, _targetXPosition, Time.deltaTime * _speed);
            _content.anchoredPosition = new Vector2(newXPosition, _initialYPosition);

            // Dynamically adjust the size of the items while the content is moving
            UpdateItemSize();

            yield return null;
        }

        // Ensure the final position is set accurately
        _content.anchoredPosition = new Vector2(_targetXPosition, _initialYPosition);
    }

     // Update the width and height of items based on their distance from the center with smooth animation
    private void UpdateItemSize()
    {
        // Get the center position of the content view (viewport center)
        float contentCenterX = _content.anchoredPosition.x + (_content.rect.width / 2);

        // Loop through all items and adjust their width and height
        for (int i = 0; i < _items.Length; i++)
        {
            RectTransform item = _items[i].GetComponent<RectTransform>();


            // Calculate the distance from the center (horizontal axis)
            float distanceFromCenter = Mathf.Abs(Mathf.Abs(_content.anchoredPosition.x) - ((_itemWidth + _spacing) * i + _spacing));

            // Calculate the scale factor based on the distance from the center
            float targetScale = Mathf.Lerp(_maxScale, _minScale, distanceFromCenter / (_itemWidth + _spacing));

            // Update the width and height based on the target scale
            item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _itemWidth * targetScale);
            item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _itemHeight * targetScale);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateItemSize();
    }

}
