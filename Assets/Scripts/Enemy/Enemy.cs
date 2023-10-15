using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected int health;
    [SerializeField] protected int speed;
    [SerializeField] protected int gems;

    [Header("Enemy Waypoint")]
    [SerializeField] protected Transform waypointA;
    [SerializeField] protected Transform waypointB;
    protected Vector3 currentTarget;

    [Header("Enemy Animation")]
    protected bool idleInitialState = true;

    [Header("Enemy References")]
    protected Animator animator;
    protected Player player;

    public virtual void Init()
    {
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        Init();
    }

    public virtual void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && animator.GetBool("InCombat") == false)
        {
            return;
        }
        Flip();
        Movement();
    }

    public virtual void Movement()
    {
        float distance = Vector3.Distance(transform.position, player.transform.localPosition);

        if (distance > 3f)
        {            
            if (transform.position == waypointA.position)
            {
                currentTarget = waypointB.position;
                if (!idleInitialState)
                    animator.SetTrigger("Idle");
            }
            else if (transform.position == waypointB.position)
            {
                currentTarget = waypointA.position;
                idleInitialState = false;
                animator.SetTrigger("Idle");
            }

            animator.SetBool("InCombat", false);
            Vector3 targetPosition = new(currentTarget.x, transform.position.y, currentTarget.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else if (distance < 3f)
        {
            animator.SetBool("InCombat", true);
            Vector3 targetPlayerPosition = new(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPlayerPosition, speed * Time.deltaTime);
        }
    }

    public virtual void Flip()
    {
        Vector3 direction = player.transform.localPosition - transform.localPosition;

        if (animator.GetBool("InCombat") == false)
        {
            if (currentTarget == waypointA.position)
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            else
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (animator.GetBool("InCombat") == true)
        {
            if (direction.x > 0)
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            else if (direction.x < 0)
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}
