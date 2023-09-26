using System.Collections;
using System.Collections.Generic;
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
        if (collidedObject.CompareTag("LeftBorder") || collidedObject.CompareTag("RightBorder"))
        {
            ReflectOnBorder(collidedObject);
        }
        else if (collidedObject.CompareTag("Player"))
        {
            ReflectOnPlayer(collidedObject);
        }
        else if (collidedObject.CompareTag("Alien") && isReflected)
        {
            DestroyAlien(collidedObject);
        }
    }

    private void ReflectOnBorder(GameObject collidedObject)
    {
        // Calculate the reflection angle based on which border was hit
        float reflectionAngle = 0f;

        if (collidedObject.CompareTag("LeftBorder"))
        {
            reflectionAngle = transform.rotation.eulerAngles.z - 90.0f;
        }
        else if (collidedObject.CompareTag("RightBorder"))
        {
            reflectionAngle = transform.rotation.eulerAngles.z - 270.0f;
        }

        // Apply the reflection angle to the projectile
        transform.rotation = Quaternion.Euler(0, 0, reflectionAngle);

        isReflected = true;
    }


    private void ReflectOnPlayer(GameObject collidedObject)
    {
        Rigidbody2D collidedObjectRigidBody = collidedObject.GetComponent<Rigidbody2D>();
        PlayerController playerController = collidedObject.GetComponent<PlayerController>();

        if (collidedObjectRigidBody == null || playerController == null)
        {
            return;
        }

        float playerVelocityX = playerController.GetCurrentSpeed();
        float playerMaxMoveSpeed = playerController.GetMoveSpeed();
        Debug.Log("Velocity: " + playerVelocityX);

        // Calculate the reflected angle based on the velocity components
        float reflectedAngle = 180.0f - Mathf.Atan2(playerVelocityX, playerMaxMoveSpeed) * Mathf.Rad2Deg;
        Debug.Log("Reflected Angle: " + reflectedAngle);
        
        Quaternion reflectedRotation = Quaternion.Euler(0, 0, reflectedAngle);
        transform.rotation = reflectedRotation;

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
