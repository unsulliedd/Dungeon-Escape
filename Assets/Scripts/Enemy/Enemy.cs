using UnityEngine;

// This abstract class represents the base functionality for enemy characters in the game.
public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected int health;                      // The health points of the enemy.
    [SerializeField] protected int speed;                       // The movement speed of the enemy.
    [SerializeField] protected int gems;                        // The amount of gems rewarded upon defeating the enemy.
    protected bool isDead = false;                              // Flag indicating if the enemy is dead.

    [Header("Enemy Waypoint")]
    [SerializeField] protected Transform waypointStart;         // The starting waypoint for enemy movement.
    [SerializeField] protected Transform waypointEnd;           // The ending waypoint for enemy movement.
    [SerializeField] protected bool isLookLeft;                 // Flag indicating initial facing direction.
    protected Vector3 waypoint1;                                // Position of the first waypoint.
    protected Vector3 waypoint2;                                // Position of the second waypoint.
    protected Vector3 currentTarget;                            // The current target waypoint for movement.
    [SerializeField] protected float minAttackDistancce = 3f;   // The distance at which the enemy can attack the player.
    [SerializeField] protected float minDistancetoPlayer = 6f;  // The minimum distance between the enemy and the player.
    private float distance;                                     // Distance between the enemy and the player.
    private float dirDistance;                                  // Directional horizontal distance between the enemy and the player.
    protected float gapBetweenPlayer = 1.5f;                    // Gap between the enemy and the player.

    [Header("Enemy Animation")]
    protected bool idleInitialState = true;                     // Initial state flag for the idle animation.

    [Header("Enemy References")]
    protected Animator animator;                                // Reference to the enemy's animator component.
    protected Player player;                                    // Reference to the player character.
    protected bool playerAlive;                                 // Flag indicating if the player is alive.
    [SerializeField] protected GameObject _diamondPrefab;       // Reference to the diamond prefab for drops.

    void Awake()
    {
        // Retrieve the animator component and player reference.
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogError("Animator is NULL");
        if (!GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player))
            Debug.LogError("Player is NULL");

        // Initialize the enemy.
        Init();
    }

    // Initializes the enemy with waypoint positions and initial states.
    public virtual void Init()
    {
        waypoint1 = waypointStart.position;
        waypoint2 = waypointEnd.position;

        waypointStart.gameObject.SetActive(false);
        waypointEnd.gameObject.SetActive(false);

        currentTarget = waypoint1;

        waypoint1.y = transform.position.y;
        waypoint2.y = transform.position.y;
    }

    void Update()
    {
        // Check if the player is alive.
        playerAlive = player.IsPlayerAlive();

        // Update player status and handle movement if not in idle or combat animation.
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.GetBool("InCombat"))
            return;

        // Calculate the distance between the enemy and the player.
        distance = CalculateDistanceToPlayer().distance;
        dirDistance = CalculateDistanceToPlayer().dirDistance;

        if (!isDead)
        {
            Flip(dirDistance, distance);
            Movement(dirDistance, distance);
        }
    }

    // Handles the enemy movement logic.
    private void Movement(float dirDistance,float distance)
    {
        // If the player is not within a certain distance, move between waypoints.
        if (distance > 6f || !playerAlive || !IsBetweenWaypoints())
        {
            // Move towards the target waypoint based on the current position.
            if (Vector3.SqrMagnitude(transform.position - waypoint1) < 0.05)
            {
                if (!idleInitialState)
                    animator.SetTrigger("Idle");
                currentTarget = waypoint2;
            }
            else if (Vector3.SqrMagnitude(transform.position - waypoint2) < 0.05)
            {
                animator.SetTrigger("Idle");
                currentTarget = waypoint1;
                idleInitialState = false;
            }

            animator.SetBool("InCombat", false);
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        }

        // If the player is within a certain distance, move towards the player.
        if (distance < minDistancetoPlayer && playerAlive && IsBetweenWaypoints())
        {
            // Adjust the gap between the player and the enemy based on their positions.
            if (dirDistance > 0)
                gapBetweenPlayer *= -1;

            // If the player is within a certain distance, update the enemy's position to be close to the player with a specified gap.
            Vector2 targetPlayerDestination = new(player.transform.position.x + gapBetweenPlayer, transform.position.y);

            if (distance < minAttackDistancce)
            {
                animator.SetBool("InCombat", true);
                animator.SetTrigger("Idle");
            }
            else
                animator.SetBool("InCombat", false);

            // Move towards the player within the specified distance.
            if (distance > minAttackDistancce/2)
                transform.position = Vector2.MoveTowards(transform.position, targetPlayerDestination, speed * Time.deltaTime);
        }
    }

    // Flips the enemy's facing direction based on the player's position.
    private void Flip(float dirDistance, float distance)
    {
        if (distance > minDistancetoPlayer || !playerAlive || !IsBetweenWaypoints())
        {
            // Flip the enemy based on the facing direction and initial look direction.
            if (isLookLeft)
            {
                if (currentTarget == waypoint1)
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                else
                    transform.rotation = Quaternion.Euler(0f, -180f, 0f);
            }
            else
            {
                if (currentTarget == waypoint1)
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                else
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else if (distance < minDistancetoPlayer && playerAlive && IsBetweenWaypoints())
        {
            // Flip the enemy based on the direction to the player.
            if (dirDistance > 0)
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            else if (dirDistance < 0)
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    // Checks if the player is between the waypoints of the enemy.
    private bool IsBetweenWaypoints()
    {
        // Check if the dot product of the normalized vectors is less than 0,
        // indicating that the player is between waypoint1 and waypoint2 in both cases.
        return Vector3.Dot((waypoint2 - waypoint1).normalized, (player.transform.position - waypoint2).normalized) < 0f
            && Vector3.Dot((waypoint1 - waypoint2).normalized, (player.transform.position - waypoint1).normalized) < 0f;
    }

    // Calculates the distance between the enemy and the player.
    private (float distance, float dirDistance) CalculateDistanceToPlayer()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        dirDistance = player.transform.position.x - transform.position.x;
        return (distance, dirDistance);
    }
}