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
public class ActivateActionWeaponChange : BaseAction {
	
	#region public Fields	
	
	/// <summary>
	/// The game objects that will be activated.
	/// </summary>
	//public GameObject[] GameObjects;	
	public int currentWeapon=0;
	public Transform[] weapons;

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
				

				
				currentWeapon = currentWeapon+1;
				if(currentWeapon==3)
					currentWeapon=0;
				for(int i = 0; i < weapons.Length; i++) {
					if(i == currentWeapon){
						weapons[i].gameObject.SetActive(true);
					}
					else{
						weapons[i].gameObject.SetActive(false);
					}
				}
						}
				}
	}
	

	//fixedUpdate is called once per physics loop
	//Do all MOVEMENT and physics stuff here.
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
  