using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class MenuManager : NetworkBehaviour
{
    [Header("Menus")]
    public GameObject inventoryUI;
    public GameObject mainMenuUI;
    public GameObject debugMenuUI;
    public GameObject pointerUI;
    [Header("Player")]
    public GameObject playerNGO;
    [Header("Other")]
    public GameObject cameraGO;

    void Start()
    {

        //mainMenuUI = gameObject.GetComponentInChildren<MenuButtons>().gameObject;
        //debugMenuUI = GameObject.Find("DebugMenuUI");
        //inventoryUI.SetActive(false);
        //debugMenuUI.SetActive(false);
        //mainMenuUI.SetActive(false);
    }
    public void MainMenu(GameObject playerGO)
    {
        Debug.Log("Running on MenuManager!");
        var isMobile = playerGO.GetComponent<CharacterControllerScript>().GetCanMove;
        var playerNetState = playerGO.GetComponent<PlayerNetworkState>();
        Debug.Log(isMobile);

        if (!playerNetState.n_inMainMenu.Value && !isMobile)
        {
            Cursor.lockState = CursorLockMode.None;
            pointerUI.SetActive(false);
            mainMenuUI.SetActive(true);
            playerNetState.n_inMainMenu.Value = false;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            pointerUI.SetActive(true);
            mainMenuUI.SetActive(false);
            playerNetState.n_inMainMenu.Value = false;
            Cursor.visible = false;
        }
    }

    public void ResumeButton()
    {
        //MainMenu();
    }
    public void OptionsButton()
    {
        //Gotta make options UI and settings to change
    }
    public void QuitButton()
    {
        //if (!IsOwner) return;
        //Application.Quit();
    }

    public void Inventory(GameObject playerGO)
    {
        Debug.Log("Running on MenuManager!");
        var playerNetState = playerGO.GetComponent<PlayerNetworkState>();

        if (!playerNetState.n_inMenu.Value)
        {
            playerNetState.n_inMenu.Value = true;
            inventoryUI.SetActive(true);
            pointerUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (playerNetState.n_inMenu.Value)
        {
            playerNetState.n_inMenu.Value = false;
            inventoryUI.SetActive(false);
            pointerUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
