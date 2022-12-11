using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    private Camera fpsCam;
    private MouseLook MouseLook;
    //private Item selectableItem;
    private Tree treeObject;
    //private Loot lootObject;
    private string lootName;
    public TextMeshProUGUI selectableName;
    public Image reticleSprite;
    private GameObject pointerUI;
    private PlayerMovement PlayerMovement;
    //public ItemsAddedUIManager itemAddedUIScript;
    //private ItemContainer itemContainer;
    //public EquipmentManager equipmentManager;

    public TextMeshProUGUI gameInfoText;

    public Image reticle;

    public bool isLookingAtSelectable;
    public bool hasBeenOpened;
    public bool lootIsOpened;
    public bool itemWasAdded;

    public GameObject SearchingUI;
    public Animation gameInfo;
    public Image searchingBar;

    //public PlayerInventory playerInventoryScript;
    //public ItemSlot currentItemSlot;

    private float barWidth;

    public GameObject parentWindow;
    public Transform contentWindow; //The GridLayout object we using
    public TextMeshProUGUI title;
    public MenuManager MenuManager;

    private float resourceAdded;
    private int lastTimeInt;

    void Start()
    {
        pointerUI = GameObject.Find("PointerUI");
        //SearchingUI = GameObject.Find("SearchingUI");
        fpsCam = GameObject.Find("Camera").GetComponent<Camera>();
        MouseLook = GameObject.Find("Camera").GetComponent<MouseLook>();
        //itemAddedUIScript = GameObject.Find("Items Added Panel").GetComponent<ItemsAddedUIManager>();
        MenuManager = GameObject.Find("UICanvas").GetComponent<MenuManager>();
        //itemContainer = GameObject.Find("Inventory").GetComponent<ItemContainer>();
        PlayerMovement = GameObject.Find("FPSPlayer").GetComponent<PlayerMovement>();

        isLookingAtSelectable = false;
        hasBeenOpened = false;

        SearchingUI.SetActive(false);
        selectableName.text = "";

        //Debug.Log("SelectionManager Counted: " + playerInventoryScript.playerItems.Count);
    }

    //public IEnumerator Searching(float duration)
    //{
    //    PlayerMovement.enabled = false;
    //    SearchingUI.SetActive(true);
    //    pointerUI.SetActive(false);
    //    MouseLook.enabled = false;

    //    var timer = 0f;
    //    while (timer <= duration)
    //    {
    //        //Searching Bar
    //        timer += UnityEngine.Time.deltaTime;
    //        float change = 100f / 175f;
    //        barWidth = Mathf.Lerp(0f, 175f, change * timer);

    //        searchingBar.rectTransform.sizeDelta = new Vector2(barWidth, searchingBar.rectTransform.sizeDelta.y);

    //        yield return null;
    //    }

    //    PlayerMovement.enabled = true;
    //    SearchingUI.SetActive(false);
    //    pointerUI.SetActive(true);
    //    MouseLook.enabled = true;

    //    if (!MenuManager.inMenu)
    //    {
    //        MenuManager.Inventory();
    //        itemContainer.OpenLoot(lootName, lootObject.lootItems);
    //        lootObject.isLooted = true;
    //    }
    //}

    //public void OpenLootedContainer()
    //{
    //    PlayerMovement.enabled = true;
    //    SearchingUI.SetActive(false);
    //    pointerUI.SetActive(true);
    //    MouseLook.enabled = true;

    //    if (!MenuManager.inMenu)
    //    {
    //        MenuManager.Inventory();
    //        itemContainer.OpenLoot(lootName, lootObject.lootItems); //THIS ONE
    //    }
    //    else if (MenuManager.inMenu)
    //    {
    //        itemContainer.OpenLoot(lootName, lootObject.lootItems);
    //    }
    //}

    public void AddResource()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = fpsCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        isLookingAtSelectable = false;
        reticle.color = Color.white;
        selectableName.text = "";
        if (Physics.Raycast(ray, out hit, 6.0f))
        {
            switch (hit.transform.gameObject.tag)
            {
                //case "Item":
                //    isLookingAtSelectable = true;
                //    selectableItem = hit.transform.gameObject.GetComponent<Item>();
                //    selectableName.text = selectableItem.item.itemName;
                //    reticle.color = Color.green;
                //    if (Input.GetButtonDown("Interactable"))
                //    {
                //        if (playerInventoryScript.AddItem(selectableItem.item, 1, Random.Range(1, selectableItem.item.maxStack)))
                //        {
                //            itemAddedUIScript.ItemAdded(selectableItem.item.itemName, selectableItem.amount, selectableItem.item.itemIcon);
                //            Destroy(selectableItem.gameObject);
                //        }
                //    }
                //    break;
                //case "Loot":
                //    isLookingAtSelectable = true;
                //    lootObject = hit.transform.gameObject.GetComponent<Loot>();
                //    selectableName.text = lootObject.lootName;
                //    lootName = lootObject.lootName;
                //    reticle.color = Color.green;
                //    if (Input.GetButtonDown("Interactable") && !itemContainer.isOpenedLoot && !lootObject.isLooted)
                //    {
                //        StartCoroutine(Searching(1.9f));
                //    }
                //    if (Input.GetButtonDown("Interactable") && !itemContainer.isOpenedLoot && !lootIsOpened && lootObject.isLooted)
                //    {
                //        OpenLootedContainer();
                //    }
                //    break;
                //case "WoodResource":
                //    treeObject = hit.transform.gameObject.GetComponent<Tree>();
                //    selectableName.text = treeObject.treeName;
                //    reticle.color = Color.green;
                //    if (Input.GetMouseButton(0) && equipmentManager.isAxeEquipped)
                //    {
                //        //Debug.Log("Chopping!");
                //        if ((int)Time.time > lastTimeInt && treeObject.treeHealth >= 0)
                //        {
                //            treeObject.treeHealth -= 10;
                //            playerInventoryScript.AddItem(treeObject.wood, 1, 0);
                //            itemAddedUIScript.ItemAdded(treeObject.wood.itemName, 1, treeObject.wood.itemIcon);
                //            if (treeObject.treeHealth <= 0)
                //            {
                //                Destroy(treeObject.gameObject);
                //                Debug.Log("Tree has been chopped down!");
                //            }
                //            Debug.Log("Added 1x wood!");
                //        }
                //        lastTimeInt = (int)Time.time;
                //    }
                //    break;
                default:
                    isLookingAtSelectable = false;
                    selectableName.text = "";
                    reticle.color = Color.white;
                    break;
            }
        }
    }
}
