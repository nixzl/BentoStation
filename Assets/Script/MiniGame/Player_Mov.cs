using UnityEngine;

public class Player_Mov : MonoBehaviour
{
    public int playerSpeed = 9;
    private bool facingRight = true;
    public int PlayerJumpPower = 1250;
    private float moveX;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    // 1. Add a variable for the Animator
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // 2. Cache the Animator component
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            Jump();
        }
        
        if (moveX < 0.0f && facingRight == true)
        {
            FlipPlayer();
        }
        else if (moveX > 0.0f && facingRight == false)
        {
            FlipPlayer();
        }
        
        rb.linearVelocity = new Vector2(moveX * playerSpeed, rb.linearVelocity.y);

        // 3. Update the Animator!
        // Mathf.Abs converts -1 (moving left) into 1, so the speed is always positive.
        anim.SetFloat("Speed", Mathf.Abs(moveX));
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * PlayerJumpPower);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}