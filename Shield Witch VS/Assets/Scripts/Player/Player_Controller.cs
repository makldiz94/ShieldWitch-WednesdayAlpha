﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour {


	//Cam
	public float groundY;
	public bool hitGround;
	public bool hitMoving;

    public float maxSpeed = 10f;
    public float baseSpeed = 10f;
    public float jumpForce = 300f;
    public float baseJump = 300f;
    private bool facingRight = true;

    private Rigidbody2D body2D;
    private Animator anim;

    private bool grounded = false;
    public Transform groundCheck;
    private float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public int curHealth;
    public int maxHealth = 3;

	[Header("Audio")]
	private AudioSource[] allAudioSources;
	private AudioSource jumpSource;
	private AudioSource damageSource;
	private AudioSource deathSource;
	public AudioClip jumpsound;
	public AudioClip damagesound;
	public AudioClip deathsound;

	private GameObject camcam;

    void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
    }

	// Use this for initialization
	void Start ()
    {
		GetComponent<Rigidbody2D> ().gravityScale = .5f;
		camcam = GameObject.Find ("Main_Camera(1)");
		//body2D.transform.position = CheckPoint.GetActiveCheckPointPosition();
        curHealth = maxHealth;
		AudioSource[] allAudioSources = GetComponents<AudioSource>();
		jumpSource = allAudioSources [0];
		damageSource = allAudioSources [1];
		deathSource = allAudioSources [2];

		maxSpeed = baseSpeed;
		jumpForce = baseJump;
		//baseJump = 0;

	}

	void FixedUpdate () {

        //uses the groundcheck transform to find whether we are ON THE GROUND. Returns true or false.
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        //Using Axis to move Horizontal, should work on controller.
        float move = Input.GetAxis("Horizontal");

        //makes the animator change to the movement animation
        if (grounded)
        {
            anim.SetBool("Jumping", false);
            anim.SetFloat("Speed", Mathf.Abs(move));
        } else {
            anim.SetFloat("Speed", 0);
            //anim.SetBool("Grounded", false);
        }

        if(Input.GetAxisRaw("RightJoyHorizontal") > 0.1 || Input.GetAxisRaw("RightJoyHorizontal") < -0.1 ||
            Input.GetAxisRaw("RightJoyVertical") > 0.1 || Input.GetAxisRaw("RightJoyVertical") < -0.1 && grounded)
        {
            //anim.SetBool("Casting", true);
        }
        else
        {
            //anim.SetBool("Casting", false);
        }

        //moves the player left or right based on button press.
        body2D.velocity = new Vector2(move * maxSpeed, body2D.velocity.y);



        if(move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
	}

    //In the update function it starts off with the Jump and goes into the players health.
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Not a hard reset, but a reset none the less.
            body2D.transform.position = CheckPoint.GetActiveCheckPointPosition();
            curHealth = maxHealth;
            maxSpeed = baseSpeed;
            jumpForce = baseJump;
        } 

		if(Input.GetButtonDown("Fire2"))
		{
			maxSpeed = 4;
			baseJump = 550;
			jumpForce = 550;
		}
        //Currently Jump is just the space button for testing purposes, we will change this!
        if(grounded && Input.GetButtonDown("Jump"))
        {
            anim.SetBool("Ground", true);
            anim.SetBool("Jumping", true);
            body2D.AddForce(new Vector2(0, jumpForce));
			//Jump Audio
			jumpSource.clip = jumpsound;
			jumpSource.Play ();
        }
        else
        {
            anim.SetBool("Ground", false);
        }

        if (!grounded)
        {
            anim.SetBool("Ground", true);
            anim.SetBool("Jumping", true);
        }
        else
        {
            anim.SetBool("Ground", false);
        }
        //Checks whether you have Health.
        if(curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }

        if(curHealth <= 0)
        {

            Die();
        }
			
    }

    void Flip()
    {
        //acquires local scale and flipping it to make the character face properly.
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Die()
    {
        StartCoroutine(Death());
		
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        //if an enemy bullet touches player, health decreases and bullet destroys
		if (col.gameObject.tag == "Deadly")
        {
            //Take damage audio
			damageSource.clip = damagesound;
			damageSource.Play ();
            //curHealth--;
            StartCoroutine(Damage());
            StartCoroutine(Hit());

            //Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "Killbox")
        {
            Die();
        }
    }
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Enemy" || col.gameObject.tag == "Deadly" || col.gameObject.tag == "BulletHold")
		{
			//Take damage audio
			damageSource.clip = damagesound;
			damageSource.Play ();
            //curHealth--;
            StartCoroutine(Damage());
            StartCoroutine(Hit());
        }

		if (col.gameObject.tag == "Ground") {
			groundY = col.transform.position.y;
			hitGround = true;
			hitMoving = false;

		}

		if (col.gameObject.tag == "Moving") {
			hitGround = false;
			hitMoving = true;
		}
	}

    IEnumerator Damage()
    {
        curHealth--;
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator Death()
    {
        //Death Audio
        deathSource.clip = deathsound;
        deathSource.Play();

        maxSpeed = 0f;
        jumpForce = 0f;
        //anim.SetBool("Dead", true);
        yield return new WaitForSeconds(0f); //2f

        //This needs to be updated in case they run out of lives?
        body2D.transform.position = CheckPoint.GetActiveCheckPointPosition();
        curHealth = maxHealth;
        maxSpeed = baseSpeed;
        jumpForce = baseJump;
    }

    IEnumerator Hit()
    {
        anim.SetBool("Hit", true);
        yield return new WaitForSeconds(.8f);
        anim.SetBool("Hit", false);
    }
}
