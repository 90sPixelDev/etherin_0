using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerScript : NetworkBehaviour
{
    [SerializeField] private CharacterController charaController;
    [SerializeField] private DefaultInputActions defaultInputActions;
    [SerializeField] private Transform fpsCam;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerVitals PlayerVitals;
    float xRotation, yRotation;

    [Header("Player Movement Speeds")]
    [SerializeField] private float currentSpeed = 12f;
    [SerializeField] private float walkSpeed = 12f;
    [SerializeField] private float runSpeed = 18f;
    [Header("Gravity Force")]
    [SerializeField] private float gravity = -9.81f;
    Vector3 velocity;
    [Header("On Ground Information")]
    [SerializeField] private float groundDistance = 0.07f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool _isRunning = false;
    public bool GetisRunning { get => _isRunning; set => _isRunning = value; }
    [SerializeField] private bool isOnSlope;

    [SerializeField] public float jumpheight = 1f;
    [Header("Conditions")]
    [SerializeField] private bool _IsMobile = true;
    public bool IsMobile { get => _IsMobile; set => _IsMobile = !_IsMobile; }

    [Header("References Extra")]
    [SerializeField] MenuManager menuManager;
    private Vector2 moveInput;
    private Vector2 lookPos;


    private void Awake()
    {
        charaController = GetComponent<CharacterController>();
    }
    public void Start()
    {
        PlayerVitals = GetComponent<PlayerVitals>();
        //menuManager = GameObject.Find("UICanvas").GetComponent<MenuManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner && IsMobile)
        {
            PlayerMove();
            PlayerLook();

            velocity.y += gravity * Time.deltaTime;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -1f;
            }
            charaController.Move(velocity * Time.deltaTime);

            //To configure to run when holding "run"
            //if (Input.GetButton("Run") && PlayerVitals.currentStamina > 1f)
            //{
            //    isRunning = true;
            //    currentSpeed = runSpeed;
            //}
            //else
            //{
            //    isRunning = false;
            //    currentSpeed = walkSpeed;
            //}
            //}
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        moveInput = ctx.ReadValue<Vector2>();
    }

    public void PlayerMove()
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = moveInput.x;
        moveDirection.z = moveInput.y;
        charaController.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2 * gravity);
        }
    }

    public void OnMouseLook(InputAction.CallbackContext ctx)
    {
        lookPos = ctx.ReadValue<Vector2>();
    }

    public void PlayerLook()
    {
        Cursor.lockState = CursorLockMode.Locked;

        xRotation -= lookPos.y * Time.deltaTime * 30;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        yRotation += lookPos.x * Time.deltaTime * 30;
        fpsCam.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        currentSpeed *= 1.35f;
    }
    
    //public void OnMenu(InputAction.CallbackContext ctx)
    //{
    //    menuManager.MainMenu();
    //}
}
