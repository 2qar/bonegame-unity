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
    public Vector2 knockbackForce = new Vector2(2, 3);

    SpriteRenderer sr;

	void Start () 
	{
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
	}
	
	void Update () 
	{
		
	}

    public void takeDamage(int damage, Vector2 sourcePosition)
    {
        health -= damage;
        StartCoroutine(damageEffect());
        knockback(sourcePosition);
    }

    private void knockback(Vector2 position)
    {
        //Vector2 knockbackForce = new Vector2(2, 3);
        if (position.x > transform.position.x)
            knockbackForce.x *= -1;

        rb.velocity = knockbackForce;
    }

    private IEnumerator damageEffect()
    {
        sr.color = Color.white;
        yield return new WaitForSeconds(.05f);
        sr.color = Color.red;

        yield return null;
    }

}