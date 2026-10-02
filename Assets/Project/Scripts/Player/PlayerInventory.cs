using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Slots")]
    // Using Sprite to represent items visually for now. 
    // In a full game, this would be a custom Item class or ScriptableObject.
    public Sprite[] slots = new Sprite[5];
    public int selectedSlotIndex = 0;

    private void Update()
    {
        HandleSlotSelection();
    }

    private void HandleSlotSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedSlotIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedSlotIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedSlotIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedSlotIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) selectedSlotIndex = 4;
    }

    /// <summary>
    /// Adds an item to the first empty slot. Returns true if successful.
    /// </summary>
    public bool AddItem(Sprite itemIcon)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = itemIcon;
                return true;
            }
        }
        return false; // Inventory is full
    }
}