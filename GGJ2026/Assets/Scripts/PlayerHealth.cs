using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        var shield = GetComponent<PlayerShield>();
        if (shield != null)
            amount = shield.ModifyDamage(amount);



        currentHealth -= amount;

        Debug.Log("Player Health"+currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Destroy(gameObject);
        //FindObjectOfType<Score>()?.SaveBestScore();
        //SceneManager.LoadScene("GameOverScreen");
        FindObjectOfType<Score>()?.SaveBestScore();
        SceneManager.LoadScene("GameOverScreen");
    }
}
