public interface ILoadingScreen
{
    float Progress { get; set;}
    void SetActive(bool active);
}