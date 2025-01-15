using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Set the checkpoint when the player enters the triggers
        CheckpointManager.Instance.SetCheckpoint(transform.position);
    }
}
