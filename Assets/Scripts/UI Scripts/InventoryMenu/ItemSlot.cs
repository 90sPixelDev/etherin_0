using TMPro;
using Unity.Netcode;
using UnityEngine.UI;

public class ItemSlot : NetworkBehaviour
{
    public ItemObject item;

    public Image itemIcon;

    public int itemAmt;
    public TextMeshProUGUI itemAmtText;

    public int currentCondition;
    public Image conditionBar;

    private void Start()
    {
        UpdateItemInfo();
    }

    public void UpdateItemInfo()
    {
        if (item != null)
        {
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = item.itemIcon;
            if (itemAmt > 1)
            {
                itemAmtText.gameObject.SetActive(true);
                itemAmtText.text = itemAmt.ToString();
            }

            if (currentCondition > 1)
            {
                conditionBar.gameObject.SetActive(item.isDegradable);
                var condition = item.maxCondition / currentCondition;
                conditionBar.fillAmount = condition;
            }
        }
    }
}
