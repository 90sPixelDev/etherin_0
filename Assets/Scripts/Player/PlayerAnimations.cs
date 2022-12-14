using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerAnimations : MonoBehaviour
{
    public bool hasAxe = false;
    public bool canSwing = false;
    public bool isSwinging = false;
    public float swingTimer = 1;

    public bool canPlayRunAnimAgain = true;
    public bool isRunning => characterControllerScript.GetisRunning;
    public float runTimer = 1;

    public Animation playerAnim;

    public CharacterController fpsController;
    public PlayerVitals playerVitals;
    //public HotBarSelector HotBarSelector;
    public CharacterControllerScript characterControllerScript;
    public MenuManager menuManager;

    public void Start()
    {
        //fpsController = GameObject.Find("FPSPlayer").GetComponent<CharacterController>();
        //HotBarSelector = GameObject.Find("HotBarContent").GetComponent<HotBarSelector>();
        playerVitals = GetComponent<PlayerVitals>();
        characterControllerScript = GetComponent<CharacterControllerScript>();
        //menuManager = GameObject.Find("UICanvas").GetComponent<MenuManager>();
    }

    public void Update()
    {
        ////IDLE SECTION
        //if (fpsController.velocity.magnitude <= 0 && !isSwinging && !HotBarSelector.selectedUIItemSlot.itemSlot.hasItem)
        //{
        //    playerAnim.Play("Idle");
        //    playerAnim["Idle"].wrapMode = WrapMode.Loop;
        //    playerAnim["Idle"].speed = 1f;
        //}
        //if (fpsController.velocity.magnitude <= 0 && !isSwinging && HotBarSelector.selectedUIItemSlot.itemSlot.hasItem)
        //{
        //    playerAnim.Play("IdleWithWeapon");
        //    playerAnim["IdleWithWeapon"].wrapMode = WrapMode.Loop;
        //    playerAnim["IdleWithWeapon"].speed = 1f;
        //}


        ////RUN SECTION | CURRENTLY NOT WORKING NEEDS FIXING |
        //if (isRunning && fpsController.velocity.magnitude > 0 && canPlayRunAnimAgain)
        //{
        //    //Debug.Log("Player is running!");
        //    playerAnim["Run"].blendMode = AnimationBlendMode.Blend;
        //    playerAnim["Run"].wrapMode = WrapMode.Loop;
        //    playerAnim["Run"].speed = 1f;
        //    playerAnim.Play("Run");

        //    //canPlayRunAnimAgain = false;
        //}

        //if (isRunning)
        //{
        //    runTimer -= Time.deltaTime;
        //}
        //if (runTimer <=0  || runTimer < 1.292f && !isRunning)
        //{
        //    //canPlayRunAnimAgain = true;
        //    runTimer = 1.292f;
        //}

        ////SWINGING
        //if (Input.GetMouseButton(0) && !menuManager.inMenu && canSwing)
        //{
        //    if (!isSwinging)
        //    {
        //        if (HotBarSelector.selectedUIItemSlot.itemSlot.hasItem)
        //        {
        //            //TOOL SWINGING WITH CORRECT ANGLE ADJUSTMENT
        //            if ( HotBarSelector.selectedUIItemSlot.itemSlot.item.itemType == ItemType.Tool)
        //            {
        //                //Reduce Stamina
        //                playerVitals.currentStamina -= 20;
        //                //Swing Animation
        //                playerAnim.Play("Swing02");
        //                playerAnim["Swing02"].speed = 1.5f;
        //                isSwinging = true;
        //                canSwing = false;
        //            }
        //            //WEAPON SWINGING WITH CORRECT ANGLE ADJUSTMENT
        //            if (HotBarSelector.selectedUIItemSlot.itemSlot.item.itemType == ItemType.Weapon)
        //            {
        //                //Reduce Stamina
        //                playerVitals.currentStamina -= 12;
        //                //Swing Animation
        //                playerAnim.Play("Swing01");
        //                playerAnim["Swing01"].speed = 1.5f;
        //                isSwinging = true;
        //                canSwing = false;
        //            }
        //        }
        //        //IF NO ITEM IS CURRENTLY IN SLOT THEN JUST PUNCH
        //        else
        //        {
        //            playerVitals.currentStamina -= 7;
        //            playerAnim.Play("PunchRight");
        //            //playerAnim["PunchRight"].wrapMode = WrapMode.Loop;
        //            playerAnim["PunchRight"].speed = 1f;
        //            isSwinging = true;
        //            canSwing = false;
        //        }
        //    }
        //}

        ////COUNTDFOWN UNTIL PLAYER CHARACTER CAN SWING AGAIN
        //if (canSwing == false)
        //{
        //    swingTimer -= Time.deltaTime;
        //}
        ////AMOUNT OF TIME TO BE ABLE TO SWING AGAIN
        //if (swingTimer <= 0)
        //{
        //    swingTimer = 1f;
        //    canSwing = true;
        //    isSwinging = false;
        //}

    }

}
