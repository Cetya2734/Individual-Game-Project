using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 worldPointA;
    private Vector3 worldPointB;
    private Vector3 target;

    private void Start()
    {
        // Save the world positions at the beginning
        worldPointA = pointA.position;
        worldPointB = pointB.position;
        target = worldPointB;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            target = (target == worldPointA) ? worldPointB : worldPointA;
        }
    }
}
