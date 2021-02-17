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

    [SerializeField]
    Sprite weaponSprite;
    [SerializeField]
    PlayerMeleeAttack meleeAttack;


    #region 공격 처리 관련

    float fAttackSpeed = 2f;

    [SerializeField]
    float fCurrentAttackSpeed = 0f;

    [SerializeField]
    GameObject targetEnemyObject = null;

    #endregion

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

    //나중에 Init든 뭐든 초기화로 수정할 예정
    void Start()
    {
        PlayerMeleeAttack.WeaponParam p = new PlayerMeleeAttack.WeaponParam();

        p.Damage = 10;
        p.knockbackForce =3;
        p.targetSturnTime = 1;
        p.WeaponSprite = weaponSprite;

        meleeAttack.InitWeapon(p);
    }

    void Update()
    {
        if(targetEnemyObject != null)
        {
            var direction = (targetEnemyObject.transform.position - transform.position).normalized;
            float rotation_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            m_Attack_Direction.localRotation = Quaternion.Euler(0,0,rotation_z);
        }

        if(targetEnemyObject != null && fCurrentAttackSpeed <= 0)
        {
            fCurrentAttackSpeed = fAttackSpeed;
            m_Animator.SetTrigger("Attack_Club_0");
        }
        else  if(fCurrentAttackSpeed > 0)
        {
            fCurrentAttackSpeed -= Time.deltaTime;
        }
    }

    public void Move(Vector2 _inputDirection)
    {
        inputDirection = _inputDirection;

        if(inputDirection.x !=0)
        {
            if(isFlipX!=(inputDirection.x < 0))
                FlipX(isFlipX = inputDirection.x < 0);
        }
        if(inputDirection.x!=0 || inputDirection.y !=0)
        {
            m_Direction = new Vector2(inputDirection.x, inputDirection.y).normalized;
            float rotation_z = Mathf.Atan2(m_Direction.y, m_Direction.x) * Mathf.Rad2Deg;
            m_Attack_Direction.localRotation = Quaternion.Euler(0,0,rotation_z);
        }
        
        m_Animator.SetFloat("Vector_Y", inputDirection.y);
        m_Animator.SetFloat("Vector_X", inputDirection.x);

        inputDirection.Normalize();
        bool isMove = inputDirection.x != 0 || inputDirection.y != 0;
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

    public void TryAttackClub0()
    {
		m_Animator.SetTrigger("Attack_Club_0");
    }

    public void EndAttackMotion()
    {
        meleeAttack.InitWeapon();
    }

    void OnTriggerEnter2D( Collider2D other )
    {
        if(other.tag.Contains("Monster"))
        {
            if(targetEnemyObject == null)
                targetEnemyObject = other.gameObject;
        }
    }

    void OnTriggerExit2D( Collider2D other)
    {
        if(other.gameObject == targetEnemyObject)
            targetEnemyObject = null;
    }

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
