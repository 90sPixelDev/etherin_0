using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using UnityEngine.UI;

public class ControlsManager : NetworkBehaviour
{
    [SerializeField]
    PlayerInput playerInput;

    [Header("UI")]
    [SerializeField] Texture2D gamepadReticle;
    [SerializeField] Texture2D mouseReticle;

    void Awake()
    {
        playerInput = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerInput>();
        playerInput.onControlsChanged += UIReticleSet;
    }

    private void UIReticleSet(PlayerInput plyObj)
    {
        Debug.Log("Something Changed on Controls!");

        //if (plyObj.currentControlScheme == "Gamepad")
        //{
        //    Debug.Log("GAMEPAD!");
        //    Cursor.SetCursor(gamepadReticle, new Vector2(32, 32), CursorMode.Auto);
        //} else
        //{
        //    Debug.Log("OTHER");
        //    Cursor.SetCursor(mouseReticle, new Vector2(0, 0), CursorMode.Auto);
        //}
    }
}
