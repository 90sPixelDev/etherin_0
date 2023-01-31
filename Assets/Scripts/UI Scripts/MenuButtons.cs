using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MenuButtons : NetworkBehaviour
{
    public MenuManager menuManager;

    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.Find("UICanvas").GetComponent<MenuManager>();
    }

    public void ResumeButton()
    {
        menuManager.ResumeButton();
    }
    public void OptionsButton()
    {
        //Gotta make options UI and settings to change
    }
    public void QuitButton()
    {
        NetworkManager.Shutdown();
        Application.Quit();
    }
}
