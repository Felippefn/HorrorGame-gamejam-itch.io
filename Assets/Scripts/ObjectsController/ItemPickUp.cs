using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemType itemType;
    public GameObject worldModel;

    public void Pickup(PlayerItemsManager playerItems)
    {
        playerItems.PickupItem(itemType);
        worldModel.SetActive(false);
        gameObject.SetActive(false);
    }
}
