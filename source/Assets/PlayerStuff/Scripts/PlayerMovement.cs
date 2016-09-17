using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	//This component is only enabled for "my player" (i.e. the character belonging to the local client machine).
	//Remote players figures will be saved by a NetworkCharacter, which is also responsible for sending my player's
	//location to other computers.

	public float speed = 10f;		
	public float jumpSpeed = 10f;		

	Vector3 direction = Vector3.zero;		//forward/back & left/right direction
	float verticalVelocity = 0;

	CharacterController cc;
	Animator anim;


	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		//WASD forward/back & left/right movement is stored in "direction".
		direction = transform.rotation * new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

		//This ensures we don't move faster diagonally
		if (direction.magnitude > 1f) {
				direction = direction.normalized;
		}

		//Set our animation "Speed" parameter. This will move us from "idle" to "run" animations,
		//but we could also use this to blend between "walk" and "run" as well.
		anim.SetFloat ("Speed", direction.magnitude);

		//If we're on the ground and the player wants to jump, set
		//verticalVelocity to a positive number

		if (cc.isGrounded && Input.GetButton ("Jump")) {
			verticalVelocity = jumpSpeed;
		} 
		AdjustAimAngle ();
	}


	void AdjustAimAngle(){
		Camera myCamera = this.GetComponentInChildren<Camera> ();

		if (myCamera == null) {
			Debug.LogError("No camera on player");
			return;
		}
		float AimAngle = 0;
		if (myCamera.transform.rotation.eulerAngles.x <= 90f) {
			//we are looking DOWN.	
			AimAngle = - myCamera.transform.rotation.eulerAngles.x;
		} 
		else {
			AimAngle = 360 - myCamera.transform.rotation.eulerAngles.x;	
		}
		anim.SetFloat ("AimAngle", AimAngle);
	}

	//fixedUpdate is called once per physics loop
	//Do all MOVEMENT and physics stuff here.
	void FixedUpdate(){

		//"direction" is the desired movement direction based on our player's input.
		Vector3 dist = direction * speed * Time.deltaTime ;

		if (cc.isGrounded && verticalVelocity < 0) {
			//We're currently on ground and vertical velocity is
			//not positive (i.e. we're not starting a jump).

			//Ensure that we're not playing the jump animation.
			anim.SetBool("Jumping",false);
		

			//Set our vertical velocity to almost zero. This ensures that:
			// a) We don't start falling at warp speed if we fall of a cliff (by being close to zero).
			// b) cc.isGrounded returns true every frame (by still being slightly negative as opposed to zero).
			verticalVelocity = Physics.gravity.y * Time.deltaTime ;
		}

		else{
			//We 're either not grounded, or we have a positive ertivalVelocity (i.e. we are starting a jump).

			//To make sure we don't go into a jump animation while walking down a slope make sure that
			//verticalVelocity is above some arbitrary threshold befor triggering the animation.
			if(Mathf.Abs(verticalVelocity) > jumpSpeed*0.75f){
				anim.SetBool("Jumping", true);
			}
			verticalVelocity += Physics.gravity.y * Time.deltaTime ;
		}

		dist.y = verticalVelocity * Time.deltaTime;

		cc.Move(dist) ;
	}
}
