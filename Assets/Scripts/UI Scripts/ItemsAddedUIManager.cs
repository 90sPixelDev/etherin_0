using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemsAddedUIManager : MonoBehaviour
{
    public GameObject itemAddedBanner;
    public ItemAddedUI itemAddedUI;
    public Animator anim;
    public int currentAnimCount = 0;
    public int lastAnimCount = 0;

    public bool canPlayAnim = true;

    public void ItemAdded(string _name, int _amount, Sprite _icon)
    {
        var itemBanner = Instantiate(itemAddedBanner, transform);
        itemAddedUI = itemBanner.GetComponent<ItemAddedUI>();
        anim = itemBanner.GetComponent<Animator>();

        itemAddedUI.itemAddedInfo.text = string.Format("{0} x{1}", _name, _amount.ToString());
        itemAddedUI.itemAddedIcon.sprite = _icon;

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var anchorPos = itemBanner.GetComponent<RectTransform>().anchoredPosition;
                itemBanner.GetComponent<RectTransform>().anchoredPosition = new Vector2(anchorPos.x, anchorPos.y + (40 * (i + 1)));
            }
        }

        anim.Play("ItemAdded2");
    }
}
