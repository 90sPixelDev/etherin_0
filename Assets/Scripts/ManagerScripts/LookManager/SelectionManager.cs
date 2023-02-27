using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Unity.VisualScripting;

public class SelectionManager : NetworkBehaviour
{
    private Camera cam;
    public NetworkObject selectable;

    [Header("Reticle")]
    public TextMeshProUGUI selectableText;
    public Image reticle;

    [Header("Focus Info")]
    public bool isLookingAtSelectable = false;
    public SelectableItemFocusType selectableItemFocusType;

    public enum SelectableItemFocusType
    {
        NONE,
        ITEM,
        LOOT
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();

        selectableText = GameObject.FindGameObjectWithTag("PointerUI").transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        reticle = GameObject.FindGameObjectWithTag("PointerUI").transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();

        selectableText.text = "";
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        isLookingAtSelectable = false;
        reticle.color = Color.white;
        selectableText.text = "";
        selectableItemFocusType = SelectableItemFocusType.NONE;

        if (Physics.Raycast(ray, out hit, 6.0f))
        {
            switch (hit.transform.gameObject.tag)
            {
                case "item":
                    isLookingAtSelectable = true;
                    selectable = hit.transform.GetComponent<NetworkObject>();
                    selectableText.text = selectable.GetComponent<WorldItem>().item.itemName;
                    reticle.color = Color.green;
                    selectableItemFocusType = SelectableItemFocusType.ITEM;
                    break;
                case "loot":
                    isLookingAtSelectable = true;
                    selectable = hit.transform.GetComponent<NetworkObject>();
                    selectableText.text = selectable.GetComponent<LootNetwork>().lootName;
                    reticle.color = Color.green;
                    selectableItemFocusType = SelectableItemFocusType.LOOT;
                    break;
                default:
                    isLookingAtSelectable = false;
                    selectable = null;
                    selectableText.text = "";
                    reticle.color = Color.white;
                    selectableItemFocusType = SelectableItemFocusType.NONE;
                    break;
            }
        }
    }
}
