using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float velocityHitMultiplier;
    [SerializeField] private float timeToUp = 5f;

    [SerializeField] private Transform targeMove;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;

    [SerializeField] private float downAndUpSpeed = 5f;
    [SerializeField] private Transform targeRotation;
    [SerializeField] private Quaternion downRatation;

    [SerializeField] private bool isRight;

    private bool isDown;

    private Quaternion originalRotation;

    private float currentHealth;
    private float currentSpeed;
    private float time;

    void Start()
    {
        originalRotation = targeRotation.localRotation;
        currentHealth = health;
        currentSpeed = speed;
        time = timeToUp;
    }

    void Update()
    {
        isDown = currentHealth <= 0;

        if (isDown) 
        {
            targeRotation.localRotation = Quaternion.Slerp(targeRotation.localRotation, downRatation, Time.deltaTime * downAndUpSpeed);

            if(time <= 0) 
            {
                time = timeToUp;
                currentHealth = health;
                currentSpeed = speed;
            }
            else 
            {
                time -= Time.deltaTime;
            }
        }
        else 
        {
            targeRotation.localRotation = Quaternion.Slerp(targeRotation.localRotation, originalRotation, Time.deltaTime* downAndUpSpeed);

            targeMove.Translate(isRight ? currentSpeed * Time.deltaTime * Vector3.up : currentSpeed * Time.deltaTime * -Vector3.up);

            if (Vector2.Distance(targeMove.position, leftPoint.position) < 0.5f || Vector2.Distance(targeMove.position, rightPoint.position) < 0.5f)
            {
                isRight = !isRight;
            }
        }
    }

    public void Damage(float amount) 
    {
        currentHealth -= amount;
        currentSpeed *= velocityHitMultiplier;
        isRight = !isRight;

        if (currentSpeed > maxSpeed)
            currentSpeed = maxSpeed;
    }
}
