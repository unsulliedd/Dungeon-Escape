using UnityEngine;

public abstract class TrapDamage : MonoBehaviour
{
    private Player player;  // Reference to the Player class.

    // This method is called when the script instance is being loaded.
    public virtual void Awake()
    {
        // Try to get the Player component from a GameObject with the "Player" tag.
        if (!GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player))
            // Log an error if the Player component is not found.
            Debug.LogError("Player component not found");
    }

    // This method is called when the Collider2D enters a trigger collider attached to this object.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider has the "Player" tag.
        if (other.CompareTag("Player"))
        {
            // Check if the player is alive before causing damage.
            if (player.IsPlayerAlive())
                // Call the InstantDeath method on the player.
                player.InstantDeath();
        }
    }
}
