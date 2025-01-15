using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerEventsTrigger2D : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Pointer Events")]
    public UnityEvent onClick;
    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit.Invoke();
    }
}
