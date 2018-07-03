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

    Transform rayBox;

	void Start () 
	{
        rayBox = transform.GetChild(0);

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
	}
	
	void Update () 
	{
        Vector2 position = transform.position;
        position.x += .5f;
        position.y += .5f;

        Vector2 size = new Vector2(2, 2);
        RaycastHit2D ray = Physics2D.BoxCast(position, size, 0, Vector2.right, 2.5f, 1 << 8);

        if(ray.point != new Vector2(0, 0))
            rayBox.position = ray.point;
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