using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	private int _health = 100;
    public int health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
                Destroy(gameObject);
        }
    }


    Rigidbody2D rb;

    SpriteRenderer sr;

	void Start () 
	{
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
	}
	
	void Update () 
	{
        
	}

    public void takeDamage(Weapon weapon, Vector2 sourcePosition)
    {
        health -= weapon.damage;
        StartCoroutine(damageEffect());
        knockback(sourcePosition, weapon.knockback);
    }

    private void knockback(Vector2 position, Vector2 knockback)
    {
        if (position.x > transform.position.x)
            knockback.x *= -1;

        rb.velocity = knockback;
    }

    private IEnumerator damageEffect()
    {
        sr.color = Color.white;
        yield return new WaitForSeconds(.05f);
        sr.color = Color.red;

        yield return null;
    }

}