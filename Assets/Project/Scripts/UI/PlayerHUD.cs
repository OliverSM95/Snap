using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro

public class PlayerHUD : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerAttributes playerAttributes;
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Bars")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;

    [Header("Stats")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text streetCredText;

    [Header("Hotbar")]
    [SerializeField] private Image[] hotbarBackgrounds = new Image[5];
    [SerializeField] private Image[] hotbarItemIcons = new Image[5];
    [SerializeField] private Color selectedColor = Color.yellow;
    [SerializeField] private Color unselectedColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);

    private void Start()
    {
        if (playerAttributes == null) playerAttributes = FindObjectOfType<PlayerAttributes>();
        if (playerInventory == null) playerInventory = FindObjectOfType<PlayerInventory>();
    }

    private void Update()
    {
        if (playerAttributes == null || playerInventory == null) return;

        UpdateBars();
        UpdateStatsText();
        UpdateHotbarUI();
    }

    private void UpdateBars()
    {
        if (healthSlider != null)
            healthSlider.value = playerAttributes.CurrentHealth / playerAttributes.MaxHealth;

        if (staminaSlider != null)
            staminaSlider.value = playerAttributes.CurrentStamina / playerAttributes.MaxStamina;
    }

    private void UpdateStatsText()
    {
        if (moneyText != null)
            moneyText.text = $"${playerAttributes.Money}";

        if (streetCredText != null)
            streetCredText.text = $"Cred: {playerAttributes.StreetCred}";
    }

    private void UpdateHotbarUI()
    {
        for (int i = 0; i < hotbarBackgrounds.Length; i++)
        {
            // Highlight the currently selected slot
            if (hotbarBackgrounds[i] != null)
            {
                hotbarBackgrounds[i].color = (i == playerInventory.selectedSlotIndex) ? selectedColor : unselectedColor;
            }

            // Display the item icon if one exists, otherwise hide the icon image
            if (hotbarItemIcons[i] != null)
            {
                Sprite itemSprite = playerInventory.slots[i];
                if (itemSprite != null)
                {
                    hotbarItemIcons[i].sprite = itemSprite;
                    hotbarItemIcons[i].enabled = true;
                }
                else
                {
                    hotbarItemIcons[i].enabled = false;
                }
            }
        }
    }
}