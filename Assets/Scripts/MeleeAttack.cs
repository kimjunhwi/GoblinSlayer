using System.Collections;
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
        // Debug.Log(string.Format("trigger {0}", col.gameObject.name));
        // col.attachedRigidbody.AddForce()
        Vector3 Direaction = (col.transform.position - transform.position).normalized;
        if(ToMonster)
        {
            col.GetComponent<Monster_Controller>().BeAttacked(Damage, Direaction, KnockbackForce, TargetStunTime);
            // GetComponentInParent<Character_Controller>().Stun(SelfStunTime);
        }else
        {
            col.GetComponent<Character_Controller>().BeAttacked(Damage, Direaction, KnockbackForce, TargetStunTime);
            GetComponentInParent<Monster_Controller>()._Stun(SelfStunTime);
        }
    }
}
