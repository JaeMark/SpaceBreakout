using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    private void Start()
    {
        // Schedule the shooting every 5 seconds
        InvokeRepeating("TryShoot", 0f, 1f);
    }

    private void TryShoot()
    {
        // Generate a random number between 0 and 1
        float randomProbability = Random.Range(0f, 1f);

        if (randomProbability <= 0.1f)
        {
            Vector2 raycastOrigin = transform.position + Vector3.down * 1.0f; // Adjusted origin point
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 10.0f, LayerMask.GetMask("Alien"));

            if (hit.collider == null)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        // Instantiate a projectile and set its position and direction
        GameObject projectile = Instantiate(projectilePrefab, transform.position + Vector3.down * 1.0f, Quaternion.identity);
    }

    public void DestroyAlien()
    {
        AlienSwarmManager.Instance.OnDestroyEvent();
        Destroy(gameObject);
    }
}
