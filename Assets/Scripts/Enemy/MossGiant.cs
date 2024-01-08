using UnityEngine;

public class MossGiant : Enemy, IDamageable
{
    public int Health { get; set; }

    // Initilize
    public override void Init()
    {
        base.Init();
        Health = base.health;
    }

    public void Damage()
    {
        if (isDead == false)
        {
            Health--;
            animator.SetTrigger("Hit");
            AudioManager.Instance.PlayMossGiantSounds(0,transform.position);
            animator.SetBool("InCombat", true);
            if (Health < 1)
            {
                isDead = true;
                animator.SetTrigger("Death");
                AudioManager.Instance.PlayMossGiantSounds(1, transform.position);
                GameObject diamonds = Instantiate(_diamondPrefab, transform.position, Quaternion.identity);
                diamonds.GetComponent<Diamond>().diamondValue = base.gems;
                Destroy(this.gameObject, 10f);
            }
        }
    }
}

