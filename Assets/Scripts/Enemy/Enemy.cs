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
    protected bool isHit = false;

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

        if (!isHit)
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        float distance = Vector3.Distance(transform.localPosition, player.transform.localPosition);
        if (distance > 2.0f)
        {
            isHit = false;
            animator.SetBool("InCombat", false);
        }
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
