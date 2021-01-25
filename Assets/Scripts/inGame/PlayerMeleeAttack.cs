using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] int Damage = 10;
    [SerializeField] float KnockbackForce = 3f;
    [SerializeField] float TargetStunTime = 0.5f;
    [SerializeField] float SelfStunTime = 0.5f;
    [SerializeField] bool ToMonster = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        //var Enemy = inGameManager.Instance.Enemies.Equals(col.gameObject.GetInstanceID());
          
        //if(Enemy != null)
        //{
            Vector2 Direction = (Vector2)(col.transform.position - transform.position).normalized;
            col.GetComponent<Monster_Controller>().BeAttacked(Damage, Direction, KnockbackForce, TargetStunTime);
            //Enemy.BeAttacked(Damage, Direction, KnockbackForce, TargetStunTime);
        //}
    }
}
