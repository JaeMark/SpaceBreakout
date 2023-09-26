using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckLoseCondition : MonoBehaviour
{
    [SerializeField] private GameObject loseUI;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().enabled = false; // Hide mesh
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Alien"))
        {
            // Trigger lose condition
            loseUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
