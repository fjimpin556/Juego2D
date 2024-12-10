using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] int speed = 4;
    [SerializeField] int jumpForce = 5;
    [SerializeField] int dashForce = 2;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator anim;
    [SerializeField] GameObject shot;
    [SerializeField] CapsuleCollider2D capsule;

    // Position control
    bool jumped = false;
    public static bool right = true;
    float height;
    float lastHeight;
    bool shooted = false;
    bool ducked = false;
    bool dashed = false;
    bool canDash = true;


    // TextosUI
    [SerializeField] TMP_Text TextLives;
    [SerializeField] TMP_Text TextItems;
    [SerializeField] TMP_Text TextTime;

    [SerializeField] int items = 0;
    [SerializeField] int lives = 3;
    [SerializeField] float time = 180;

    // Ganar/Perder
    [SerializeField] GameObject TextWin, TextLose;
    bool endGame = false;

    // Objetos
    bool son = false;
    string objectSonType;
    GameObject objectSon;

    // Audio
    AudioSource audioSrc;
    [SerializeField] AudioClip soundJump, soundShoot, soundItem, soundDamage;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.invulnerable = false;
        rb = GetComponent<Rigidbody2D>();
        TextLives.text = "Lives: " + lives;
        TextItems.text = "Items: " + items;
        TextTime.text = time.ToString();

        audioSrc = GetComponent<AudioSource>();

        capsule = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!endGame)
        {
            if (!ducked && !dashed)
            {
                float inputX = Input.GetAxis("Horizontal");
                rb.linearVelocity = new Vector2(inputX * speed, rb.linearVelocity.y);
                print(inputX);

                if (inputX > 0)
                {
                    sprite.flipX = false;
                    right = true;
                }
                else if (inputX < 0)
                {
                    sprite.flipX = true;
                    right = false;
                }

                if (Input.GetKeyDown(KeyCode.Space) && grounded())
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    audioSrc.PlayOneShot(soundJump);
                }
                else if (Input.GetKeyDown(KeyCode.Space) && jumped == false)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    audioSrc.PlayOneShot(soundJump);
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

            // Disparo
            if (Input.GetMouseButtonDown(0))
            {
                if (!shooted)
                {
                    shooted = true;
                    Instantiate(shot, new Vector3(transform.position.x, transform.position.y + 0.8f, 0), Quaternion.identity);
                    anim.SetBool("isShooting", true);
                    audioSrc.PlayOneShot(soundShoot);
                    Invoke("returnShot", 1);
                }
            }

            time -= Time.deltaTime;
            if (time < 0)
            {
                time = 0;
                endGame = true;
                TextLose.SetActive(true);
                Invoke("goToMenu", 3);
            }

            float min, sec;
            min = Mathf.Floor(time / 60);
            sec = time % 60;
            TextTime.text = min.ToString("00") + ":" + sec.ToString("00");

            // Agacharse
            if (Input.GetKey(KeyCode.LeftControl) && grounded())
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    if (!dashed && canDash)
                    {
                        ducked = true;
                        anim.SetBool("isDucking", true);
                        dashed = true;
                        canDash = false;
                        capsule.offset = new Vector2(-0.1060266f, 0.7418327f);
                        capsule.size = new Vector2(0.9914839f, 1.483665f);
                        if (right)
                        {
                            rb.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
                        }
                        else
                        {
                            rb.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
                        }
                        Invoke("stopDashing", 0.8f);
                        Invoke("canDashing", 3);
                    }
                }
                else
                {
                    capsule.offset = new Vector2(0.06700808f, 0.7810583f);
                    capsule.size = new Vector2(0.9703304f, 1.562116f);
                    anim.SetBool("isDucking", true);
                    ducked = true;
                }
            }
            else
            {
                if (!dashed)
                {
                    capsule.offset = new Vector2(0.0397824f, 0.9362447f);
                    capsule.size = new Vector2(0.9158791f, 1.872489f);
                    anim.SetBool("isDucking", false);
                    ducked = false;
                }
            }

            // Salto caida
            height = transform.position.y;
            if (height < lastHeight)
            {
                anim.SetBool("isDescending", true);
            }
            else if (height > lastHeight)
            {
                anim.SetBool("isDescending", false);
            }

            lastHeight = height;

            // Usar objeto
            if (Input.GetKeyDown(KeyCode.Y) && son)
            {
                useObject();
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
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
            items += 1;
            audioSrc.PlayOneShot(soundItem);
            TextItems.text = "Items: " + items;
            if (items > 9)
            {
                endGame = true;
                TextWin.SetActive(true);
                Invoke("goToCredits", 3);
            }
        }

        if (other.gameObject.tag == "PowerUp")
        {
            if (!son)
            {
                other.gameObject.transform.SetParent(transform);
                other.transform.localPosition = new Vector2(-0.1f, 0.5f);
                son = true;
                objectSonType = "PowerUp";
                objectSon = other.gameObject;
            }            
        }

        if (other.gameObject.tag == "Poisonus")
        {
            Damage();
        }

        if (other.gameObject.tag == "Bouncer")
        {
            rb.AddForce(Vector2.up * 3 * jumpForce, ForceMode2D.Impulse);
            audioSrc.PlayOneShot(soundJump);
        }
    }

    void becomeVulnerable()
    {
        sprite.color = Color.white;
        GameManager.invulnerable = false;
    }

    public void Damage()
    {
        if (!endGame)
        {
            lives -= 1;
        }
        sprite.color = Color.red;
        audioSrc.PlayOneShot(soundDamage);
        GameManager.invulnerable = true;
        Invoke("becomeVulnerable", 2);
        if (lives < 0)
        {
            lives = 0;
            endGame = true;
            TextLose.SetActive(true);
            Invoke("goToMenu", 3);
        }
        TextLives.text = "Vidas: " + lives;
    }

    void goToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    void goToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    void returnShot()
    {
        shooted = false;
    }

    void stopDashing()
    {
        dashed = false;
        ducked = false;
        anim.SetBool("isDucking", false);
    }

    void canDashing()
    {
        canDash = true;
    }

    void useObject()
    {
        if (objectSonType == "PowerUp")
        {
            Destroy(objectSon);
            audioSrc.PlayOneShot(soundItem);
            GameManager.invulnerable = true;
            sprite.color = Color.cyan;
            Invoke("becomeVulnerable", 5);
        }
    }
}
