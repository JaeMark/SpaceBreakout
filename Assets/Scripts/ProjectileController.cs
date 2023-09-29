using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 0.5f;
    [SerializeField] private float projectileSpeedIncrement = 0.1f;
    [SerializeField] private float projectileLifetime = 10.0f;

    private bool isReflected = false;
    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(projectileLifetime)); // Destroy projectile after some time
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.down * projectileSpeed * Time.deltaTime);
        //transform.Translate(0, -projectileSpeed * Time.deltaTime, 0);
        projectileSpeed += projectileSpeedIncrement;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * projectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject collidedObject = other.gameObject;

        if (collidedObject.CompareTag("LeftBorder") || collidedObject.CompareTag("RightBorder") || collidedObject.CompareTag("Player"))
        {
            Reflect(collidedObject);
        } 
        else if (collidedObject.CompareTag("Alien") && isReflected)
        {
            DestroyAlien(collidedObject);
        }
    }

    private void Reflect(GameObject contactObject)
    {
        // Get the normal vector of the contact surface
        Vector2 surfaceNormal = contactObject.transform.up;
        float currentAngle = transform.rotation.eulerAngles.z;
        if (surfaceNormal == Vector2.up || surfaceNormal == -Vector2.up)
        {
            // Surface normal is (0, 1) or (0, -1)
            // Apply the reflection angle along the Y-axis
            float newAngle = 180f - currentAngle;
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }
        else if (surfaceNormal == Vector2.right || surfaceNormal == -Vector2.right)
        {
            // Surface normal is (1, 0) or (-1, 0)
            // Apply the reflection angle along the X-axis
            float newAngle = -currentAngle;
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }

        isReflected = true;
    }

    private void DestroyAlien(GameObject collidedObject)
    {
        AlienController alienController = collidedObject.GetComponent<AlienController>();
        if (alienController != null)
        {
            alienController.DestroyAlien();
        }
        Destroy(gameObject);
    }
}
