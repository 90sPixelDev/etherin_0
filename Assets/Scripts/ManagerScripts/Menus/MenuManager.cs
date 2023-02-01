using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public GameObject inventoryUI;
    public GameObject mainMenuUI;
    public GameObject debugMenuUI;
    public GameObject pointerUI;

    public GameObject cameraGO;
    //public Transform contentWindow;
    //public SelectionManager SelectionManager;


    // Start is called before the first frame update
    void Start()
    {
        //if (IsHost || IsClient)
        //{
        //    playerNGO = NetworkManager.Singleton.LocalClient.PlayerObject;
        //}
        mainMenuUI = gameObject.GetComponentInChildren<MenuButtons>().gameObject;
        debugMenuUI = GameObject.Find("DebugMenuUI");
        //inventoryUI.SetActive(false);
        //debugMenuUI.SetActive(false);
        //mainMenuUI.SetActive(false);
    }

    public void MainMenu(GameObject playerGO)
    {
        Debug.Log("Running on MenuManager!");
        var isMobile = playerGO.GetComponent<CharacterControllerScript>().IsMobile;
        var playerNetState = playerGO.GetComponent<PlayerNetworkState>();
        Debug.Log(isMobile);

        if (!playerNetState.inMainMenu.Value && !isMobile)
        {
            Cursor.lockState = CursorLockMode.None;
            pointerUI.SetActive(false);
            mainMenuUI.SetActive(true);
            playerNetState.inMainMenu.Value = false;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            pointerUI.SetActive(true);
            mainMenuUI.SetActive(false);
            playerNetState.inMainMenu.Value = false;
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

        if (!playerNetState.inMenu.Value)
        {
            playerNetState.inMenu.Value = true;
            inventoryUI.SetActive(true);
            pointerUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (playerNetState.inMenu.Value)
        {
            playerNetState.inMenu.Value = false;
            inventoryUI.SetActive(false);
            pointerUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
