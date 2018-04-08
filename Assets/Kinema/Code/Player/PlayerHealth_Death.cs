using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth_Death : MonoBehaviour
{

    public event Action OnPlayerDeath = delegate { };
    PlayerHealth_Damage collisionDamage;

    private void Start()
    {
        collisionDamage = GetComponent<PlayerHealth_Damage>();
        collisionDamage.OnNodeDamaged += CheckNodeDamage;
    }

    private void CheckNodeDamage(CharacterNode node)
    {
        if (node.DamageCurrent >= node.DamageMax)
            Die();
    }
    public void Die()
    {
        OnPlayerDeath();
    }
}
