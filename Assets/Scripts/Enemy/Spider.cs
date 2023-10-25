using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy, IDamageable
{
    [SerializeField] private GameObject _acidPrefab;
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
            animator.SetBool("InCombat", true);
            if (Health < 1)
            {
                isDead = true;
                animator.SetTrigger("Death");
                GameObject diamonds = Instantiate(_diamondPrefab, transform.position, Quaternion.identity);
                diamonds.GetComponent<Diamond>().diamondValue = base.gems;
                Destroy(this.gameObject, 10f);
            }
        }
    }

    public void Attack()
    {
        Instantiate(_acidPrefab, transform.position, Quaternion.identity);
    }
}
