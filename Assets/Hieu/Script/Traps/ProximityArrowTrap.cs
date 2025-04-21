using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityArrowTrap : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireSpeed = 10f;
    public float fireDistance = 5f;
    public float fireDamage = 1f;

    private bool hasFired = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasFired && collision.CompareTag("Player"))
        {
            FireProjectile();
            hasFired = true;
        }
    }

    private void FireProjectile()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = proj.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.FireProjectile(fireSpeed, fireDistance, fireDamage);
        }
    }

    private void OnDrawGizmos()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.right * fireDistance);
        }
    }
}
