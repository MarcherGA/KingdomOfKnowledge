using UnityEngine;

public interface IInteractableSurface
{
    /// <summary>
    /// Determines if the player can move on this surface.
    /// </summary>
    bool CanMoveOnSurface{get;}

    /// <summary>
    /// Determines if the player can jump off this surface.
    /// </summary>
    bool CanJumpOffSurface{get;}

    /// <summary>
    /// Optional: Provides a custom velocity or force to apply when jumping off this surface.
    /// </summary>
    /// <returns>The velocity to apply, or Vector3.zero if no custom velocity is needed.</returns>
    float JumpForce{get;}

    // /// <summary>
    // /// Optional: Handles any custom interaction logic when the player collides with the surface.
    // /// </summary>
    // /// <param name="player">The player GameObject interacting with the surface.</param>
    // void OnPlayerInteract(GameObject player);
}
