using UnityEngine;

// Skeleton class inherits from Enemy and implements IDamageable interface
public class Skeleton : Enemy, IDamageable
{
    public int Health { get; set; }     // Property to get and set health implementing IDamageable

    // Initialize method overrides base class Init method
    public override void Init()
    {
        base.Init();            // Call base class Init method
        Health = base.health;   // Set Health to the base class health value
    }

    // Damage method from IDamageable interface
    public void Damage()
    {
        if (!isDead)
        {
            Health--;
            animator.SetTrigger("Hit");
            AudioManager.Instance.PlaySkeletonSounds(0, transform.position);
            animator.SetBool("InCombat", true);

            if (Health < 1)
            {
                isDead = true;
                animator.SetTrigger("Death");
                
                GameManager.Instance.enemiesKilled++;   // Increase the count of killed enemies in GameManager
                AudioManager.Instance.PlaySkeletonSounds(1, transform.position);    // Play Skeleton death sound

                // Instantiate diamonds at the enemy's position
                GameObject diamonds = Instantiate(_diamondPrefab, transform.position, Quaternion.identity);
                // Set diamondValue to the base class gems value
                diamonds.GetComponent<Diamond>().diamondValue = base.gems;
                // Destroy the enemy object after 10 seconds
                Destroy(this.gameObject, 10f);
            }
        }
    }
}
