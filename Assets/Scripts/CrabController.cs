using UnityEngine;
using UnityEngine.SceneManagement;

public class CrabController : MonoBehaviour
{
    [SerializeField] int speed = 3;
    [SerializeField] Vector3 endPosition;
    Vector3 startPosition;
    bool goingToTheEnd = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (goingToTheEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            if (transform.position == endPosition)
            {
                goingToTheEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            if (transform.position == startPosition)
            {
                goingToTheEnd = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player"){
            SceneManager.LoadScene("Level1");
        }
    }
}
