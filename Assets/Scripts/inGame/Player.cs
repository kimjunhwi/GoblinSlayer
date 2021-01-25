using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum E_ATTACK_TYPE
    {
        E_MELEE,
        E_RANGE,
    }

    [SerializeField] Animator m_Animator;
    [SerializeField] Rigidbody2D m_Rigidbody;
    [SerializeField] Transform m_Attack_Direction;
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] float m_Speed = 2.0f;
    bool isFlipX = false;
    Vector2 m_Direction = Vector2.right;
    bool isStun = false;

    Vector2 inputDirection = Vector2.zero;

    public int Health {get; private set;}
    public float MoveSpeed {get; private set;}
    public int Enhance { get; private set;}

    public void Move(Vector2 _inputDirection)
    {
        inputDirection = _inputDirection;

        if(inputDirection.x !=0)
        {
            if(isFlipX!=(inputDirection.x < 0))
                FlipX(isFlipX = inputDirection.x < 0);
        }
        
        m_Animator.SetFloat("Vector_Y", inputDirection.y);
        m_Animator.SetFloat("Vector_X", inputDirection.x);

        inputDirection.Normalize();
    }

    void FixedUpdate()
    {
        if(!isStun)
        {
            m_Rigidbody.velocity = inputDirection * m_Speed;
        }
    }

    // void TryAttackClub0()
    // {
    //     if(Input.GetButtonDown("Fire1"))
	// 	{
	// 		m_Animator.SetTrigger("Attack_Club_0");
	// 	}
    // }

    void FlipX(bool isFlip)
    {
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
