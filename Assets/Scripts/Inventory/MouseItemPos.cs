using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseItemPos : MonoBehaviour
{
    private ItemSlot mouseItemSlot;

    void Start()
    {
        mouseItemSlot = GetComponent<UIItemSlot>().itemSlot;
    }

    void LateUpdate()
    {
        if (mouseItemSlot.item)
        {
            transform.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
        }
    }
}
