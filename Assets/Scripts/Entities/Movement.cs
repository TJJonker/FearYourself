using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D Rigidbody;

    public int MovementSpeed { get; private set; } = 4000;
    [SerializeField] private float Drag = .98f;
    [SerializeField] private float jumpheight = 3;
    private float GravityForce;

    // GroundCheck variables
    private const float GROUND_DISTANCE = .025f;

    [SerializeField] private LayerMask EnvironmentLayer;

    public bool DeathCollision { get; set; } = false;

    private Vector2 Yvelocity;

    // Whether the player can move or not
    public bool CanMove { get; set; } = true;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        GravityForce = Rigidbody.gravityScale * -9.81f;
    }

    private void Update()
    {
        if (!CanMove) return;
        Move();
        ApplyVerticalDrag();
        Jump();
    }

    private void Move()
    {
        var XMovement = Input.GetAxisRaw("Horizontal");
        var Movement = new Vector2(XMovement, 0);
        Rigidbody.AddForce(Movement * MovementSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            var jumpForce = Mathf.Sqrt(jumpheight * -2 * GravityForce);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, jumpForce * Vector2.up.y);
        }
    }

    private void ApplyVerticalDrag()
    {
        var velocity = Rigidbody.velocity;
        Rigidbody.velocity = new Vector2(velocity.x * Drag, Rigidbody.velocity.y);
    }

    private bool IsGrounded()
    {
        // Determine start position
        Vector2 startPos1 = new Vector2(transform.position.x + transform.localScale.x / 2f,
                                        transform.position.y - transform.localScale.y / 2f);
        Vector2 startPos2 = new Vector2(transform.position.x - transform.localScale.x / 2f,
                                        transform.position.y - transform.localScale.y / 2f);
        // Create two raycasts
        RaycastHit2D hit1 = Physics2D.Raycast(startPos1, Vector2.down, GROUND_DISTANCE, EnvironmentLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(startPos2, Vector2.down, GROUND_DISTANCE, EnvironmentLayer);
        // Return true if ground hit, else false
        if (hit1.collider || hit2.collider) return true;
        return false;
    }


}