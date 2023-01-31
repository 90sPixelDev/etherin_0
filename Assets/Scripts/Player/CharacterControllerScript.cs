using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerScript : NetworkBehaviour
{
    [SerializeField] private CharacterController charaController;
    [SerializeField] private NetworkObject ngo;
    //[SerializeField] private DefaultInputActions defaultInputActions;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerVitals playerVitals;
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
    public bool IsMobile { get => _IsMobile; set => _IsMobile = value; }

    [Header("References Extra")]
    [SerializeField] public Transform fpsCam;
    [SerializeField] public MenuManager playerHUD;
    [SerializeField] GameObject playerInventoryUI;
    private Vector2 moveInput;
    private Vector2 lookPos;
    public bool refsLoaded = false;

    [Header("DEBUG")]
    [SerializeField] GameObject testObject;
    [SerializeField] public string testString = "TESTER!";


    private void Awake()
    {
        charaController = GetComponent<CharacterController>();
        ngo = GetComponent<NetworkObject>();
        playerInput = GetComponent<PlayerInput>();
        playerVitals = GetComponent<PlayerVitals>();
    }

    [ClientRpc]
    public void SetReferencesClientRPC()
    {
        //if (!IsOwner) return;
        fpsCam = GameObject.FindGameObjectWithTag("FPCam").transform;
        playerHUD = GameObject.FindGameObjectWithTag("MainUI").GetComponent<MenuManager>();
        playerInventoryUI = GameObject.FindGameObjectWithTag("InventoryUI");
        refsLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        //return;
        if (IsOwner && refsLoaded)
        {

            if (IsMobile)
            {
                PlayerMove();
                CameraFollowPlayer();
                PlayerLook();
            }

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

    public void CameraFollowPlayer()
    {
        if (!(refsLoaded && IsOwner)) return;
        fpsCam.position = transform.position + new Vector3(0, 3.5f, 0);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        Debug.Log("CHARACTER MOVING!");
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
        if (!IsOwner) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2 * gravity);
        }
    }

    public void OnMouseLook(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        lookPos = ctx.ReadValue<Vector2>();
    }

    public void PlayerLook()
    {
        if (!refsLoaded) return;
        Cursor.lockState = CursorLockMode.Locked;

        xRotation -= lookPos.y * Time.deltaTime * 30;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        yRotation += lookPos.x * Time.deltaTime * 30;
        fpsCam.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        currentSpeed *= 1.35f;
    }

    public void OnMenu()
    {
        if (IsOwner)
        {
        var movementAbility = IsMobile ? IsMobile = false : IsMobile = true;
        playerHUD.MainMenu(movementAbility);
        }
    }
    
    public void OnInventoryUI()
    {
        if (IsOwner)
        {
        var movementAbility = IsMobile ? IsMobile = false : IsMobile = true;
        playerHUD.Inventory(movementAbility);
        }
    }
}
