using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController playerController;
    public PlayerVitals PlayerVitals;

    [Header("Player Movement Speeds")]
    public float currentSpeed = 12f;
    public float walkSpeed = 12f;
    public float runSpeed = 18f;
    [Header("Gravity Force")]
    public float gravity = -9.81f;
    Vector3 velocity;
    [Header("On Ground Information")]
    public float groundDistance = 0.07f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public bool isGrounded;
    public bool isRunning;
    public bool isOnSlope;

    public float jumpheight = 1f;
    [Header("Conditions")]
    public bool isMobile = true;

    public void Start()
    {
        isRunning = false;
        PlayerVitals = GetComponent<PlayerVitals>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            isMobile = !isMobile;
        }
        //isGrounded is true as long as the physiscs system checks that the groundCheck is within the groundDistance of the groundMask!
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //If character is on the ground and their velocity is below 0 reset it to -2 so it doesnt continue to drop forever
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        if (isMobile)
        {
            //Naming the movement axis!
            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");

            //Configure the inputs to the movement type
            Vector3 move = transform.right * xMove + transform.forward * zMove;

            //Telling to Run
            playerController.Move(move * currentSpeed * UnityEngine.Time.deltaTime);
            //Applying gravity over time
            velocity.y += gravity * UnityEngine.Time.deltaTime;
            //Moving character down due to gravity
            playerController.Move(velocity * UnityEngine.Time.deltaTime);

            //When pressing Jump to actually jump!
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpheight * -2 * gravity);
            }

            //To configure to run when holding "run"
            if (Input.GetButton("Run") && PlayerVitals.currentStamina > 1f)
            {
                isRunning = true;
                currentSpeed = runSpeed;
            }
            else
            {
                isRunning = false;
                currentSpeed = walkSpeed;
            }
        }

    }
}
