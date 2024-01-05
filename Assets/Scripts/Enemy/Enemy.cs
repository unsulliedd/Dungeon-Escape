using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected int health;
    [SerializeField] protected int speed;
    [SerializeField] protected int gems;
    protected bool isDead = false;

    [Header("Enemy Waypoint")]
    [SerializeField] protected Transform waypoint1;
    [SerializeField] protected Transform waypoint2;
    protected Vector3 currentTarget;
    protected float distance;
    protected float gapBetweenPlayer = 1.5f;

    [Header("Enemy Animation")]
    protected bool idleInitialState = true;

    [Header("Enemy References")]
    protected Animator animator;
    protected Player player;
    protected bool playerAlive;
    [SerializeField] protected GameObject _diamondPrefab;

    public virtual void Init()
    {
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        Init();
        currentTarget = waypoint1.position;
    }

    public virtual void Update()
    {
        playerAlive = player.IsPlayerAlive();
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

        if (distanceX < 0)
            gapBetweenPlayer *= -1;

        Vector2 targetPlayerDestination = new(player.transform.position.x + gapBetweenPlayer, transform.position.y);

        if (distance > 6f || !playerAlive)
        {            
            if (transform.position == waypoint1.position)
            {
                if (!idleInitialState)
                    animator.SetTrigger("Idle");
                currentTarget = waypoint2.position;
            }
            else if (transform.position == waypoint2.position)
            {
                animator.SetTrigger("Idle");
                currentTarget = waypoint1.position;
                idleInitialState = false;
            }

            animator.SetBool("InCombat", false);
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        }
        if (distance < 6f && playerAlive)
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
        Debug.DrawLine(transform.position, currentTarget, Color.green);
    }

    public virtual void Flip()
    {
        float direction = player.transform.position.x - transform.position.x;

        if (distance > 6f || !playerAlive)
        {
            if (currentTarget == waypoint1.position)
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            else
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (distance < 6f && playerAlive)
        {
            if (direction > 0)
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            else if (direction < 0)
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}
