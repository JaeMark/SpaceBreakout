using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    [SerializeField] private AlienSwarmMovementController alienSwarmMovementController;
    [SerializeField] private AlienSwarmGunnerManager alienSwarmGunnerManager;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private float rotationSpeed = 1f;

    private bool isGunner = false;
    private void Start()
    {
        // Set a gunner 
        if (CheckIfGunner())
        {
            alienSwarmGunnerManager.SetAsGunner(gameObject);
        }
    }

    private void Update()
    {
        if (isGunner)
        {
            TrackPlayerShip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * 1f);
    }

    public bool CheckIfGunner()
    {
        // The alien ship is a gunner if no alien ship is below it
        Vector2 raycastOrigin = transform.position + Vector3.down * 1.0f; // Adjusted origin point
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 10.0f, LayerMask.GetMask("Alien"));
        isGunner = hit.collider == null;
        return isGunner;
    }

    public void Shoot()
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
        alienSwarmGunnerManager.OnDestroyEvent(gameObject);
        alienSwarmMovementController.OnDestroyEvent(gameObject);
        Destroy(gameObject);
    }
}
