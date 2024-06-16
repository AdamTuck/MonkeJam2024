using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Health : MonoBehaviour
{
    [Header("Health Attributes")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float minHealth;
    [SerializeField] private bool isPlayerHealth;

    [Header("Actions")]
    public UnityAction<float> OnHealthUpdated;
    public UnityAction OnDeath;

    public bool isDead { get; private set; }
    private float health;

    void Start()
    {
        health = maxHealth;
        OnHealthUpdated(health);
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

        OnHealthUpdated(health);
    }

    public void RespawnPlayer ()
    {
        isDead = false;
        health = maxHealth;
    }
}