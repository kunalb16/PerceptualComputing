using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {
	//WeaponChange mywp;
	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;
	float realAimAngle = 0;
	Animator anim;

	bool gotFirstUpdate = false;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator> ();
		//Debug.Log ("abc" + anim);
		if (anim == null) {
			Debug.LogError("ERROR:No animator component on this character");		
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (photonView.isMine ) {


				} else if (photonView.isMine)
						 {
				} 
		else {
			transform.position = Vector3.Lerp(transform.position,realPosition,0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation,realRotation,0.1f);
			anim.SetFloat("AimAngle",Mathf.Lerp(anim.GetFloat("AimAngle"),realAimAngle,0.1f));
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		//Debug.Log ("abcd");
		if (stream.isWriting) {
			//This is OUR player. We need to send our actual position to the network.
			//Debug.Log("Sendposition"+realPosition);
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(anim.GetFloat("Speed"));
			stream.SendNext(anim.GetBool("Jumping"));
			stream.SendNext(anim.GetFloat("AimAngle"));

		} 
		else {
			//This is someone else's player. We need to receive their position(as of a
			//few millisecond ago, and update our version of that player.
			//Debug.Log("position" + realPosition);


			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();
			anim.SetFloat("Speed",(float)stream.ReceiveNext());
			anim.SetBool("Jumping",(bool)stream.ReceiveNext());
			realAimAngle = (float)stream.ReceiveNext();

			if(gotFirstUpdate == false){
				transform.position = realPosition;
				transform.rotation = realRotation;
				anim.SetFloat("AimAngle",realAimAngle);
				gotFirstUpdate = true;
			}
		}
	}
}
