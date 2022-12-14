using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : NetworkBehaviour
{
    public GameObject inventoryUI;
    public GameObject debugMenuUI;

    public GameObject cameraGO;
    //public Transform contentWindow;
    //public SelectionManager SelectionManager;

    public GameObject mainMenuUI;
    public GameObject pointerUI;

    public bool isPaused = false;
    public bool inMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        inventoryUI = GameObject.Find("PlayerInventoryUI");
        debugMenuUI = GameObject.Find("DebugMenuUI");
        cameraGO = GameObject.Find("PlayerVIewCam");
        inventoryUI.SetActive(false);
        debugMenuUI.SetActive(false);
        //SelectionManager = GameObject.Find("GameManager").GetComponent<SelectionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            //if (Input.GetButtonDown("Cancel"))
            //    MainMenu();
            //if (Input.GetButtonDown("Menu"))
            //{
            //    Inventory();
            //}
        }
    }

    public void MainMenu()
    {
        if (!isPaused)
        {
            isPaused = true;
            Cursor.lockState = CursorLockMode.None;
            mainMenuUI.SetActive(true);
            pointerUI.SetActive(false);
            inMenu = false;
            inventoryUI.SetActive(false);
            Cursor.visible = true;
        }
        else
        {
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            mainMenuUI.SetActive(false);
            pointerUI.SetActive(true);
            Cursor.visible = false;
        }
    }

    public void ResumeButton()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        mainMenuUI.SetActive(false);
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

    public void Inventory()
    {
        if (isPaused)
            return;
        if (!inMenu)
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
