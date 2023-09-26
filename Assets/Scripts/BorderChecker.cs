using UnityEngine;

public class BorderChecker : MonoBehaviour
{
    private bool eventTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Alien") && !eventTriggered)
        {
            eventTriggered = true;
            AlienSwarmManager.Instance.ChangeDirection();
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
