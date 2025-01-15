using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    [SerializeField] private PlayerRespawn playerRespawn;  // Reference to the PlayerRespawn script
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Trigger respawn when the player enters the trigger zone
        playerRespawn.RespawnAtCheckpoint();
    }
}
