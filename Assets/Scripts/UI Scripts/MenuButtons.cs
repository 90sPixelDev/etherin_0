using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public MenuManager menuManager;

    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.Find("UICanvas").GetComponent<MenuManager>();
    }

    public void ResumeButton()
    {
        menuManager.isPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        menuManager.mainMenuUI.SetActive(false);
        menuManager.pointerUI.SetActive(true);
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
