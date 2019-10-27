using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;

    public Text endText;

    public Text lives;

    public AudioClip musicClipOne;

    public AudioClip soundClipOne;

    public AudioSource musicSource;

    private int scoreValue = 0;

    private int livesValue = 3;

    private bool facingRight = true;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        musicSource.clip = musicClipOne;
        musicSource.Play();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        endText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            EndGame();
        }
        else if (collision.collider.tag == "enemy")
        {
            //Subtract one to the current value of our lives variable.
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            //... then DESTROY the other object we just collided
            Destroy(collision.collider.gameObject);
            EndGame();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            anim.SetBool("inAir", false);
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetBool("inAir", true);
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
    private void EndGame()
    {
        if (scoreValue == 4)
        {
            transform.position = new Vector2(100f, 1.5f);
            rd2d.velocity = new Vector2(0f, 0f);
        }
        else if (scoreValue >= 8)
        {
            endText.color = new Color(255, 255, 0, 255);
            endText.text = "You win! Game created by Amy Schwinge";
            musicSource.clip = soundClipOne;
            musicSource.loop = false;
            musicSource.Play();
        }
        else if (livesValue == 0)
        {
            endText.color = new Color(255, 0, 0, 255);
            endText.text = "You lose! Game created by Amy Schwinge";
            Destroy(this);
        }        
    }
}