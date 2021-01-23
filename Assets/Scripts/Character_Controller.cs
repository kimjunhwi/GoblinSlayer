using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    [SerializeField] Animator m_Animator;
    [SerializeField] Rigidbody2D m_Rigidbody;
    [SerializeField] Transform m_Attack_Direction;
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] float m_Speed = 2.0f;
    bool isFlipX = false;
    Vector2 m_Direction = Vector2.right;
    bool isStun = false;
    void Update()
    {
        if(!isStun)
        {
            TryMove();
            TryAttackClub0();
        }
    }

    void TryMove()
    {
        // if(!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Character_Idle")
        //     &&!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Character_Run"))
        // {
        //     return;
        // }
        float Vector_Y = Input.GetAxis("Vertical");
        float Vector_X = Input.GetAxis("Horizontal");
        if(Vector_X!=0)
        {
            if(isFlipX!=(Vector_X < 0))
                FlipX(isFlipX = Vector_X < 0);
        }
        if(Vector_X!=0 || Vector_Y !=0)
        {
            // if(!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Character_Attack_Club"))
            {
                m_Direction = new Vector2(Vector_X, Vector_Y).normalized;
                Debug.DrawLine(transform.position, transform.position + new Vector3(m_Direction.x, m_Direction.y), Color.red, 1.0f);
                float rotation_z = Mathf.Atan2(m_Direction.y, m_Direction.x) * Mathf.Rad2Deg;
                m_Attack_Direction.localRotation = Quaternion.Euler(0,0,rotation_z);
            }
        }
        m_Animator.SetFloat("Vector_Y", Vector_Y);
        m_Animator.SetFloat("Vector_X", Vector_X);

        m_Rigidbody.velocity = new Vector2(Vector_X, Vector_Y) * m_Speed;
    }

    void TryAttackClub0()
    {
        if(Input.GetButtonDown("Fire1"))
		{
			m_Animator.SetTrigger("Attack_Club_0");
		}
    }

    void FlipX(bool isFlip)
    {
        // gameObject.transform.localScale = new Vector3(isFlipX ? -1 : 1, 1, 1);
        m_SpriteRenderer.flipX = isFlip;
    }

    public void BeAttacked(int Damage, Vector2 Direaction, float KnockbackForce = 1f, float StunTime = 1f)
    {
        if(!isStun)
        {
            StartCoroutine(Stun(StunTime));
            m_Animator.SetTrigger("Hit");
            m_Rigidbody.AddForce(Direaction * KnockbackForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator Stun(float sec)
    {
        isStun = true;
        // m_Animator.SetBool("Hit", true);
        yield return new WaitForSeconds(sec);
        // m_Animator.SetBool("Hit", false);
        isStun = false;
    }
}
