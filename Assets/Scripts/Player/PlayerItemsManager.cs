using UnityEngine;
using UnityEngine.UI;

public class PlayerItemsManager : MonoBehaviour
{
    [Header("Items no Player")]
    public GameObject flashlight;
    public GameObject cameraItem;
    public GameObject droneItem;

    [Header("UI")]
    public Text activeItemText;

    private ItemType? currentItem = null;

    void Start()
    {
        DisableAll();
        UpdateUI(null);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(ItemType.Flashlight);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Equip(ItemType.Camera);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Equip(ItemType.Drone);
    }

    public void PickupItem(ItemType item)
    {
        GetItemObject(item).SetActive(true);
        Equip(item);
    }

    void Equip(ItemType item)
    {
        DisableAll();
        GetItemObject(item).SetActive(true);
        currentItem = item;

        var flashlightCtrl = GetItemObject(item).GetComponent<FlashlightController>();
        if (flashlightCtrl != null)
            flashlightCtrl.TurnOff();

        UpdateUI(item);
    }

    void DisableAll()
    {
        flashlight.SetActive(false);
        cameraItem.SetActive(false);
        droneItem.SetActive(false);
    }

    void UpdateUI(ItemType? item)
    {
        if (activeItemText == null) return;

        activeItemText.text = item == null
            ? ""
            : $"Item: {item}";
    }

    GameObject GetItemObject(ItemType item)
    {
        return item switch
        {
            ItemType.Flashlight => flashlight,
            ItemType.Camera => cameraItem,
            ItemType.Drone => droneItem,
            _ => null
        };
    }
}
