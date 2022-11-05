using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    Animator animator;

    [SerializeField] float moveSpeed = 0.2f;

    float movingTimer;
    float movingCounter;

    float waitingTimer;
    float waitingCounter;

    Vector3 stopPosition;
    int moveDirection;

    bool isMoving;

    void Start()
    {
        animator = GetComponent<Animator>();

        movingTimer = Random.Range(3, 8);
        waitingTimer = Random.Range(2, 5);

        waitingCounter = waitingTimer;
        movingCounter = movingTimer;

        GetMoveDirection();
    }

    void Update()
    {
        if(isMoving)
        {
            Move();

            if(movingCounter <= 0)
            {
                StopMovement();
            }
        }
        else
        {
            waitingCounter -= Time.deltaTime;

            if(waitingCounter <= 0)
            {
                GetMoveDirection();
            }
        }
    }

    void Move()
    {
        animator.SetBool("IsRunning", true);
        movingCounter -= Time.deltaTime;

        switch (moveDirection)
        {
            case 0:
                MoveToDirection(new Vector3(0f, 0f, 0f));
                break;

            case 1:
                MoveToDirection(new Vector3(0f, 90f, 0f));
                break;

            case 2:
                MoveToDirection(new Vector3(0f, -90f, 0f));
                break;

            case 3:
                MoveToDirection(new Vector3(0f, 180f, 0f));
                break;
        }
    }

    void MoveToDirection(Vector3 rotationDirection)
    {
        transform.localRotation = Quaternion.Euler(rotationDirection);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void StopMovement()
    {
        stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isMoving = false;
        transform.position = stopPosition;

        animator.SetBool("IsRunning", false);

        waitingCounter = waitingTimer;     //reset the waitCounter
    }

    void GetMoveDirection()
    {
        moveDirection = Random.Range(0, 4);

        isMoving = true;
        movingCounter = movingTimer;
    }
}
