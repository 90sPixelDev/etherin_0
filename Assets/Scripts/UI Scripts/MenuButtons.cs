using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public MenuManager MenuManager;

    // Start is called before the first frame update
    void Start()
    {
        MenuManager = GameObject.Find("UICanvas").GetComponent<MenuManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResumeButton()
    {
        MenuManager.isPaused = false;
        MenuManager.cameraGO.GetComponent<MouseLook>().enabled = true;
        UnityEngine.Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        MenuManager.mainMenuUI.SetActive(false);
        MenuManager.pointerUI.SetActive(true);
    }
    public void OptionsButton()
    {
        //Gotta make options UI and settings to change
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
