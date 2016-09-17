/*******************************************************************************

INTEL CORPORATION PROPRIETARY INFORMATION
This software is supplied under the terms of a license agreement or nondisclosure
agreement with Intel Corporation and may not be copied or disclosed except in
accordance with the terms of that agreement
Copyright(c) 2012-2014 Intel Corporation. All Rights Reserved.

*******************************************************************************/
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RSUnityToolkit;

/// <summary>
/// Activate action: Activate the game objects on trigger
/// </summary>
[EventTrigger.EventTriggerAtt]
public class ActivateAction : BaseAction {
	
	#region public Fields	
	
	/// <summary>
	/// The game objects that will be activated.
	/// </summary>
	//public GameObject[] GameObjects;	
	public float speed = 6.0F;
	public float jumpSpeed = 80.0F;
	//public float gravity = 5.0F;
	Vector3 direction = new Vector3(0,0,0);		//forward/back & left/right direction
	float verticalVelocity = 0;
	
	CharacterController cc;
	Animator anim;
	#endregion
	
	#region Public Methods
	
	/// <summary>
	/// Returns the actions's description for GUI purposes.
	/// </summary>
	/// <returns>
	/// The action description.
	/// </returns>
	public override string GetActionDescription()
	{ 
		return "This Action activates the game objects whenever the associated triggers are fired.";
	}
	
	/// <summary>
	/// Sets the default trigger values (for the triggers set in SetDefaultTriggers() )
	/// </summary>
	/// <param name='index'>
	/// Index of the trigger.
	/// </param>
	/// <param name='trigger'>
	/// A pointer to the trigger for which you can set the default rules.
	/// </param>
	public override void SetDefaultTriggerValues(int index, Trigger trigger)
	{				
		switch (index)
		{
		case 0:
			((EventTrigger)trigger).Rules = new BaseRule[1] { AddHiddenComponent<GestureDetectedRule>() };
			((GestureDetectedRule)(trigger.Rules[0])).Gesture = MCTTypes.RSUnityToolkitGestures.Grab;
			break;
		}
	}
	
	#endregion
	
	#region Protected Methods
	
	/// <summary>
	/// Sets the default triggers for that action.
	/// </summary>
	protected override void SetDefaultTriggers()
	{	
		SupportedTriggers = new Trigger[1]{
		AddHiddenComponent<EventTrigger>()};			
	}
	
	#endregion
	
	#region Private Methods
	
	/// <summary>
	/// Update is called once per frame.
	/// </summary>
	
	void Awake(){
		cc = GetComponent<CharacterController> ();
		anim = GetComponent<Animator> ();
	}
	
	
	// Update is called once per frame
	void Update () {
		
		//WASD forward/back & left/right movement is stored in "direction".

		//If we're on the ground and the player wants to jump, set
		//verticalVelocity to a positive number
		
		ProcessAllTriggers ();
		
		//Start Event
		foreach (Trigger trgr in SupportedTriggers) {						
						if (trgr.Success) {
				

								verticalVelocity = jumpSpeed ;
								AdjustAimAngle ();
						}
				}
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
	#endregion
		
	#region Menu
	#if UNITY_EDITOR
	
	/// <summary>
	/// Adds the action to the RealSense Unity Toolkit menu.
	/// </summary>
	[UnityEditor.MenuItem ("RealSense Unity Toolkit/Add Action/Activate")]	
	static void AddThisAction () 
	{
		AddAction<ActivateAction>();
	} 
	
	#endif
	#endregion
}
  