using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    [SerializeField] private ILoadingScreen _loadingScreen;
    


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
        SceneManager.sceneLoaded += SceneLoaded;
        FindLoadingScreen();
    }   

    private void FindLoadingScreen()
    {
        _loadingScreen = FindObjectsByType<LoadingScreen>(FindObjectsInactive.Include, FindObjectsSortMode.None).FirstOrDefault();
        _loadingScreen?.SetActive(false);
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(mode == LoadSceneMode.Single)
        {
            FindLoadingScreen();
        }
    }

    public void LoadScene(string sceneName)
    {
        if(_loadingScreen == null) {
            SceneManager.LoadScene(sceneName);
            return;
        }
        StartCoroutine(LoadSceneWithLoadingScreen(sceneName));
    }

    IEnumerator LoadSceneWithLoadingScreen(string sceneName)
    {
        // Activate loading screen
        _loadingScreen.SetActive(true);
        _loadingScreen.Progress = progress;

        // Load the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / .9f);
            _loadingScreen.Progress = progress;

            yield return null;
        }
    }
}
