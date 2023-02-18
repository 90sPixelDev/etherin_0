using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarSelector : MonoBehaviour
{
    public GameObject[] hotBarSlots = new GameObject[9];
    public Button selectedItem;
    public int selectedSlot = 0;
    public UIItemSlot selectedUIItemSlot;

    public GameObject itemToEquip;
    //public ItemSlot heldItemSlot;
    public Transform itemPos;
    public MenuManager menuManager;
    //public EquipmentManager equipmentManager;

    void Start()
    {
        itemPos = GameObject.Find("Hold_R").transform;

        selectedSlot = 0;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            hotBarSlots[i] = this.transform.GetChild(i).gameObject;
        }
        selectedItem = hotBarSlots[selectedSlot].GetComponent<Button>();
        selectedUIItemSlot = hotBarSlots[selectedSlot].GetComponent<UIItemSlot>();
        selectedItem.Select();
    }

    //void Update()
    //{
    //    if (Input.GetAxis("Mouse ScrollWheel") < 0f && !menuManager.inMenu)
    //    {
    //        selectedSlot++;
    //        if (selectedSlot < 9)
    //        {
    //            selectedItem = hotBarSlots[selectedSlot].GetComponent<Button>();
    //            selectedItem.Select();
    //        }
    //        else
    //        {
    //            selectedSlot = 0;
    //            selectedItem = hotBarSlots[selectedSlot].GetComponent<Button>();
    //            selectedItem.Select();
    //        }
    //        selectedUIItemSlot = hotBarSlots[selectedSlot].GetComponent<UIItemSlot>();
    //        if (selectedUIItemSlot.itemSlot.hasItem)
    //        {
    //            UnequipItem();
    //            Debug.Log("There is an equipable item!");
    //            heldItemSlot = selectedUIItemSlot.itemSlot;
    //            itemToEquip = selectedUIItemSlot.itemSlot.item.itemPrefab;
    //            EquipItem(heldItemSlot, itemToEquip);
    //        }
    //        else
    //        {
    //            itemToEquip = null;
    //            UnequipItem();
    //        }
    //    }
    //    if (Input.GetAxis("Mouse ScrollWheel") > 0f && !menuManager.inMenu)
    //    {
    //        selectedSlot--;
    //        if (selectedSlot > -1)
    //        {
    //            selectedItem = hotBarSlots[selectedSlot].GetComponent<Button>();
    //            selectedItem.Select();

    //        }
    //        else
    //        {
    //            selectedSlot = 8;
    //            selectedItem = hotBarSlots[selectedSlot].GetComponent<Button>();
    //            selectedItem.Select();
    //        }
    //        selectedUIItemSlot = hotBarSlots[selectedSlot].GetComponent<UIItemSlot>();
    //        if (selectedUIItemSlot.itemSlot.hasItem)
    //        {
    //            UnequipItem();
    //            Debug.Log("There is an equipable item!");
    //            heldItemSlot = selectedUIItemSlot.itemSlot;
    //            itemToEquip = selectedUIItemSlot.itemSlot.item.itemPrefab;
    //            EquipItem(heldItemSlot, itemToEquip);
    //        }
    //        else
    //        {
    //            itemToEquip = null;
    //            UnequipItem();
    //        }
    //    }
    //}

    //public void EquipItem(ItemSlot itemSlot, GameObject item)
    //{
    //    itemToEquip = Instantiate(item, itemPos);

    //    if (itemSlot.item.itemType == ItemType.Tool)
    //    {
    //        var toolItem = (ToolObject)itemSlot.item;
    //        Debug.Log("Equipped Tool!");
    //        itemToEquip.transform.localRotation = Quaternion.Euler(61, -90, 0);
    //        itemToEquip.transform.localPosition = new Vector3(-0.25f, -0.1f, 0);
    //        //itemEquiped.transform.rotation = Quaternion.Euler(itemPos.rotation.x, (itemPos.rotation.y - 10), itemPos.rotation.z);
    //        itemToEquip.transform.localScale += new Vector3(1, 1, 1);
    //        if (toolItem.toolType == ToolType.AXE)
    //        {
    //            equipmentManager.isAxeEquipped = true;
    //            equipmentManager.isPickaxeEquipped = false;
    //            equipmentManager.isShovelEquipped = false;
    //        }
    //    }
    //    if (itemSlot.item.itemType == ItemType.Weapon)
    //    {
    //        Debug.Log("Equipped Weapon!");
    //        itemToEquip.transform.localRotation = Quaternion.Euler(-20, -90, 0);
    //        itemToEquip.transform.localPosition = new Vector3(-.15f, -.075f, 0f);
    //        //itemEquiped.transform.rotation = Quaternion.Euler(itemPos.rotation.x, (itemPos.rotation.y - 10), itemPos.rotation.z);
    //        itemToEquip.transform.localScale += new Vector3(1, 1, 1);
    //    }
    //    if (itemSlot.item.itemType == ItemType.Potion)
    //    {
    //        Debug.Log("Equipped Potion!");
    //        itemToEquip.transform.localRotation = Quaternion.Euler(-20, -90, 0);
    //        //itemToEquip.transform.localPosition = new Vector3(-.15f, -.075f, 0f);
    //        //itemEquiped.transform.rotation = Quaternion.Euler(itemPos.rotation.x, (itemPos.rotation.y - 10), itemPos.rotation.z);
    //        //itemToEquip.transform.localScale += new Vector3(1, 1, 1);
    //    }

    //}
    //public void UnequipItem()
    //{
    //    equipmentManager.isAxeEquipped = false;
    //    equipmentManager.isPickaxeEquipped = false;
    //    equipmentManager.isShovelEquipped = false;

    //    foreach (Transform itemHeld in itemPos)
    //    {
    //        Destroy(itemHeld.gameObject);
    //    }
    //}
}
