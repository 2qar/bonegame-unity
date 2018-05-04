using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
	public float speed;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

	Rigidbody2D rb;

    bool jumping;

	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () 
	{
        movementController();    
    }

    void movementController()
    {
        if (Input.GetKey(KeyCode.A))
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        else if (Input.GetKey(KeyCode.D))
            rb.velocity = new Vector2(speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);

        bool grounded = checkGrounded();
        if (grounded && Input.GetKeyDown(KeyCode.Space))
            rb.velocity = Vector2.up * 7;
        jump();
    }

    void jump()
    {
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
            
    }

    bool checkGrounded()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, 1 << 9);
        if (ray.distance <= .52f)
            return true;
        return false;
    }

}
