using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Controller : MonoBehaviour
{
    [SerializeField] Animator m_Animator;
    [SerializeField] Rigidbody2D m_Rigidbody;
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] float m_Speed = 2.0f;
    [SerializeField] Transform m_Target;
    [SerializeField] float m_StopDistance = 0.1f;
    [SerializeField] float m_Hp = 10;
    [SerializeField] bool isStun;
    void Update()
    {
        if(isStun)
        {
            return;
        }
        TryMove();
        TryDie();
    }
    void TryMove()
    {
        Vector3 dir = m_Target.position - transform.position;
        // Debug.Log(string.Format("{0:F4} {1:F4}", dir.sqrMagnitude, m_StopDistance * m_StopDistance));
        if(dir.sqrMagnitude > m_StopDistance * m_StopDistance)
        {
            m_Rigidbody.velocity = dir.normalized * m_Speed;
        }else
        {
            m_Rigidbody.velocity = Vector3.zero;
        }
    }
    void TryDie()
    {
        if(m_Hp <= 0)
        {
            m_Animator.SetBool("Die", true);
            m_Rigidbody.velocity = Vector3.zero;
            this.enabled = false;
            Destroy(gameObject, 1f);
        }
    }

    public void _Stun(float sec)
    {
        if(!isStun)
        {
            StartCoroutine(Stun(sec));
        }
    }

    IEnumerator Stun(float sec)
    {
        isStun = true;
        m_Rigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(sec);
        isStun = false;
    }

    public void BeAttacked(int Damage, Vector2 Direaction, float KnockbackForce = 1f, float StunTime = 1f)
    {
        if(!isStun)
        {
            StartCoroutine(Stun(StunTime));
            // m_Animator.SetTrigger("Hit");
            m_Rigidbody.AddForce(Direaction * KnockbackForce, ForceMode2D.Impulse);
        }
    }
}
