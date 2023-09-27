using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

public class AlienSwarmManager : MonoBehaviour
{
    [SerializeField] private GameObject[] alienShips;
    [SerializeField] private float horizontalSpeed = 0.1f;
    [SerializeField] private float descentSpeed = 0.5f;
    [SerializeField] private float speedIncreasePerDestroyedAlien = 0.1f;
    [SerializeField] private float shootInterval = 1f;

    private bool isMovingRight = true;
    private int numDestroyed = 0;
    private Coroutine swarmMovementCoroutine;
    private Coroutine commandToShootCoroutine;
    private List<GameObject> alienGunners = new List<GameObject>();
    private List<GameObject> remainingAliens = new List<GameObject>();


    public static AlienSwarmManager Instance; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        swarmMovementCoroutine = StartCoroutine(MoveSwarmHorizontally());
        commandToShootCoroutine = StartCoroutine(CommandToShoot());
        foreach (GameObject alien in alienShips)
        {
            remainingAliens.Add(alien);
        }

    }

    private IEnumerator MoveSwarmHorizontally()
    {
        while (true)
        {
            float movement = isMovingRight ? horizontalSpeed : -horizontalSpeed;
            transform.Translate(Vector3.right * movement * Time.deltaTime);
            yield return null;
        }
    }

    private void MoveSwarmDown()
    {
        transform.Translate(Vector3.down * descentSpeed);
    }

    public void ChangeDirection()
    {
        StopCoroutine(swarmMovementCoroutine);

        isMovingRight = !isMovingRight;
        MoveSwarmDown();
        swarmMovementCoroutine = StartCoroutine(MoveSwarmHorizontally());
    }

    public void OnDestroyEvent(GameObject alienToDestroy)
    {
        if (commandToShootCoroutine != null)
        {
            StopCoroutine(commandToShootCoroutine);
        }

        numDestroyed++;
        if (remainingAliens.Contains(alienToDestroy))
        {
            remainingAliens.Remove(alienToDestroy);
        }
        if (IsSwarmDestroyed())
        {
            // Trigger win state
            GameStateHandler.Instance.TriggerWinState();
        }
        else
        {
            horizontalSpeed += speedIncreasePerDestroyedAlien;
        }

        Destroy(alienToDestroy);

        // Remove from gunner list if applicable
        if (alienGunners.Contains(alienToDestroy))
        {
            alienGunners.Remove(alienToDestroy);
            UpdateGunnerList();
        }
        commandToShootCoroutine = StartCoroutine(CommandToShoot());
    }

    private bool IsSwarmDestroyed()
    {
        return numDestroyed == alienShips.Length;
    }

    public void SetAsGunner(GameObject gunner)
    {
        if (!alienGunners.Contains(gunner))
        {
            alienGunners.Add(gunner);
        }
    }

    private void UpdateGunnerList()
    {
        foreach (GameObject alien in remainingAliens)
        {
            if (alien != null)
            {
                if (alien.GetComponent<AlienController>().CheckIfGunner())
                {
                    SetAsGunner(alien);
                }
            }
        }
    }

    private IEnumerator CommandToShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);

            // Check if there are any gunners available
            if (alienGunners.Count > 0)
            {
                // Select a random gunner, and tell it to shoot
                int randomIndex = Random.Range(0, alienGunners.Count);
                // GameObject selectedGunner = alienGunners[randomIndex];
                Debug.Log("Shooting");
                alienGunners[randomIndex].GetComponent<AlienController>().Shoot(); 
            }
        }
    }

}
