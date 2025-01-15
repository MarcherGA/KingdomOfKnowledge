using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour, ILoadingScreen
{
    [SerializeField] private Slider _loadingBar;

    public float Progress { get => _loadingBar.value; set => _loadingBar.value = value; }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
