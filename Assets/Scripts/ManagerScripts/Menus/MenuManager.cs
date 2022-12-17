using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : NetworkBehaviour
{
    public NetworkObject playerNGO;
    public GameObject inventoryUI;
    public GameObject debugMenuUI;

    public GameObject cameraGO;
    //public Transform contentWindow;
    //public SelectionManager SelectionManager;

    public GameObject playerHUD;
    public GameObject mainMenuUI;
    public GameObject pointerUI;

    public bool isPaused = false;
    public bool inMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        playerHUD = GameObject.FindGameObjectWithTag("MainUI");
        playerNGO = GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkObject>();
        mainMenuUI = playerHUD.GetComponentInChildren<MenuButtons>().gameObject;
        debugMenuUI = GameObject.Find("DebugMenuUI");
        cameraGO = GameObject.Find("PlayerViewCam");
        inventoryUI.SetActive(false);
        debugMenuUI.SetActive(false);
        mainMenuUI.SetActive(false);

        //SelectionManager = GameObject.Find("GameManager").GetComponent<SelectionManager>();
    }

    public void MainMenu(bool canMove)
    {
        if (!isPaused && !canMove)
        {
            isPaused = true;
            Cursor.lockState = CursorLockMode.None;
            pointerUI.SetActive(false);
            mainMenuUI.SetActive(true);
            inMenu = false;
            Cursor.visible = true;
        }
        else
        {
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            pointerUI.SetActive(true);
            mainMenuUI.SetActive(false);
            Cursor.visible = false;
        }
    }

    public void ResumeButton()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerHUD.SetActive(false);
        pointerUI.SetActive(true);
        Cursor.visible = false;
    }
    public void OptionsButton()
    {
        //Gotta make options UI and settings to change
    }
    public void QuitButton()
    {
        Application.Quit();
    }

    public void Inventory(bool canMove)
    {
        if (isPaused)
            return;
        if (!inMenu && !canMove)
        {
            inMenu = true;
            inventoryUI.SetActive(true);
            pointerUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (inMenu)
        {
            inMenu = false;
            inventoryUI.SetActive(false);
            pointerUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
