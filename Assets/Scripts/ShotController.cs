using UnityEngine;

public class ShotController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.right * speed;
        Invoke("DestroyShot", 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyShot()
    {
        Destroy(gameObject);
    }
}
