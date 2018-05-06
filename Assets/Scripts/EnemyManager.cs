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
            print("ouchie now i have " + _health + " health");
		}
	}

    Rigidbody2D rb;

	void Start () 
	{
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () 
	{
		
	}

    public void takeDamage(int damage, Vector2 sourcePosition)
    {
        health-= damage;
        knockback(sourcePosition);
    }

    private void knockback(Vector2 position)
    {
        Vector2 knockbackForce = new Vector2(2, 3);
        if (position.x > transform.position.x)
            knockbackForce.x *= -1;

        rb.velocity += knockbackForce;
    }

}