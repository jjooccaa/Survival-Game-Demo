using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    [SerializeField] float speed = 12f;
    [SerializeField] float gravity = -9.81f * 2;
    [SerializeField] float jumpHeight = 3f;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;

    Vector3 lastPosition = new Vector3();

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float forwardInput = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalInput + transform.forward * forwardInput;

        characterController.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded) 
        {

            SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
        }
        else
        {
            SoundManager.Instance.grassWalkSound.Stop();
        }

        lastPosition = gameObject.transform.position;
    }
}
