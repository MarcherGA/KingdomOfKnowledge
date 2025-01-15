using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider setup")]
    [SerializeField, Range(0, 1f)]
    protected float _sliderValue;
    public bool CurrentValue { get; private set; }

    private bool _previousValue;
    private Slider _slider;

    [Header("Animation")]
    [SerializeField, Range(0, 1f)] private float _animationDuration = 0.5f;
    [SerializeField] private AnimationCurve _slideEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine _animateSliderCoroutine;

    [Header("Events")]
    [SerializeField] public UnityEvent<bool> onValueChanged;

    [Header("Text")]
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _toggleOnText;
    [SerializeField] private string _toggleOffText;


    private ToggleSwitchGroupManager _toggleSwitchGroupManager;

    protected Action _transitionEffect;

    protected virtual void OnValidate()
    {
        SetupToggleComponents();

        _slider.value = _sliderValue;
    }

    private void SetupToggleComponents()
    {
        if (_slider != null)
            return;

        SetupSliderComponent();
    }

    private void SetupSliderComponent()
    {
        _slider = GetComponent<Slider>();

        if (_slider == null)
        {
            Debug.Log("No slider found!", this);
            return;
        }

        _slider.interactable = false;
        var sliderColors = _slider.colors;
        sliderColors.disabledColor = Color.white;
        _slider.colors = sliderColors;
        _slider.transition = Selectable.Transition.None;
    }

    public void SetupForManager(ToggleSwitchGroupManager manager)
    {
        _toggleSwitchGroupManager = manager;
    }


    protected virtual void Awake()
    {
        SetupSliderComponent();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }


    public void Toggle()
    {
        if (_toggleSwitchGroupManager != null)
            _toggleSwitchGroupManager.ToggleGroup(this);
        else
            SetStateAndStartAnimation(!CurrentValue);
    }

    public void Toggle(bool value)
    {
        if (CurrentValue == value) return;

        Toggle();
    }

    public void ToggleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo);
    }


    private void SetStateAndStartAnimation(bool state)
    {
        _previousValue = CurrentValue;
        CurrentValue = state;

        if (_previousValue != CurrentValue)
        {
            onValueChanged?.Invoke(CurrentValue);
        }

        if (_animateSliderCoroutine != null)
            StopCoroutine(_animateSliderCoroutine);

        _animateSliderCoroutine = StartCoroutine(AnimateSlider());
    }


    private IEnumerator AnimateSlider()
    {
        float startValue = _slider.value;
        float endValue = CurrentValue ? 1 : 0;

        float time = 0;

        if(_text != null)
        {
            _text.text = CurrentValue ? _toggleOnText : _toggleOffText;
            ToggleRectPositionLeftRight(_text.rectTransform);
            _text.alpha = 0;
        }

        if (_animationDuration > 0)
        {
            while (time < _animationDuration)
            {
                time += Time.deltaTime;

                float lerpFactor = _slideEase.Evaluate(time / _animationDuration);
                _slider.value = _sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                _transitionEffect?.Invoke();

                yield return null;
            }
        }
        _text.alpha = 1;

        _slider.value = endValue;
    }

    public void ToggleRectPositionLeftRight(RectTransform rect)
    {
        rect.offsetMin = new Vector2(-rect.offsetMin.x, rect.offsetMin.y);
        rect.offsetMax = new Vector2(-rect.offsetMax.x, rect.offsetMax.y);
    }
}