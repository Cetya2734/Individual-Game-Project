using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
    [SerializeField] private GameObject gravityPromptUI_Enter;
    [SerializeField] private GameObject gravityPromptUI_Exit;

    private void Start()
    {
        if (gravityPromptUI_Enter) gravityPromptUI_Enter.SetActive(false);
        if (gravityPromptUI_Exit) gravityPromptUI_Exit.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchGravity playerSwitch = other.GetComponent<SwitchGravity>();
            if (playerSwitch != null)
            {
                playerSwitch.SetCanSwitchGravity(true);
            }

            if (gravityPromptUI_Enter) gravityPromptUI_Enter.SetActive(true);
            if (gravityPromptUI_Exit) gravityPromptUI_Exit.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchGravity playerSwitch = other.GetComponent<SwitchGravity>();
            if (playerSwitch != null)
            {
                playerSwitch.SetCanSwitchGravity(false);
            }

            if (gravityPromptUI_Enter) gravityPromptUI_Enter.SetActive(false);
            if (gravityPromptUI_Exit) gravityPromptUI_Exit.SetActive(false);
        }
    }
}
