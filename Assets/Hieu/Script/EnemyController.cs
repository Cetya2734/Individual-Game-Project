using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float patrolRange = 5f;
    public float detectionRange = 8f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;

    private Transform player;
    private Vector2 initialPosition;
    private bool movingRight = true;
    private bool isChasing = false;
    private bool isAttacking = false;
    private float attackCooldownTimer = 0f;

    private Rigidbody2D rb;
    //private Animator anim;

    public LayerMask playerLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        initialPosition = transform.position;

        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Reduce attack cooldown over time
        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;

        // Determine behavior based on state
        if (isAttacking)
        {
            rb.velocity = Vector2.zero; // Stop movement during attack
        }
        else if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        // Check for state transitions
        DetectPlayer();
    }

    private void Patrol()
    {
        //anim.SetBool("isChasing", false);
        //anim.SetBool("isAttacking", false);

        float distanceFromInitial = transform.position.x - initialPosition.x;

        // Move in the current direction
        if (movingRight)
        {
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);

            // Check if it exceeds the patrol range
            if (transform.position.x > initialPosition.x + patrolRange)
            {
                Flip();
            }
        }
        else
        {
            rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);

            if (transform.position.x < initialPosition.x - patrolRange)
            {
                Flip();
            }
        }

        //anim.SetBool("isPatrolling", true);
    }

    private void DetectPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            isChasing = false;
            isAttacking = true;
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            isAttacking = false;
        }
        else
        {
            isChasing = false;
            isAttacking = false;
        }
    }

    private void ChasePlayer()
    {
        //anim.SetBool("isPatrolling", false);
        //anim.SetBool("isAttacking", false);
        //anim.SetBool("isChasing", true);

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);

        // Flip enemy based on movement direction
        if ((direction.x > 0 && !movingRight) || (direction.x < 0 && movingRight))
        {
            Flip();
        }
    }

    private void AttackPlayer()
    {
        if (attackCooldownTimer <= 0)
        {
            //anim.SetTrigger("Attack");
            attackCooldownTimer = attackCooldown;
            // Add attack logic here (e.g., deal damage to the player)
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        // Visualize patrol range
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(initialPosition + Vector2.left * patrolRange, initialPosition + Vector2.right * patrolRange);

        // Visualize detection and attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
