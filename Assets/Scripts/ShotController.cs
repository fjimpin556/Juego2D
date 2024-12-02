using UnityEngine;

public class ShotController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (PlayerControl.right)
        {
            rb.linearVelocity = Vector2.right * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.left * speed;
        }
        Invoke("DestroyShot", 3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void DestroyShot()
    {
        Destroy(gameObject);
    }
}
