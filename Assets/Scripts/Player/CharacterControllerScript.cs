using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using SmartConsole;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class CharacterControllerScript : NetworkBehaviour
{
    public enum PlayerAnimState
    {
        IDLE,
        WALK,
        WALK_REVERSE,
        RUN,
        RUN_REVERSE,
        WALK_LEFT,
        WALK_RIGHT
    }

    [SerializeField] CharacterController charaController;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] string currControlScheme;
    [SerializeField] InputDevice currDevice;
    [SerializeField] PlayerNetworkVitals playerNetworkVitals;
    [SerializeField] PlayerNetworkState playerNetworkState;
    [SerializeField] Animator animator;
    [SerializeField] PlayerTag playerTag;
    [SerializeField] SelectionManager selectionManager;
    [SerializeField] LootContainerNetwork lootContainerNetwork;
    float xRotation, yRotation;

    [Header("Player Movement Speeds")]
    [SerializeField] float currentSpeed;
    const float walkSpeed = 5;
    const float runSpeed = 10;
    [SerializeField]
    private NetworkVariable<PlayerAnimState> netAnimState = new NetworkVariable<PlayerAnimState>(PlayerAnimState.IDLE, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);

    [Header("Gravity Force")]
    float gravity = -9.81f;
    Vector3 velocity;

    [Header("On Ground Information")]
    [SerializeField] float groundDistance = 0.07f;
    [SerializeField] int jumpheight = 2;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    [Header("Conditions")]
    [SerializeField] bool _IsMobile = true;
    public bool GetCanMove { get => _IsMobile; set => _IsMobile = value; }
    [SerializeField] bool _CanLook = true;
    public bool GetCanLook { get => _CanLook; set => _CanLook = value; }
    [SerializeField] bool _isRunning = false;
    public bool GetisRunning { get => _isRunning; set => _isRunning = value; }
    [SerializeField] bool isGrounded;
    [SerializeField] bool isOnSlope;

    [Header("References Extra")]
    [SerializeField] Transform fpsCam;
    private Vector2 moveInput;
    private Vector2 lookPos;
    public bool refsLoaded = false;

    //[Header("UI References")]
    private GameObject mainMenuUI;
    private GameObject playerInvMenuUI;
    private GameObject debugMenuUI;
    private GameObject pointerUI;
    private GameObject mainMenuFirstSelect;
    private GameObject playerInvMenuFirstSelect;
    private GameObject debugMenuFirstSelect;

    private void Awake()
    {
        charaController = GetComponent<CharacterController>();
        playerNetworkVitals = GetComponent<PlayerNetworkVitals>();
        playerNetworkState = GetComponent<PlayerNetworkState>();
        playerInput = GetComponent<PlayerInput>();
        lootContainerNetwork = GetComponentInChildren<LootContainerNetwork>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnNetworkSpawn()
    {
        enabled = IsClient;
        if (!IsOwner)
        {
            enabled = false;
            charaController.enabled = false;

            return;
        }


        playerInput.enabled = true;
        charaController.enabled = true;
    }

    private void Start()
    {
        if (IsOwner)
        {
            transform.position = new Vector3(0, 35, 0);
        }
    }

    [ClientRpc]
    public void SetReferencesClientRPC(ClientRpcParams clientRpcParams)
    {
        if (refsLoaded) return;

        fpsCam = GameObject.FindGameObjectWithTag("FPCam").transform;
        selectionManager = fpsCam.GetComponent<SelectionManager>();
        mainMenuUI = GameObject.FindGameObjectWithTag("MainUI").transform.GetChild(0).gameObject;
        playerInvMenuUI = GameObject.FindGameObjectWithTag("PlayerInvUI").transform.GetChild(0).gameObject;
        pointerUI = GameObject.FindGameObjectWithTag("PointerUI").transform.GetChild(0).gameObject; ;
        debugMenuUI = GameObject.Find("DebugMenuUI").transform.GetChild(0).gameObject;

        mainMenuFirstSelect = mainMenuUI.transform.GetChild(1).gameObject;
        playerInvMenuFirstSelect = playerInvMenuUI.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        debugMenuFirstSelect = debugMenuUI.transform.GetChild(0).GetChild(2).gameObject;

        playerInput.uiInputModule = mainMenuUI.GetComponentInParent<InputSystemUIInputModule>();

        Cursor.lockState = CursorLockMode.Locked;

        refsLoaded = true;
    }

    // Update is called once per frame
    private void Update()
    {
        //return;
        if (IsOwner && refsLoaded)
        {
            if (GetCanMove && GetCanLook)
            {
                PlayerMove();
                PlayerLook();
                CheckIfOnGround();
                CameraFollowPlayer();
            }
            else if (GetCanMove)
            {
                PlayerMove();
                CheckIfOnGround();
                CameraFollowPlayer();
            }

            velocity.y += gravity * Time.deltaTime;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -1f;
            }


            charaController.Move(velocity * Time.deltaTime);

            if (playerNetworkVitals.n_CurrentStamina.Value <= 0 || !_isRunning)
            {
                SetToWalkSpeed();
            }
            else if (playerNetworkVitals.n_CurrentStamina.Value > 0 && _isRunning)
            {
                SetToRunSpeed();
            }

            AnimVisualsServerRpc();
        }
    }

    private void SetToWalkSpeed()
    {
        currentSpeed = walkSpeed;
    }
    private void SetToRunSpeed()
    {
        currentSpeed = runSpeed;
    }

    public void OnTestNetworkVarByKeyPress(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        if (ctx.performed) playerNetworkVitals.EditHealth(-10f);
    }

    public void CameraFollowPlayer()
    {
        if (!(refsLoaded && IsOwner)) return;
        fpsCam.position = transform.position + new Vector3(0, 3.75f, 0.25f);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        moveInput = ctx.ReadValue<Vector2>();
        
    }

    public void PlayerMove()
    {
        if (!IsOwner) return;

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = moveInput.x;
        moveDirection.z = moveInput.y;
        charaController.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);

        if (moveDirection.x <= 0 && moveDirection.z <= 0)
        {
            _isRunning = false;
        }

        SetNetAnimState();
    }

    private void SetNetAnimState()
    {
        if (moveInput.y > 0 && !_isRunning)
        {
            netAnimState.Value = PlayerAnimState.WALK;
        }
        else if(moveInput.y > 0 && _isRunning)
        {
            netAnimState.Value = PlayerAnimState.RUN;
        }
        else if (moveInput.x < 0)
        {
            netAnimState.Value = PlayerAnimState.WALK_LEFT;
        }
        else if (moveInput.x > 0)
        {
            netAnimState.Value = PlayerAnimState.WALK_RIGHT;
        }
        else if (moveInput.y < 0 && moveInput.x < 0)
        {
            netAnimState.Value = PlayerAnimState.WALK_REVERSE;
        }
        else if (moveInput.y > 0 && moveInput.x > 0)
        {
            netAnimState.Value = PlayerAnimState.WALK;
        }
        else if (moveInput.y < 0)
        {
            netAnimState.Value = PlayerAnimState.WALK_REVERSE;
        }
        else
        {
            netAnimState.Value = PlayerAnimState.IDLE;
        }
    }

    public void AnimVisuals()
    {
        if (netAnimState.Value == PlayerAnimState.WALK)
        {
            animator.SetFloat("DirectionY", 1f);
            animator.SetFloat("DirectionX", 0f);
        }
        else if (netAnimState.Value == PlayerAnimState.RUN)
        {
            animator.SetFloat("DirectionY", 2f);
            animator.SetFloat("DirectionX", 0f);
        }
        else if (netAnimState.Value == PlayerAnimState.IDLE)
        {
            animator.SetFloat("DirectionY", 0f);
            animator.SetFloat("DirectionX", 0f);
        }
        else if (netAnimState.Value == PlayerAnimState.WALK_REVERSE)
        {
            animator.SetFloat("DirectionY", -1f);
            animator.SetFloat("DirectionX", 0f);
        }
        else if (netAnimState.Value == PlayerAnimState.WALK_LEFT)
        {
            animator.SetFloat("DirectionX", -1f);
            animator.SetFloat("DirectionY", 0f);

        }
        else if (netAnimState.Value == PlayerAnimState.WALK_RIGHT)
        {
            animator.SetFloat("DirectionX", 1f);
            animator.SetFloat("DirectionY", 0f);
        }
    }


    [ServerRpc]
    public void AnimVisualsServerRpc()
    {
        AnimVisuals();
    }

    public void CheckIfOnGround()
    {
        if (!IsOwner) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        var runValue = ctx.ReadValueAsButton();

        if (ctx.performed)
        {
            Debug.Log("JUMP!");
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpheight * -2 * gravity);
            }
        }
    }

    public void OnMouseLook(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        lookPos = ctx.ReadValue<Vector2>();
    }

    public void PlayerLook()
    {
        if (!refsLoaded && !IsOwner) return;

        int lookSensitivity = 15;

        if (PlayerInput.all[0].currentControlScheme == "Gamepad")
        {
            lookSensitivity = 170;
        }

        xRotation -= lookPos.y * lookSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        yRotation += lookPos.x * lookSensitivity * Time.deltaTime;
        fpsCam.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void OnRun(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        if (ctx.performed)
        {
            _isRunning = true;

        }
        if (ctx.canceled)
        {
            _isRunning = false;
        }
    }

    public void OnMenuUI(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        if (ctx.performed)
        {
            MenuUI();
        }

    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        if (ctx.performed)
        {
            if (selectionManager.isLookingAtSelectable && selectionManager.selectable != null && selectionManager.selectableItemFocusType == SelectionManager.SelectableItemFocusType.LOOT)
            {
                Debug.Log("INTERACTED WITH LOOT!");
                lootContainerNetwork.CommandLootUIServerRpc(NetworkManager.Singleton.LocalClientId);

            }
        }
    }

    public void OnInventoryUI(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        if (ctx.performed)
        {
            InventoryUI();
        }
    }

    public void InventoryUI()
    {
        if (playerNetworkState.n_inMainMenu.Value)
        {
            MenuUI();
        }
        if (playerNetworkState.n_inLootMenu.Value)
        {
            lootContainerNetwork.CloseLoot();
        }

        // Turning on Inventory
        if (!playerNetworkState.n_inMenu.Value)
        {
            playerNetworkState.n_inMenu.Value = true;
            playerTag.SetPlayerTagInMenuServerRpc();
            playerInvMenuUI.SetActive(true);
            pointerUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            playerInput.SwitchCurrentActionMap("UI");
            Debug.Log(playerInput.currentActionMap);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(playerInvMenuFirstSelect);
        }
        // Turning off Inventory
        else if (playerNetworkState.n_inMenu.Value)
        {
            playerNetworkState.n_inMenu.Value = false;
            playerTag.UpdatePlayerTagVarServerRpc();
            playerInvMenuUI.SetActive(false);
            pointerUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerInput.SwitchCurrentActionMap("Player");
            Debug.Log(playerInput.currentActionMap);
        }
    }

    private void MenuUI()
    {
        if (playerNetworkState.n_inMenu.Value)
        {
            InventoryUI();
        }

        // Turning on Menu
        if (!playerNetworkState.n_inMainMenu.Value)
        {
            Cursor.lockState = CursorLockMode.None;
            playerTag.SetPlayerTagInMenuServerRpc();
            pointerUI.SetActive(false);
            mainMenuUI.SetActive(true);
            playerNetworkState.n_inMainMenu.Value = true;
            Cursor.visible = true;

            playerInput.SwitchCurrentActionMap("UI");
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainMenuFirstSelect);
        }
        // Turning off Menu
        else if (playerNetworkState.n_inMainMenu.Value)
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerTag.UpdatePlayerTagVarServerRpc();
            pointerUI.SetActive(true);
            mainMenuUI.SetActive(false);
            playerNetworkState.n_inMainMenu.Value = false;
            Cursor.visible = false;

            playerInput.SwitchCurrentActionMap("Player");
        }
    }
}
