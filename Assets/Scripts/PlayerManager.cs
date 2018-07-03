using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour 
{
    private const int PLATFORM_MASK = 1 << 9;
    private const int ENEMY_MASK = 1 << 11;
    private float halfSize = 1f;

    private const float HALF_PLAYER_SIZE = .5f;

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
            //if (value)
                //sword.transform.localPosition = new Vector2(-swordOffset, 0);
            //else
                //sword.transform.localPosition = new Vector2(swordOffset, 0);

            _facingLeft = value;
        }
    }

	Rigidbody2D rb;
    //GameObject sword;
    float swordOffset;
    private Weapon weapon;

    public List<Item> items { get; protected set; }

    public float swordAttackSpeed = .3f;
    private float swordAttackTimer;

	void Start () 
	{
        weapon = new Weapon(5, .3f, new Vector2(2, 3));

        items = new List<Item>();
        items.Add(weapon);

		rb = GetComponent<Rigidbody2D>();

        //sword = transform.GetChild(0).gameObject;
        //swordOffset = sword.transform.localPosition.x;

        swordAttackTimer = Time.time;
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
        float floorDistance = halfSize / 2 + .03f;

        Vector2 playerPosition = transform.position;
        Vector2 leftRayPosition = playerPosition - new Vector2(.5f, 0);
        Vector2 rightRayPosition = playerPosition + new Vector2(.5f, 0);

        RaycastHit2D leftRay = Physics2D.Raycast(leftRayPosition, Vector2.down, Mathf.Infinity, PLATFORM_MASK);
        RaycastHit2D rightRay = Physics2D.Raycast(rightRayPosition, Vector2.down, Mathf.Infinity, PLATFORM_MASK);

        bool floorLeft = leftRay.distance <= floorDistance;
        bool floorRight = rightRay.distance <= floorDistance;

        return floorLeft || floorRight;
    }

    // check for a wall on the left or right
    private RaycastHit2D[] checkForObject(bool left, float castDistance, int mask)
    {
        Vector2 direction = getDirection(left);

        float playerX = transform.position.x;
        float playerY = transform.position.y;
        float offset = HALF_PLAYER_SIZE;
        if (left)
            offset *= -1;

        Vector2 playerTop = new Vector2(playerX + offset, playerY + HALF_PLAYER_SIZE);
        Vector2 playerBottom = new Vector2(playerX + offset, playerY - HALF_PLAYER_SIZE);

        RaycastHit2D topRay = Physics2D.Raycast(playerTop, direction, castDistance, mask);
        RaycastHit2D bottomRay = Physics2D.Raycast(playerBottom, direction, castDistance, mask);

        return new RaycastHit2D[] { topRay, bottomRay }; 
    }
    
    // a gameobject if either of the raycasts hit one
    private GameObject objectCheck(bool left, float castDistance, int mask)
    {
        RaycastHit2D[] casts = checkForObject(left, castDistance, mask);

        foreach (RaycastHit2D cast in casts)
            if (cast.transform != null)
                return cast.transform.gameObject;

        return null;
    }

    private void attackController()
    {
        if(Input.GetKeyDown(KeyCode.E) && Time.time > swordAttackTimer)
        {
            GameObject enemy = objectCheck(facingLeft, 1.25f, ENEMY_MASK);

            if(enemy != null)
            {
                EnemyManager enemyMan = enemy.GetComponent<EnemyManager>();
                enemyMan.takeDamage(weapon, transform.position);
                swordAttackTimer = Time.time + swordAttackSpeed;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            explode(collision.gameObject.transform.position);
    }

    private void explode(Vector2 position)
    {
        try
        {
            // store the bones
            GameObject[] bones = new GameObject[6];

            // get the head
            bones[0] = transform.GetChild(0).gameObject;
            // get the spine
            bones[1] = bones[0].transform.GetChild(0).gameObject;

            // get all of the limbs attached to the spine
            for (int index = 0; index < 4; index++)
                bones[index + 2] = bones[1].transform.GetChild(index).gameObject;

            for (int index = 1; index < bones.Length; index++)
            {
                Rigidbody2D rb = bones[index].AddComponent<Rigidbody2D>();
                //BoxCollider2D collider = bone.AddComponent<BoxCollider2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;

                Vector2 knockback = new Vector2(2, 3);
                if (position.x > transform.position.x)
                    knockback.x *= -1;

                rb.velocity = knockback;
                rb.angularVelocity = Random.Range(-360, 360);

                bones[index].transform.parent = null;
                bones[index].gameObject.layer = LayerMask.NameToLayer("Bones");
            }

            setMovementData(Resources.Load<PlayerMovementData>("PlayerMovement/HeadMovement"));
        }
        catch { print("no booones"); }
    }

    private void setMovementData(PlayerMovementData data)
    {
        print(data); 
        speed = data.speed;
        jumpHeight = data.jumpHeight;
        fallMultiplier = data.fallMultiplier;
        lowJumpMultiplier = data.lowJumpMultiplier;
        halfSize = data.halfSize;
    }

}       