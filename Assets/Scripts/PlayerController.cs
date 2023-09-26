using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float accelerationRate = 0.1f;
    [SerializeField] private float accelerationCurrent = 0f;
    [SerializeField] private float accelerationMax = 1f;
    [SerializeField] private float accelerationDecay = 0.05f;
    [SerializeField] private bool hasAccelerationDecay = true;

    [SerializeField] private float velocityCurrent = 0f;
    [SerializeField] private float velocityMax = 10f;
    [SerializeField] private float velocityDecay = 1.0f;
    [SerializeField] private bool hasVelocityDecay = true;

    [SerializeField] private float boostMaxVelocity = 20f; // New max speed during boost

    [SerializeField] private float leftBorder = -7f;
    [SerializeField] private float rightBorder = 7f;

    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>(); 
    }

    public float GetMoveSpeed()
    {
        return velocityMax;
    }

    public float GetCurrentSpeed()
    {
        return velocityCurrent;
    }

    private void Update()
    {
        float currentVelocityMax = velocityMax;
        if (Input.GetAxisRaw("Jump") > 0.1f)
        {
            currentVelocityMax = boostMaxVelocity;
        }

        float movementInput = Input.GetAxisRaw("Horizontal");

        accelerationCurrent += accelerationRate * movementInput;
        if (hasAccelerationDecay)
        {
            if (accelerationCurrent > 0)
            {
                accelerationCurrent -= accelerationDecay;
            }
            else if (accelerationCurrent < 0)
            {
                accelerationCurrent += accelerationDecay;
            }
        }
        accelerationCurrent = Mathf.Clamp(accelerationCurrent, -accelerationMax, accelerationMax);

        velocityCurrent += accelerationCurrent;
        if (hasVelocityDecay)
        {
            if (velocityCurrent > 0)
            {
                velocityCurrent -= velocityDecay;
            }
            else if (velocityCurrent < 0)
            {
                velocityCurrent += velocityDecay;
            }
        }
        velocityCurrent = Mathf.Clamp(velocityCurrent, -currentVelocityMax, currentVelocityMax);

        Vector3 inputVelocity = new Vector3(velocityCurrent, 0, 0);
        transform.Translate(inputVelocity * Time.deltaTime);

        // Handle borders
        Vector3 shipPosition = transform.position;
        shipPosition.x = Mathf.Clamp(transform.position.x, leftBorder, rightBorder);
        transform.position = shipPosition;
    }
}