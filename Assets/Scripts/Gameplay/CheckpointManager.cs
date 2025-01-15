using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get { return _instance; } }
    private static CheckpointManager _instance;  // Singleton instance

    private Vector3 _lastCheckpointPosition;  // Stores the last checkpoint position
    private bool _isCheckpointSet = false;   // Flag to check if the checkpoint is set

    private void Awake()
    {
        // Ensure only one instance of CheckpointManager exists and it persists across scenes
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scene loads
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances if any
        }
    }

    // Set a new checkpoint
    public void SetCheckpoint(Vector3 newCheckpointPosition)
    {
        _lastCheckpointPosition = newCheckpointPosition;
        _isCheckpointSet = true;
        Debug.Log("Checkpoint reached at: " + newCheckpointPosition);
    }

    // Get the last checkpoint position
    public Vector3 GetLastCheckpointPosition()
    {
        if (_isCheckpointSet)
        {
            return _lastCheckpointPosition;
        }
        else
        {
            Debug.LogWarning("No checkpoint set! Please set a checkpoint first.");
            return Vector3.zero;  // Return a default value if no checkpoint is set
        }
    }
}
