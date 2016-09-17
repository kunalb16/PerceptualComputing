using UnityEngine;
using System.Collections;

public class WeaponChange : MonoBehaviour {


	public int currentWeapon=0;
	public Transform[] weapons;
//	public GameObject[] crosshair;
// Use this for initialization


	void Start () {
		changeWeapon (0);
				
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("1")) {
								changeWeapon (0);
						} else if (Input.GetKeyDown ("2")) {
								changeWeapon (1);
						} else if (Input.GetKeyDown ("3")) {
								changeWeapon (2);
						}
					}
	public void changeWeapon(int num) {
		currentWeapon = num;
		for(int i = 0; i < weapons.Length; i++) {
			if(i == num){
				weapons[i].gameObject.SetActive(true);
			}
			else{
				weapons[i].gameObject.SetActive(false);
			}
		}
	}
	
	
	
	
	}

