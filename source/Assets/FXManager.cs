using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {

	public GameObject sniperBulletFXPrefab;
	public GameObject sniperBulletFX2Prefab;
	public GameObject sniperBulletFX3Prefab;


	[RPC]
	void SniperBulletFX( Vector3 startPos, Vector3 endPos){
		Debug.Log ("SniperBulletFX");

		if (sniperBulletFXPrefab != null) {
			GameObject sniperFX = (GameObject)Instantiate (sniperBulletFXPrefab, startPos, Quaternion.LookRotation( endPos - startPos ));
			LineRenderer lr = sniperFX.transform.Find ("LineFX").GetComponent<LineRenderer> ();
			if(lr != null){
				lr.SetPosition (0, startPos);
				lr.SetPosition (1, endPos);
			}
			else{
				Debug.LogError("sniperBulletFXPrefab's linerenderer is missing.");
			}
		}
		else {
			Debug.LogError("sniperBulletFXPrefab is misssing");
		}


	}
	[RPC]
	void SniperBulletFX2( Vector3 startPos, Vector3 endPos){
		Debug.Log ("SniperBulletFX2");
		
		if (sniperBulletFX2Prefab != null) {
			GameObject sniperFX2 = (GameObject)Instantiate (sniperBulletFX2Prefab, startPos, Quaternion.LookRotation( endPos - startPos ));
			LineRenderer lr = sniperFX2.transform.Find ("LineFX").GetComponent<LineRenderer> ();
			if(lr != null){
			lr.SetPosition (0, startPos);
			lr.SetPosition (1, endPos);
			}
			else{
				Debug.LogError("sniperBulletFX2Prefab's linerenderer is missing.");
			}
			}
	
		else {
			Debug.LogError("sniperBulletFX2Prefab is misssing");
		}
		
		
	}
	[RPC]
	void SniperBulletFX3( Vector3 startPos, Vector3 endPos){
		Debug.Log ("SniperBulletFX3");
		
		if (sniperBulletFX3Prefab != null) {
			GameObject sniperFX3 = (GameObject)Instantiate (sniperBulletFX3Prefab, startPos, Quaternion.LookRotation (endPos - startPos));
			LineRenderer lr = sniperFX3.transform.Find ("LineFX").GetComponent<LineRenderer> ();
			if(lr != null){
				lr.SetPosition (0, startPos);
				lr.SetPosition (1, endPos);
			}
			else{
				Debug.LogError("sniperBulletFXPrefab's linerenderer is missing.");
			}
		}
			
		else {
			Debug.LogError("sniperBulletFX3Prefab is misssing");
		}
		
		
	}



}
