using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatform : MonoBehaviour
{
    [SerializeField] private float riseHeight = 2f;
    [SerializeField] private float riseSpeed = 2f;
    [SerializeField] private float returnSpeed = 1f;
    [SerializeField] private float delayBeforeRise = 0.5f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool playerOnPlatform = false;
    private bool isRising = false;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * riseHeight;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isRising)
        {
            playerOnPlatform = true;
            StartCoroutine(RiseAfterDelay());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }

    private IEnumerator RiseAfterDelay()
    {
        isRising = true;
        yield return new WaitForSeconds(delayBeforeRise);

        while (playerOnPlatform && Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, riseSpeed * Time.deltaTime);
            yield return null;
        }

        isRising = false;
    }

    private void Update()
    {
        // If player left, return to original position
        if (!playerOnPlatform && Vector3.Distance(transform.position, initialPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, returnSpeed * Time.deltaTime);
        }
    }
}
