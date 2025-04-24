using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] private GameObject wallToActivate;

    [Header("UI Prompts")]
    [SerializeField] private GameObject gravityPromptUI_Enter;
    [SerializeField] private GameObject gravityPromptUI_Exit;

    void Start()
    {
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        // Ensure prompts start off
        if (gravityPromptUI_Enter) gravityPromptUI_Enter.SetActive(false);
        if (gravityPromptUI_Exit) gravityPromptUI_Exit.SetActive(false);
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
        transform.eulerAngles = top ? Vector3.zero : new Vector3(0, 0, 180f);
        top = !top;
    }

    void SwitchScale()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = currentScale.x > 0 ? -2f : 2f;
        transform.localScale = currentScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GravityZone"))
        {
            canSwitchGravity = true;

            if (wallToActivate != null)
                wallToActivate.SetActive(true);

            if (gravityPromptUI_Enter != null)
                gravityPromptUI_Enter.SetActive(true);

            if (gravityPromptUI_Exit != null)
                gravityPromptUI_Exit.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GravityZone"))
        {
            canSwitchGravity = false;

            if (wallToActivate != null)
                wallToActivate.SetActive(false);

            if (gravityPromptUI_Enter != null)
                gravityPromptUI_Enter.SetActive(false);

            if (gravityPromptUI_Exit != null)
                gravityPromptUI_Exit.SetActive(false);
        }
    }
}
