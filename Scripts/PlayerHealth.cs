using UnityEngine;
using TMPro; // Make sure you are using TextMeshPro for crisp text

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI Reference")]
    // Drag your TextMeshPro object here in the Inspector
    public TextMeshProUGUI healthText;

    void Start()
    {
        // Start full health
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // You can add scene reload logic here later
            Debug.Log("PLAYER DIED");
        }

        UpdateUI();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        // Cap health at max (Serious Sam style usually allows overcharge, 
        // but let's stick to 100 for now)
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();

            // Color Logic: Red if low, White if normal
            if (currentHealth <= 30) healthText.color = Color.red;
            else healthText.color = Color.white;
        }
    }
}