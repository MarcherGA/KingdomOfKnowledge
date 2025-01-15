using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class InfiniteScroll : MonoBehaviour
{
    private ScrollRect _scrollRect;
    private RectTransform _content;
    private RectTransform _viewport;
    private HorizontalLayoutGroup _horizontalLayoutGroup;

    private RectTransform[] _items;

    private Vector2 _lastVelocity = Vector2.zero;
    private bool _isUpdated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _content = _scrollRect.content;
        _viewport = _scrollRect.viewport;
        _horizontalLayoutGroup = _content.GetComponent<HorizontalLayoutGroup>();

        _items = new RectTransform[_content.childCount];
        for (int i = 0; i < _content.childCount; i++)
        {
            _items[i] = _content.GetChild(i).GetComponent<RectTransform>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(_isUpdated)
        {
            _scrollRect.velocity = _lastVelocity;
            _isUpdated = false;
        }

        if(_content.localPosition.x > 0)
        {
            _lastVelocity = _scrollRect.velocity;
            _content.localPosition -= new Vector3(_items.Length * (_items[0].rect.width + _horizontalLayoutGroup.spacing), 0f, 0f);
            _isUpdated = true;
        }
        else if (_content.localPosition.x < 0 - (_items.Length * (_items[0].rect.width + _horizontalLayoutGroup.spacing)))
        {
            _lastVelocity = _scrollRect.velocity;
            _content.localPosition += new Vector3(_items.Length * (_items[0].rect.width + _horizontalLayoutGroup.spacing), 0f, 0f);
            _isUpdated = true;
        }
    }
}
