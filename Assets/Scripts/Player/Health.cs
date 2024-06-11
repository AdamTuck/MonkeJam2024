using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [Header("Health Attributes")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float minHealth;

    [Header("Actions")]
    public Action<float> OnHealthUpdated;
    public Action OnDeath;

    public bool isDead { get; private set; }
    private float health;

    void Start()
    {
        health = maxHealth;
        //OnHealthUpdated(health);
    }

    public void DeductHealth(float amountToDeduct)
    {
        if (isDead) return;

        health -= amountToDeduct;

        if (health <= 0)
        {
            isDead = true;
            OnDeath();
            health = 0;
        }

        //OnHealthUpdated(health);
    }

    public void RespawnPlayer ()
    {
        isDead = false;
        health = maxHealth;
    }
}