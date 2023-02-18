using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseItemPos : MonoBehaviour
{
    private ItemSlot mouseItemSlot;

    void Start()
    {
        mouseItemSlot = GetComponent<ItemSlot>();
    }

    void LateUpdate()
    {
        if (mouseItemSlot.item)
        {
            Debug.Log("Has Item!");
            transform.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
        }
    }
}
