﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] int Damage = 10;
    [SerializeField] float KnockbackForce = 3f;
    [SerializeField] float TargetStunTime = 0.5f;
    [SerializeField] float SelfStunTime = 0.5f;
    [SerializeField] bool ToMonster = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        
    }
}
