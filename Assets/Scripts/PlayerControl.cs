using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    public int speed = 4;
    public int jumpForce = 5;

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

        if (Input.GetKeyDown(KeyCode.Space) && grounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
}
