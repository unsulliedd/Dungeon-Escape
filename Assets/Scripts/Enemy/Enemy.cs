using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected int health;
    [SerializeField] protected int speed;
    [SerializeField] protected int gems;
    protected bool isDead = false;

    [Header("Enemy Waypoint")]
    [SerializeField] protected Transform waypointA;
    [SerializeField] protected Transform waypointB;
    protected Vector3 currentTarget;
    private float distance;

    [Header("Enemy Animation")]
    protected bool idleInitialState = true;

    [Header("Enemy References")]
    protected Animator animator;
    protected Player player;
    [SerializeField] protected GameObject _diamondPrefab;

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
            return;
        if (isDead == false)
        {
            Flip();
            Movement();
        }
    }

    public virtual void Movement()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        float distanceX = transform.position.x - player.transform.position.x;
        float gap = 1.5f;

        if (distanceX < 0) 
        { 
            gap *= -1;
        }
        Vector2 targetPlayerDestination = new(player.transform.position.x + gap, transform.position.y);

        if (distance > 10f || !player.IsPlayerAlive())
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
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        }
        else if (distance < 10f && player.IsPlayerAlive())
        {
            if (distance < 3f)
            {
                animator.SetBool("InCombat", true);
                animator.SetTrigger("Idle");
            }
            else
                animator.SetBool("InCombat", false);
            transform.position = Vector2.MoveTowards(transform.position, targetPlayerDestination, speed * Time.deltaTime);
        }
    }

    public virtual void Flip()
    {
        float direction = player.transform.position.x - transform.position.x;

        if (distance > 10f)
        {
            if (currentTarget == waypointA.position)
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            else
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (distance < 10f)
        {
            if (direction > 0)
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            else if (direction < 0)
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}
