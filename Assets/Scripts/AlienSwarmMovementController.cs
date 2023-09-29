using System.Collections;
using UnityEngine;

public class AlienSwarmMovementController : MonoBehaviour
{
    [Header("Swarm Reference")]
    [SerializeField] private GameObject[] alienShips;

    [Header("Movement")]
    [SerializeField] private float horizontalSpeed = 0.1f;
    [SerializeField] private float descentSpeed = 0.5f;
    [SerializeField] private float speedIncreasePerDestroyedAlien = 0.1f;

    private bool isMovingRight = true;
    private int numDestroyed = 0;
    private Coroutine swarmMovementCoroutine;

    public void Start()
    {
        swarmMovementCoroutine = StartCoroutine(MoveSwarmHorizontally());
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
        if (swarmMovementCoroutine != null)
        {
            StopCoroutine(swarmMovementCoroutine);
        }

        isMovingRight = !isMovingRight; // Change the movement direction from left to right or vice versa
        MoveSwarmDown(); // Descend on direction change
        swarmMovementCoroutine = StartCoroutine(MoveSwarmHorizontally());
    }

    public void OnDestroyEvent(GameObject alienToDestroy)
    {
        numDestroyed++;

        if (IsSwarmDestroyed())
        {
            GameStateHandler.Instance.TriggerWinState();
        }
        else
        {
            horizontalSpeed += speedIncreasePerDestroyedAlien;
        }
    }

    private bool IsSwarmDestroyed()
    {
        return numDestroyed == alienShips.Length;
    }
}
