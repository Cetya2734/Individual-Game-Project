using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGravity : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool top;
    private PlayerController player;

    [SerializeField] private float flipCooldown = 0.5f;
    private float lastFlipTime = -Mathf.Infinity;
    [SerializeField] private KeyCode gravitySwitchKey = KeyCode.R;

    [Header("Gravity Switch Control")]
    [SerializeField] private bool canSwitchGravity = false;
    [SerializeField] private GameObject wallToActivate; // Wall to enable when inside the trigger

    void Start()
    {
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (canSwitchGravity && Input.GetKeyDown(gravitySwitchKey) && Time.time - lastFlipTime > flipCooldown)
        {
            rb.gravityScale *= -1;
            Rotation();
            SwitchScale();
            lastFlipTime = Time.time;
        }
    }

    void Rotation()
    {
        if (!top)
            transform.eulerAngles = new Vector3(0, 0, 180f);
        else
            transform.eulerAngles = Vector3.zero;

        top = !top;
    }

    void SwitchScale()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = currentScale.x > 0 ? -2f : 2f;
        transform.localScale = currentScale;
    }

    public void ActivateGravityZone(Collider2D gravityZoneCollider)
    {
        if (gravityZoneCollider.CompareTag("GravityZone"))
        {
            canSwitchGravity = true;

            if (wallToActivate != null)
                wallToActivate.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GravityZone"))
        {
            canSwitchGravity = true;

            if (wallToActivate != null)
                wallToActivate.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GravityZone"))
        {
            canSwitchGravity = false;

            if (wallToActivate != null)
                wallToActivate.SetActive(false);
        }
    }
}
