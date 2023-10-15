using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy, IDamageable
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
        Health--;
        animator.SetTrigger("Hit");
        animator.SetBool("InCombat", true);
        if (Health < 1)
        {
            isDead = true;
            animator.SetTrigger("Death");
            Destroy(this.gameObject, 10f);
        }
    }
}
