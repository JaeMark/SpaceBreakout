using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private float rotationSpeed = 1f;

    private bool isShooter = false;
    private void Start()
    {
        // Schedule the shooting every 5 seconds
        InvokeRepeating("TryShoot", 0f, 1f);
    }

    private void Update()
    {
        if (isShooter)
        {
            TrackPlayerShip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * 1f);
    }

    private void TryShoot()
    {
        if (!isShooter)
        {
            Vector2 raycastOrigin = transform.position + Vector3.down * 1.0f; // Adjusted origin point
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 10.0f, LayerMask.GetMask("Alien"));
            if (hit.collider == null)
            {
                isShooter = true;
            }
        }

        float randomProbability = Random.Range(0f, 1f);
        if (randomProbability <= 0.1f && isShooter)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // Instantiate a projectile and set its position and direction
        GameObject projectile = Instantiate(projectilePrefab, muzzle.transform.position, transform.rotation);
    }

    private void TrackPlayerShip()
    {
        if (playerShip != null)
        {
            Vector3 targetDirection = playerShip.transform.position - transform.position;

            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void DestroyAlien()
    {
        AlienSwarmManager.Instance.OnDestroyEvent();
        Destroy(gameObject);
    }
}
