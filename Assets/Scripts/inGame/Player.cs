using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    #region  구르기

    float rollMoveSpeed = 10;
	float RollMoveTimer = 0.5f;

	float RollMoveMaxChargeTime = 1f;
	float RollMoveChargeTimer = 0f;

    public bool IsRollMoving { get; private set; } = false;
	Vector3 RollMoveDir = Vector3.zero;

	public event Action OnRollMovingStart = null;
	public event Action OnRollMovingEnd = null;

    Coroutine RollMoveCoroutine = null;

    #endregion

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
        bool isMove = inputDirection.x != 0 || inputDirection.y != 0;
		//ChangeAnimState(); //isMove ? BodyState.Move : BodyState.Idle);
    }

    public void UpdateRollMoving()
	{
		if (IsRollMoving == true)
			return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (RollMoveCoroutine != null)
				StopCoroutine(RollMoveCoroutine);
            
			RollMoveCoroutine = StartCoroutine(RollMoveProcess());
		}
	}

	IEnumerator RollMoveProcess()
	{
		IsRollMoving = true;
		RollMoveDir = inputDirection;

        m_Animator.SetTrigger("roll");
		float timer = RollMoveTimer;
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			transform.position += RollMoveDir * rollMoveSpeed * Time.deltaTime;
			yield return null;
		}
		IsRollMoving = false;
	}

    void FixedUpdate()
    {
        if(!isStun || IsRollMoving)
        {
            m_Rigidbody.velocity = inputDirection * m_Speed;
        }
    }

    // void ChangeAnimState(BodyState _bodyState)
	// {
	// 	if (bodyState == _bodyState)
	// 		return;

	// 	bodyState = _bodyState;

	// 	string stateName = string.Empty;
	// 	switch (bodyState)
	// 	{
	// 		case BodyState.Idle: stateName = "Player_Idle"; break;
	// 		case BodyState.Move: stateName = "Player_Move"; break;
	// 		case BodyState.Die: stateName = "Player_Die"; break;
	// 	}

	// 	bodyAnimator.Play(stateName);		
	// }

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
        if(!isStun || IsRollMoving)
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
