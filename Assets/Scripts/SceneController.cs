using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Slider _loadingBar;


    private float progress;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _loadingScreen.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithLoadingScreen(sceneName));
    }

    IEnumerator LoadSceneWithLoadingScreen(string sceneName)
    {
        // Activate loading screen
        _loadingScreen.SetActive(true);
        _loadingBar.value = 0;

        // Load the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / .9f);
            _loadingBar.value = progress;

            yield return null;
        }

        if(_loadingScreen)
        {
            // Deactivate loading screen
            _loadingScreen.SetActive(false);
        }
    }
}
