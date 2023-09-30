using UnityEngine;
using UnityEngine.Events;

public class BorderChecker : MonoBehaviour
{
    [SerializeField] private AlienSwarmMovementController alienSwarmMovementController;

    private bool eventTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Alien") && !eventTriggered)
        {
            eventTriggered = true;
            // Change direction of swarm on contact
            alienSwarmMovementController.ChangeDirection();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Alien"))
        {
            eventTriggered = false; // Reset the flag when an alien exits the collider
        }
    }
}
