using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class PlayerShield : MonoBehaviour
{
    [Header("Shield Settings")]
    public float duration = 3f;
    public float cooldown = 12f;

    [Tooltip("Multiply incoming damage by this while shielded. 0.3 = 70% reduced")]
    public float damageMultiplier = 0.3f;

    [Header("Input")]
    public KeyCode shieldKey = KeyCode.E;

    public bool IsActive { get; private set; }
    public bool IsOnCooldown { get; private set; }
    public float CooldownRemaining { get; private set; }

    float durationRemaining;

    [Header("Visuals")]
    public Image shieldOverlay;
    public float overlayAlpha = 0.25f;
    public float overlayFadeSpeed = 8f;

    [Header("Shield Text")]
    public TextMeshProUGUI shieldText;
    public float textFadeSpeed = 10f;

    //Shield cooldown
    public TextMeshProUGUI cooldownText;
    public float cooldownTextFadeSpeed = 10f;

    void Update()
    {
        // Activate
        if (Input.GetKeyDown(shieldKey))
            TryActivate();

        // Tick active shield
        if (IsActive)
        {
            durationRemaining -= Time.deltaTime;
            if (durationRemaining <= 0f)
                EndShield();
        }

        // Tick cooldown
        if (IsOnCooldown)
        {
            CooldownRemaining -= Time.deltaTime;
            if (CooldownRemaining <= 0f)
            {
                CooldownRemaining = 0f;
                IsOnCooldown = false;
            }
        }
        if (shieldOverlay != null)
        {
            float targetA = IsActive ? overlayAlpha : 0f;
            Color c = shieldOverlay.color;
            c.a = Mathf.MoveTowards(c.a, targetA, Time.deltaTime * overlayFadeSpeed);
            shieldOverlay.color = c;
        }
        if (shieldText != null)
        {
            float targetA = IsActive ? 1f : 0f;
            Color t = shieldText.color;
            t.a = Mathf.MoveTowards(t.a, targetA, Time.deltaTime * textFadeSpeed);
            shieldText.color = t;
        }



        //Shield Cooldown
        if (cooldownText != null)
        {
            bool show = IsOnCooldown && CooldownRemaining > 0f;

            // Update timer text
            if (show)
                cooldownText.text = "Shield Cooldown: " + CooldownRemaining.ToString("0.0") + "s";

            // Fade in/out
            float targetA = show ? 1f : 0f;
            Color c = cooldownText.color;
            c.a = Mathf.MoveTowards(c.a, targetA, Time.deltaTime * cooldownTextFadeSpeed);
            cooldownText.color = c;
        }
    }

    public void TryActivate()
    {
        if (IsActive || IsOnCooldown) return;

        IsActive = true;
        durationRemaining = duration;

        // Optional: visual / sound hook
        // Debug.Log("Shield ON");

        var stealth = GetComponent<PlayerStealth>();
        if (stealth != null && stealth.hiddenInGrass) return;

    }

    void EndShield()
    {
        IsActive = false;

        IsOnCooldown = true;
        CooldownRemaining = cooldown;

        // Optional: visual / sound hook
        // Debug.Log("Shield OFF, cooldown started");
    }

    // Call this from PlayerHealth to modify incoming damage
    public float ModifyDamage(float incomingDamage)
    {
        if (!IsActive) return incomingDamage;
        return incomingDamage * damageMultiplier;
    }
}
