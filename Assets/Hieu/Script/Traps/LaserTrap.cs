using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;
    public float damage = 1f;

    private Vector3 worldPointA;
    private Vector3 worldPointB;
    private Vector3 target;

    private void Start()
    {
        // Save the world positions once at start
        worldPointA = pointA.position;
        worldPointB = pointB.position;
        target = worldPointB;
    }

    private void Update()
    {
        MoveLaser();
    }

    void MoveLaser()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            target = (target == worldPointA) ? worldPointB : worldPointA;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.DecreaseHealth(damage);

                PlayerController playerController = collision.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    int direction = (collision.transform.position.x < transform.position.x) ? -1 : 1;
                    playerController.Knockback(direction);
                }
            }
        }
    }
}
