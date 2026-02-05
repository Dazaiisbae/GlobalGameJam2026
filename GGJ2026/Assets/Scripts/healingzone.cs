using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Collections.Generic;

public class HealingZone : MonoBehaviour
{
    [Header("Healing")]
    [Range(0f, 0.2f)]
    public float healPercentPerSecond = 0.02f; // 2% per second at full strength

    [Range(0f, 1f)]
    public float healCapPercent = 0.6f; // won’t heal above 60%

    [Header("Diminishing Returns")]
    public float timeToDecay = 3f;      // seconds until weakest healing
    [Range(0f, 1f)]
    public float minMultiplier = 0.25f; // 25% strength at worst

    public string playerTag = "Player";

    [Header("Visual Feedback")]
    [Range(0f, 1f)]
    public float currentHealingStrength; // read-only, for VFX

    // Tracks how long each player has stayed inside
    private Dictionary<int, float> timeInside = new Dictionary<int, float>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        timeInside[other.GetInstanceID()] = 0f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        timeInside.Remove(other.GetInstanceID());
        currentHealingStrength = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        var health = other.GetComponent<PlayerHealth>() ?? other.GetComponentInParent<PlayerHealth>();
        if (health == null) return;

        int id = other.GetInstanceID();
        if (!timeInside.ContainsKey(id))
            timeInside[id] = 0f;

        timeInside[id] += Time.deltaTime;

        float cap = health.maxHealth * healCapPercent;
        if (health.currentHealth >= cap) return;

        // Fade healing strength over time
        float t = Mathf.Clamp01(timeInside[id] / timeToDecay);
        float multiplier = Mathf.Lerp(1f, minMultiplier, t);

        float healPerSec = health.maxHealth * healPercentPerSecond * multiplier;
        float healThisFrame = healPerSec * Time.deltaTime;

        float newHealth = Mathf.Min(cap, health.currentHealth + healThisFrame);
        health.Heal(newHealth - health.currentHealth);

        currentHealingStrength = multiplier;
    }
}
