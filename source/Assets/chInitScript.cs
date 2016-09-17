using UnityEngine;
using System.Collections;

public class chInitScript : MonoBehaviour {
	public Transform myprefab;

	// Use this for initialization
	void Start () {
		Instantiate (myprefab,Vector3.zero,Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
