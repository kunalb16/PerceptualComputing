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
/// Activate action: links the transformation of the associated Game Object to the real world tracked source
/// </summary>
public class myCharacterMovement : VirtualWorldBoxAction {
	
	#region Public Fields
	
	/// <summary>
	/// The smoothing factor.
	/// </summary>
	public float SmoothingFactor = 0;
	public float speed = 6.0F;
	Vector3 direction = Vector3.zero;		//forward/back & left/right direction
	float verticalVelocity = 0;
	
	CharacterController cc;
	Animator anim;


	/// <summary>
	/// Position / Rotation constraints
	/// </summary>
	public Transformation3D  Constraints;
	
	/// <summary>
	/// Invert Positions / Rotations
	/// </summary>
	public Transformation3D  InvertTransform;	
	
	/// <summary>
	/// Effect Physics will use Unity’s MoveRotation and MovePosition to move the object.
	/// </summary>
	public bool EffectPhysics = false;
	
	/// <summary>
	/// SetDefaultsTo lets you switch in one click between 3 different tracking modes – hands, face and object tracking
	/// </summary>
	[BaseAction.ShowAtFirst]
	public Defaults SetDefaultsTo  = Defaults.HandTracking;
	
	#endregion
	
	#region Private Fields
	
	[SerializeField]
	[HideInInspector]
	private Defaults _lastDefaults = Defaults.HandTracking;
	
	private bool _actionTriggered = false;
	
	
	#endregion
	
	#region Ctor
	
	/// <summary>
	/// Constructor
	/// </summary>
	public myCharacterMovement() : base()
	{
		Constraints = new Transformation3D();			
		InvertTransform = new Transformation3D();

	}
	
	#endregion
	
	#region Public methods
	
	/// <summary>
	/// Determines whether this instance is support custom triggers.
	/// </summary>		
	public override bool IsSupportCustomTriggers()
	{
		return false;
	}
	
	/// <summary>
	/// Returns the actions's description for GUI purposes.
	/// </summary>
	/// <returns>
	/// The action description.
	/// </returns>
	public override string GetActionDescription()
	{
		return "This Action links the transformation of the associated Game Object to the real world tracked source";
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
		if (SetDefaultsTo == Defaults.HandTracking)
		{
			switch (index)
			{
			case 0:
				trigger.FriendlyName = "Start Event";
				((EventTrigger)trigger).Rules = new BaseRule[1] { AddHiddenComponent<HandDetectedRule>() };
				break;
			case 1:
				
				((TrackTrigger)trigger).Rules = new BaseRule[1] { AddHiddenComponent<HandTrackingRule>() };
				break;
			case 2:
				trigger.FriendlyName = "Stop Event";
				((EventTrigger)trigger).Rules = new BaseRule[1] { AddHiddenComponent<HandLostRule>() };
				break;
			}
		}
		else if (SetDefaultsTo == Defaults.FaceTracking)
		{
			switch (index)
			{
			case 0:
				trigger.FriendlyName = "Start Event";
				((EventTrigger)trigger).Rules = new BaseRule[1] { AddHiddenComponent<FaceDetectedRule>() };
				break;
			case 1:
				((TrackTrigger)trigger).Rules = new BaseRule[1] { AddHiddenComponent<FaceTrackingRule>() };
				break;
			case 2:
				trigger.FriendlyName = "Stop Event";
				((EventTrigger)trigger).Rules = new BaseRule[1] { AddHiddenComponent<FaceLostRule>() };
				break;
			}
		}		
		else if (SetDefaultsTo == Defaults.ObjectTracking)
		{
			switch (index)
			{
			case 0:
				trigger.FriendlyName = "Start Event";
				((EventTrigger)trigger).Rules = new BaseRule[1] { AddHiddenComponent<ObjectDetectedRule>() };
				break;
			case 1:
				((TrackTrigger)trigger).Rules = new BaseRule[1] { AddHiddenComponent<ObjectTrackingRule>() };
				break;
			case 2:
				trigger.FriendlyName = "Stop Event";
				((EventTrigger)trigger).Rules = new BaseRule[1] { AddHiddenComponent<ObjectLostRule>() };
				break;
			}
		}		
	} 
	
	/// <summary>
	/// Updates the inspector.
	/// </summary>
	public override void UpdateInspector()
	{
		if (_lastDefaults != SetDefaultsTo)
		{
			CleanSupportedTriggers();
			SupportedTriggers = null;
			InitializeSupportedTriggers();
			_lastDefaults = SetDefaultsTo;
		}		
	}
	
	#endregion
	
	#region Protected methods
	
	/// <summary>
	/// Sets the default triggers for that action.
	/// </summary>
	override protected void SetDefaultTriggers()
	{
		SupportedTriggers = new Trigger[3]{
			AddHiddenComponent<EventTrigger>(),
			AddHiddenComponent<TrackTrigger>(),
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
				ProcessAllTriggers ();
		
				//Start Event
				if (!_actionTriggered && SupportedTriggers [0].Success) {
			
						_actionTriggered = true;
				}
		
				//Stop Event
				if (_actionTriggered && SupportedTriggers [2].Success) {
						_actionTriggered = false;
			
				}
		
				if (!_actionTriggered) {
						return;
				}
		
				TrackTrigger trgr = (TrackTrigger)SupportedTriggers [1];
		
				if (trgr.Success) {
						//WASD forward/back & left/right movement is stored in "direction".
						direction = new Vector3 (0, 0, 1);
						direction = transform.TransformDirection (direction);
						//This ensures we don't move faster diagonally
						
		
						//Set our animation "Speed" parameter. This will move us from "idle" to "run" animations,
						//but we could also use this to blend between "walk" and "run" as well.
						anim.SetFloat ("Speed", direction.magnitude);
		
						//If we're on the ground and the player wants to jump, set
						//verticalVelocity to a positive number
		
						
						AdjustAimAngle ();
	
				}
		}
	void AdjustAimAngle(){



						Camera myCamera = this.GetComponentInChildren<Camera> ();
		
						if (myCamera == null) {
								Debug.LogError ("No camera on player");
								return;
						}
						float AimAngle = 0;
						if (myCamera.transform.rotation.eulerAngles.x <= 90f) {
								//we are looking DOWN.	
								AimAngle = - myCamera.transform.rotation.eulerAngles.x;
						} else {
								AimAngle = 360 - myCamera.transform.rotation.eulerAngles.x;	
						}
						anim.SetFloat ("AimAngle", AimAngle);
				}

	//fixedUpdate is called once per physics loop
	//Do all MOVEMENT and physics stuff here.
	void FixedUpdate(){
		updateVirtualWorldBoxCenter ();
		
		ProcessAllTriggers ();
		
		//Start Event
		if (!_actionTriggered && SupportedTriggers [0].Success) {
			
			_actionTriggered = true;
		}
		
		//Stop Event
		if (_actionTriggered && SupportedTriggers [2].Success) {
			_actionTriggered = false;
			
		}
		
		if (!_actionTriggered) {
			return;
		}
		
		TrackTrigger trgr = (TrackTrigger)SupportedTriggers [1];
		
		if (trgr.Success) {
		//"direction" is the desired movement direction based on our player's input.
		Vector3 dist = direction * speed * Time.deltaTime ;
		cc.Move(dist) ;
		}
	}
	/// <summary>
	/// Gets the average of the given list and add new number to the list
	/// </summary>
	/// <returns>
	/// The average and add new number.
	/// </returns>
	/// <param name='list'>
	/// List.
	/// </param>
	/// <param name='number'>
	/// Number.
	/// </param>
	private float GetAverageAndAddNewNumber(List<float> list, float number)
	{
		int size = list.Count;
		if ( size < 2 )
		{
			return number;
		}
		
		float sum = 0;
		
		for (int i =0; i < size-1; i++)
		{			
			sum+=list[i];
			list[i] = (float)list[i+1];			
		}
		sum+=list[size-1];
		list[size-1] = number;		
		
		return (sum/size);
	}
	
	
	#endregion
	
	#region Nested Types
	
	/// <summary>
	/// Default trackig modes that can be selected by SetDefaultsTo
	/// </summary>
	public enum Defaults
	{
		FaceTracking,
		HandTracking,
		ObjectTracking
	}
	
	
	#endregion
	
	#region Menu
	#if UNITY_EDITOR
	
	/// <summary>
	/// Adds the action to the RealSense Unity Toolkit menu.
	/// </summary>
	[UnityEditor.MenuItem ("RealSense Unity Toolkit/Add Action/Tracking")]
	static void AddThisAction () 
	{
		AddAction<TrackingAction>();
	} 
	
	#endif
	#endregion
	
}
