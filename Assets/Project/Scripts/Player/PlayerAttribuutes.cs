using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 20f;   // Units per second while sprinting
    [SerializeField] private float staminaRegenRate = 15f;   // Units per second while resting
    [SerializeField] private float jumpingStaminaDrainRate = 10f; // How much stamina is drained on a jump
    [SerializeField] private float regenDelay = 2.5f;        // Seconds to wait before regen starts
    [SerializeField] private float currentStamina;

    private float regenTimer;

    // Public read-only getters for UI or other scripts
    // like get functions
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;
    public bool HasStamina => currentStamina > 0.5f;
    public float StaminaLossOnJump => jumpingStaminaDrainRate;

    private PlayerController playerController;

    private void Awake()
    {
        // construct max health and stamina
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        HandleStaminaRegeneration();
    }

    /// <summary>
    /// Drains stamina while sprinting. Call this every frame during active sprint.
    /// </summary>
    public void DrainStaminaForSprint()
    {
        currentStamina = Mathf.Max(currentStamina - staminaDrainRate * Time.deltaTime, 0f);
        regenTimer = regenDelay; // Reset regen timer while actively using stamina
    }

    public void DrainStaminaForJump ()
    {
        currentStamina = Mathf.Max(currentStamina - jumpingStaminaDrainRate, 0f);
        regenTimer = regenDelay; // Reset regen timer while actively using stamina
    }
    /// <summary>
    /// 
    /// </summary>
    private void HandleStaminaRegeneration()
    {
        // keep resetting delay if moving
        if (playerController != null && playerController.IsMoving)
        {
            regenTimer = regenDelay;
            return;
        }

        // if the timer is greater than 0 decrease the timer by the time
        if (regenTimer > 0f)
        {
            regenTimer -= Time.deltaTime;
            return;
        }
        // regain stamina
        if (currentStamina < maxStamina && (!playerController || !playerController.IsMoving))
        {
            currentStamina = Mathf.Min(currentStamina + staminaRegenRate * Time.deltaTime, maxStamina);
            Debug.Log("Stamina: " + currentStamina);
        }
    }
    // take damage
    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        if (currentHealth <= 0f)
        {
            // Player death logic here
            Debug.Log("Player is out of health!");
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}