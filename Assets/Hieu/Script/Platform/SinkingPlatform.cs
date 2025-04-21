using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour
{
    [Header("Sinking Settings")]
    [SerializeField] private float sinkDistance = 0.5f;
    [SerializeField] private float sinkSpeed = 2f;
    [SerializeField] private float returnSpeed = 1f;
    [SerializeField] private float sinkDelay = 0.5f;

    private Vector3 startPosition;
    private Vector3 sinkPosition;

    private Coroutine sinkCoroutine;
    private bool isSinking = false;
    private bool playerOnPlatform = false;

    private void Start()
    {
        startPosition = transform.position;
        sinkPosition = startPosition - new Vector3(0f, sinkDistance, 0f);
    }

    private void Update()
    {
        if (isSinking)
        {
            transform.position = Vector3.MoveTowards(transform.position, sinkPosition, sinkSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, returnSpeed * Time.deltaTime);
        }
    }

    // Attach this to the trigger child object as a separate collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !playerOnPlatform)
        {
            playerOnPlatform = true;
            if (sinkCoroutine == null)
                sinkCoroutine = StartCoroutine(DelayedSink());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = false;
            isSinking = false;

            if (sinkCoroutine != null)
            {
                StopCoroutine(sinkCoroutine);
                sinkCoroutine = null;
            }
        }
    }

    private IEnumerator DelayedSink()
    {
        yield return new WaitForSeconds(sinkDelay);
        if (playerOnPlatform) // Check again in case they left
            isSinking = true;

        sinkCoroutine = null;
    }
}
