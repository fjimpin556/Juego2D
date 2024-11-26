using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] int speed = 4;
    [SerializeField] int jumpForce = 5;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator anim;
    [SerializeField] int contador = 10;
    bool jumped = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(inputX * speed, rb.linearVelocity.y);
        print(inputX);

        if (inputX > 0)
        {
            sprite.flipX = false;
        }
        else if (inputX < 0)
        {
            sprite.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        } 
        else if (Input.GetKeyDown(KeyCode.Space) && jumped == false)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumped = true;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("isRunning", true);
        }
        else 
        {
            anim.SetBool("isRunning", false);
        }

        if (grounded() == false)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
            jumped = false;
        }
    }
    bool grounded()
    {
        RaycastHit2D touch = Physics2D.Raycast(transform.position, Vector2.down, 0.2f);
        if (touch.collider == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
            contador -= 1;
        }
    }
}
