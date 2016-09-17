using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float hitPoints = 100f;
	public float hitperDistance = 66f;
	float currentHitPoints;
	float hitPointDistance;
	float damageKardita;
	
	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
	}
	
	[RPC]
	public void TakeDamage(WeaponData tik,Vector3 hitPoint){
		
		//Debug.Log ("HitPoint:" + hitPoint.y);
		hitPointDistance=(hitPoint.y-transform.position.y);
		currentHitPoints -= hitPointDistance*hitperDistance;
		
		if (currentHitPoints <= 0) {
			Die();		
		}
	}

	/*void OnGUI(){
		if (GetComponent<PhotonView> ().isMine && gameObject.tag == "Player") {
			if(GUI.Button(new Rect(Screen.width-100,0,100,40),"Suicide!")){
				Die();
			}		
		}
	}*/

	public void Die(){
		if (GetComponent<PhotonView> ().instantiationId == 0) {
			Destroy (gameObject);
		} 
		else {
			if(GetComponent<PhotonView> ().isMine){
				if(gameObject.tag == "Player"){

					NetworkManager nm = GameObject.FindObjectOfType<NetworkManager>();
					nm.standbyCamera.SetActive(true);
					nm.respawnTimer = 3f; 
				}
				PhotonNetwork.Destroy (gameObject);
			}	
		}
	}
}

