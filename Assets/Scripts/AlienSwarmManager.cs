using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlienSwarmManager : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 0.1f;
    [SerializeField] private float descentSpeed = 0.5f;
    [SerializeField] private float speedIncreasePerDestroyedAlien = 0.1f;

    private bool isMovingRight = true;
    private Coroutine swarmMovementCoroutine;
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

    public void OnDestroyEvent()
    {
        horizontalSpeed += speedIncreasePerDestroyedAlien;
    }
}
