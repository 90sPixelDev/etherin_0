using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public GameObject inventoryUI;
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
        Cursor.lockState = CursorLockMode.Locked;

        inventoryUI.SetActive(false);
        cameraGO = GameObject.Find("Camera");
        //SelectionManager = GameObject.Find("GameManager").GetComponent<SelectionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            MainMenu();
        if (Input.GetButtonDown("Menu"))
        {
            Inventory();
        }

    }

    public void MainMenu()
    {
        if (!isPaused)
        {
            isPaused = true;
            cameraGO.GetComponent<MouseLook>().enabled = false;
            Time.timeScale = 0f;
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
            cameraGO.GetComponent<MouseLook>().enabled = true;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            mainMenuUI.SetActive(false);
            pointerUI.SetActive(true);
            Cursor.visible = false;
        }
    }

    public void PauseGame()
    {
        //INCORPORTATE A METHOD TO FREEZE GAME TIME AND SHOW CURSOR
    }

    public void ResumeButton()
    {
        isPaused = false;
        cameraGO.GetComponent<MouseLook>().enabled = true;
        Time.timeScale = 1f;
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
            cameraGO.GetComponent<MouseLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (inMenu)
        {
            inMenu = false;
            inventoryUI.SetActive(false);
            pointerUI.SetActive(true);
            cameraGO.GetComponent<MouseLook>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
