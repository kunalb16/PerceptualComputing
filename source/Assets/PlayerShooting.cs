using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {


	float coolDown =0;
	WeaponData weaponData;
	ActivateActionWeaponChange weaponChange;

	FXManager fxManager;

	void Start(){
		fxManager = GameObject.FindObjectOfType<FXManager> ();

		if (fxManager == null) {
			Debug.LogError("Couldn't find fxManager");		
		}

	}
	// Update is called once per frame
	void Update () {
		coolDown -= Time.deltaTime*5;

		if (Input.GetButtonDown ("Fire1")) {

			//Player wants to shoot...so, Shoot.
			Fire();

		}
	}
	public void Fire(){

		if (weaponChange == null) {
			weaponChange = gameObject.GetComponentInChildren<ActivateActionWeaponChange> ();
			if (weaponChange == null) {
				Debug.LogError("Did not find any WeaponChange in our children");
				return;
			}
		}
		if (weaponData == null) {
			weaponData = gameObject.GetComponentInChildren<WeaponData> ();
			if (weaponData == null) {
				Debug.LogError("Did not find any WeaponData in our children");
				return;
			}
		}
		if (coolDown > 0) {
			return;		
		}

		Debug.Log ("Firing our gun!");

		Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
		Transform hitTransform;
		Vector3 hitPoint;

		hitTransform = FindClosestHitObject (ray, out hitPoint);
		//Debug.Log (hitPoint);
		if (hitTransform != null) {

			Debug.Log ("We hit!" + hitTransform.name);

			Health h = hitTransform.GetComponent<Health> ();

			while (h == null && hitTransform.parent) {
					hitTransform = hitTransform.parent;
					h = hitTransform.GetComponent<Health> ();
			}

			if (h != null) {
				//h.TakeDamage(damage);
				PhotonView pv = h.GetComponent<PhotonView> ();
				if (pv == null) {
					Debug.LogError ("Error");
				} 
				else {

					//	Debug.Log (transform.position);

					if(weaponChange.currentWeapon==0)
						h.GetComponent<PhotonView> ().RPC ("TakeDamage", PhotonTargets.AllBuffered, weaponData.damage1,hitPoint);
					else if(weaponChange.currentWeapon==1)
						h.GetComponent<PhotonView> ().RPC ("TakeDamage", PhotonTargets.AllBuffered, weaponData.damage2,hitPoint);
					else if(weaponChange.currentWeapon==2)
						//Debug.Log("weaponData.damage3:"+weaponData.damage3);
						h.GetComponent<PhotonView> ().RPC ("TakeDamage", PhotonTargets.AllBuffered, weaponData.damage3,hitPoint);
					//Debug.Log("Test");
				}
			}
					
			if (fxManager != null) {
				DoGunFX(hitPoint, weaponChange.currentWeapon);
			}
		}
		else {
			hitPoint = Camera.main.transform.position + (Camera.main.transform.forward*100f);
			DoGunFX(hitPoint, weaponChange.currentWeapon);
		}

		if(weaponChange.currentWeapon==0)
			coolDown = weaponData.fireRate1;
		else if(weaponChange.currentWeapon==1)
			coolDown = weaponData.fireRate2;
		else if(weaponChange.currentWeapon==2)
			coolDown = weaponData.fireRate3;
	}

	void DoGunFX(Vector3 hitPoint, int weapon){
		if(weapon==0)
			fxManager.GetComponent<PhotonView> ().RPC ("SniperBulletFX", PhotonTargets.All, weaponData.transform.position, hitPoint);
		else if(weapon==1)
			fxManager.GetComponent<PhotonView> ().RPC ("SniperBulletFX2", PhotonTargets.All, weaponData.transform.position, hitPoint);
		else if(weapon==2)
			fxManager.GetComponent<PhotonView> ().RPC ("SniperBulletFX3", PhotonTargets.All, weaponData.transform.position, hitPoint);
	}

	Transform FindClosestHitObject(Ray ray, out Vector3 hitPoint){

		RaycastHit[] hits = Physics.RaycastAll (ray);

		Transform closestHit = null;
		float distance = 0;
		hitPoint = Vector3.zero;

		foreach (RaycastHit hit in hits) {

			if(hit.transform != this.transform && (closestHit == null || hit.distance <distance)){

				closestHit = hit.transform;
				distance = hit.distance;
				hitPoint = hit.point;

			}
				
		}

		return closestHit;
	}
}
