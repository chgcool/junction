using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb2D;

    private float move;
    private bool jump = false;
    private bool facingRight = true;

    public float jumpForce = 50f;
    public float moveForce = 1f;

    void Awake()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // transform.position = new Vector3(0.0f, -2.0f, 0.0f);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb2D.AddForce(transform.up * jumpForce);
        }

        move = Input.GetAxis("Horizontal") * moveForce;
        rb2D.velocity = new Vector2(move, rb2D.velocity.y);
        
        if ((rb2D.velocity.x > 0 && !facingRight) || (rb2D.velocity.x < 0 && facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}