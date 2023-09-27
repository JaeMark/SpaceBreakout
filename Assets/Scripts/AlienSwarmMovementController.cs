using System.Collections;
using UnityEngine;

public class AlienSwarmMovementController : MonoBehaviour
{
    [SerializeField] private GameObject[] alienShips;
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
        StopCoroutine(swarmMovementCoroutine);

        isMovingRight = !isMovingRight;
        MoveSwarmDown();
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

        Destroy(alienToDestroy);
    }

    private bool IsSwarmDestroyed()
    {
        return numDestroyed == alienShips.Length;
    }
}
