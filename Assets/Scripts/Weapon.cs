using UnityEngine;
using System.Collections;

[System.Serializable]
[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon", order = 0)]
public class Weapon : Item
{
    public int damage { get; protected set; }
    public float attackSpeed { get; protected set; }
    public Vector2 knockback { get; protected set; }
    private Sprite sprite;
    private Animator weaponAnimator;

    public Weapon(int damage, float attackSpeed, Vector2 knockback)
    {
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.knockback = knockback;
    }

    public override string ToString()
    {
        return string.Format("[Weapon: damage={0}, attackSpeed={1}, knockback={2}]", damage, attackSpeed, knockback);
    }

}


public class Sword : Weapon
{
    public Sword(int damage, float attackSpeed, Vector2 knockback) : base(damage, attackSpeed, knockback) 
    {
        // insert unique collider animation assignment here
    }
}