using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 1f;
    private void Start()
    {
        // Ensure the checkpoint manager exists
        if (CheckpointManager.Instance == null)
        {
            Debug.LogError("CheckpointManager not assigned.");
        }
    }

    public void RespawnAtCheckpoint()
    {
        // Optionally, you could play a death animation here or any other effects
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        // You can add animation or effects here before respawning
        yield return new WaitForSeconds(respawnDelay);  // Delay before respawn (add animation here)

        Vector3 checkpointPosition = CheckpointManager.Instance.GetLastCheckpointPosition();
        if (checkpointPosition != Vector3.zero)
        {
            transform.position = checkpointPosition;  // Move player to last checkpoint position
            Debug.Log("Player respawned at: " + checkpointPosition);
        }
    }
}
