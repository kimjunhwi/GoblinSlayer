using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    WeaponParam param;

    [SerializeField]
    SpriteRenderer weaponSprite;

    [SerializeField]
    private HashSet<Enemy> list = null;
    private void Awake(){ this.list = new HashSet<Enemy>(); }
    private void OnTriggerEnter2D(Collider2D col)
    {
         Enemy enemy = col.transform.GetComponent<Enemy>();
         if(enemy != null)
         {
             this.list.Add(enemy); 
             Vector2 Direction = (Vector2)(col.transform.position - transform.position).normalized;
             col.GetComponent<Enemy>().BeAttacked(param.Damage, Direction, param.knockbackForce, param.targetSturnTime);  
         } 
    }
    private void OnTriggerExit2D(Collider2D col)
    {
         Enemy enemy = col.transform.GetComponent<Enemy>();
         if(enemy != null && this.list.Contains(enemy) == true)
         {
             this.list.Remove(enemy);  
         } 
    }

    public void InitWeapon(WeaponParam p)
    {
        param = p;
        weaponSprite.sprite = p.WeaponSprite;
    }

    public void InitWeapon()
    {
        weaponSprite.sprite = param.WeaponSprite;
    }

    public class WeaponParam
    {
        public Sprite WeaponSprite;

        public int Damage;
        public float knockbackForce;
        public float targetSturnTime;
    }
}
