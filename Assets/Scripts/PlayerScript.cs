using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    private bool facingRight = true;
    public GameObject winTextObject;
    public Text lives;
    private int livesValue = 3;
    public GameObject loseTextObject;
    private GameObject Player;

Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        winTextObject.SetActive(false);
        lives.text = livesValue.ToString();
        loseTextObject.SetActive(false);
        anim = GetComponent<Animator>();
        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

void Update()

    { 
     if (Input.GetKeyDown(KeyCode.D))

        {
          anim.SetInteger("State", 1);
         }

     if (Input.GetKeyUp(KeyCode.D))

        {
          anim.SetInteger("State", 0);
         }

     if (Input.GetKeyDown(KeyCode.A))

        {
          anim.SetInteger("State", 1);
         }

     if (Input.GetKeyUp(KeyCode.A))

        {
          anim.SetInteger("State", 0);
         }
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
        if (facingRight == false && hozMovement > 0.0f)
        {
     Flip();
        }
     else if (facingRight == true && hozMovement < 0.0f)
        {
     Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if (scoreValue == 4)
            {
              transform.position = new Vector3(35.0f, 0.25f, 0.0f);
              livesValue = 3;
              lives.text = livesValue.ToString();
            }
            if (scoreValue >= 8)
            {
              winTextObject.SetActive(true);
              Destroy(this);
              musicSource.clip = musicClipOne;
              musicSource.Stop();
              musicSource.clip = musicClipTwo;
              musicSource.Play();
              musicSource.loop = false;
              anim.SetInteger("State", 0);
            }
        }

       if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = livesValue.ToString();
            Destroy(collision.collider.gameObject);
            if (livesValue <= 0)
            {
              loseTextObject.SetActive(true);
              Destroy(this);
              anim.SetInteger("State", 0);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
       if (collision.collider.tag == "Ground" && isOnGround)
       {
           if (Input.GetKey(KeyCode.W))
           {
               rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
           }
       }
    }

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}