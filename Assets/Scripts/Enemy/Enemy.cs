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

    public virtual void Init()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Init();
    }

    public virtual void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            return;
        }
        Flip();
        Movement();
    }

    public virtual void Movement()
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
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
    }

    public virtual void Flip()
    {
        if (currentTarget == waypointA.position)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
