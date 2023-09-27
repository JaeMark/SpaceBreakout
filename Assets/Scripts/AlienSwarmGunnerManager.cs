using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSwarmGunnerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] alienShips;
    [SerializeField] private float shootInterval = 1f;

    private List<GameObject> alienGunners = new List<GameObject>();
    private List<GameObject> remainingAliens = new List<GameObject>();

    private int numDestroyed = 0;
    public void Start()
    {
        foreach (GameObject alien in alienShips)
        {
            remainingAliens.Add(alien);
        }
    }

    public void OnDestroyEvent(GameObject alienToDestroy)
    {
        numDestroyed++;

        if (remainingAliens.Contains(alienToDestroy))
        {
            remainingAliens.Remove(alienToDestroy);
        }

        if (alienGunners.Contains(alienToDestroy))
        {
            alienGunners.Remove(alienToDestroy);
            UpdateGunnerList();
        }

        StartCoroutine(CommandToShoot());
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

            if (alienGunners.Count > 0)
            {
                int randomIndex = Random.Range(0, alienGunners.Count);
                Debug.Log("Shooting");
                alienGunners[randomIndex].GetComponent<AlienController>().Shoot();
            }
        }
    }
}