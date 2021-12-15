using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// [SerializeField]
	public float health;
	public float maxHealth;
	public float movementSpeed;
	public float groundRadius;
	public float jumpForce;
	public bool airControl;
	public Transform[] GroundPoints;
	public LayerMask whatIsGround;
	
	
	public bool facingRight;
	private bool isGrounded;
	private bool jump;
	private bool dead;
	private Rigidbody2D playerRigidBody; 
	private GameObject mainCamera;
	private Animator anim;
	public Color hurtColor;


	void Awake () {
		dead = false;
		playerRigidBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		if(anim == null){
			anim = GetComponentsInChildren<Animator>()[0];
		}
		mainCamera = GameObject.Find ("Main Camera");
	}

	void Update(){
        if(StateManager.smInstance.IsPlaying()){
			if(!playerRigidBody.simulated){
				playerRigidBody.simulated = true;
			}
            InputHandler ();
        }
	}

	void FixedUpdate() {
        if(StateManager.smInstance.IsPlaying()){
            float horizontal = Input.GetAxis ("Horizontal");
            isGrounded = IsGrounded();
            HandleMovement (horizontal);
            Flip (horizontal);
            ResetValues ();
        }
	}

	void InputHandler(){
		if(!dead){
			if(Input.GetKeyDown(KeyCode.Space)){
				jump = true;
			}
		}
	}

	private void HandleMovement (float horizontal) {
		if(!dead){
			if(isGrounded || airControl){
				playerRigidBody.velocity = new Vector2(horizontal*movementSpeed, playerRigidBody.velocity.y); 
			}
			if(isGrounded && jump){
				isGrounded = false;
				playerRigidBody.AddForce(new Vector2(0, jumpForce));
				AudioManager.audioManager.Play("PlayerJump");
			}
			anim.SetFloat ("Speed", Mathf.Abs(horizontal));
		}
	}

	private void Flip(float horizontal){
		if(!dead){
			if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
				facingRight = !facingRight;
				anim.SetBool("Right", facingRight);
			}
		}
	}
	
	public void TakeDamage(float damage){
		if(!dead){
			health -= damage;
			// anim.SetTrigger("Hit");
			StartCoroutine ("HurtColor");
			if(health <=0){
				KO();
			}
		}
	}

	IEnumerator HurtColor() {
		for (int i = 0; i < 3; i++) {
			SpriteRenderer[] SPRs = GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer SPR in SPRs){
				SPR.color = hurtColor;
			}
			yield return new WaitForSeconds (.1f);
			foreach(SpriteRenderer SPR in SPRs){
				SPR.color = Color.white; //White is the default "color" for the sprite, if you're curious.
			}
			yield return new WaitForSeconds (.1f);
		}
	} //This IEnumerator runs 3 times, resulting in 3 flashes.

	public void Heal(float heal){
		if(heal + health > maxHealth){
			health = maxHealth;
		}
		else{
			health += heal;
		}
	}

	public float GetHealth(){
		float tempHealth = health;
		return tempHealth;
	}

	public void KO(){
		if(!dead){
			anim.SetTrigger("Die");
			dead = true;
			AudioManager.audioManager.Play("Die");
			StateManager.smInstance.SetState(StateManager.State.Lose);
		}
		//do game over thing
	}

	public bool isDead(){
		return dead;
	}

	private bool IsGrounded(){
		foreach (Transform point in GroundPoints) {
			//Make a circle collider over every ground point
			Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
			for (int i = 0; i < colliders.Length; i++) {
				//If collider != player, so player does not collide with itself
				if(colliders[i].gameObject != gameObject){
					return true;
				}
			}
		}
		return false;
	}

	void ResetValues(){
		jump = false;
	}

}