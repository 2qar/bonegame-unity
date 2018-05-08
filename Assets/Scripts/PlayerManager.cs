using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour 
{
    private int platformMask = 1 << 9;

	public float speed;
    public float jumpHeight = 8;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private bool _facingLeft;
    public bool facingLeft
    {
        get { return _facingLeft; }
        set
        {
            if (value)
                sword.transform.localPosition = new Vector2(-swordOffset, 0);
            else
                sword.transform.localPosition = new Vector2(swordOffset, 0);

            _facingLeft = value;
        }
    }

	Rigidbody2D rb;
    GameObject sword;
    float swordOffset;
    BoxCollider2D swordCollider;

	void Start () 
	{
		rb = GetComponent<Rigidbody2D>();

        sword = transform.GetChild(0).gameObject;
        swordOffset = sword.transform.localPosition.x;
        swordCollider = sword.GetComponent<BoxCollider2D>(); 
	}
	
	void Update () 
	{
        movementController();    
        attackController();
    }

    void FixedUpdate()
    {
        jumpFix();
    }

    private void movementController()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            facingLeft = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            facingLeft = false;
        }
        else
            rb.velocity = new Vector2(0, rb.velocity.y);

        bool grounded = checkGrounded();
        if (grounded && Input.GetKeyDown(KeyCode.Space))
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    private void jumpFix()
    {
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime; 
            
    }

    private bool checkGrounded()
    {
        const float floorDistance = .52f;

        Vector2 playerPosition = transform.position;
        Vector2 leftRayPosition = playerPosition - new Vector2(.5f, 0);
        Vector2 rightRayPosition = playerPosition + new Vector2(.5f, 0);

        RaycastHit2D leftRay = Physics2D.Raycast(leftRayPosition, Vector2.down, Mathf.Infinity, platformMask);
        RaycastHit2D rightRay = Physics2D.Raycast(rightRayPosition, Vector2.down, Mathf.Infinity, platformMask);

        bool floorLeft = leftRay.distance <= floorDistance;
        bool floorRight = rightRay.distance <= floorDistance;

        return floorLeft || floorRight;
    }

    // check for a wall on the left or right
    private bool wallCheck(bool left, float castDistance, float trueDistance, int mask)
    {
        Vector2 direction = getDirection(left);

        float playerX = transform.position.x;
        float playerY = transform.position.y;
        float offset = 0.5f;

        Vector2 playerTop = new Vector2(playerX + offset, playerY + offset);
        Vector2 playerBottom = new Vector2(playerX + offset, playerY - offset);

        RaycastHit2D topRay = Physics2D.Raycast(playerTop, direction, castDistance, mask);
        RaycastHit2D bottomRay = Physics2D.Raycast(playerBottom, direction, castDistance, mask);

        bool clippingWallTop = topRay.distance <= trueDistance && topRay.transform != null;
        bool clippingWallBottom = bottomRay.distance <= trueDistance && bottomRay.transform != null;

        return clippingWallTop || clippingWallBottom;
    }

    private void attackController()
    {
        if(Input.GetKeyDown(KeyCode.E) && wallCheck(facingLeft, 1.25f, 1.24f, 1 << 11))
        {
            Vector2 direction = getDirection(facingLeft);

            Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
            RaycastHit2D ray = Physics2D.Raycast(playerPos + direction, direction, 1.25f, 1 << 11);

            if (ray.transform != null)
            {
                if (ray.transform.gameObject.CompareTag("Enemy"))
                {
                    ray.transform.gameObject.GetComponent<EnemyManager>().takeDamage(5, transform.position);
                }
            }
        }
    }

    private Vector2 getDirection(bool lookingLeft)
    {
        if (lookingLeft)
            return Vector2.left;
        else
            return Vector2.right;
    }

}
