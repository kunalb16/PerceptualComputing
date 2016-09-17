using UnityEngine;
using System.Collections;

public class initGUIText : MonoBehaviour {
	public Texture myText;
	// Use this for initialization
	void Start () {
	
	}
	void OnGUI(){
		GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 20,20),myText);
	
	}
	// Update is called once per frame
	void Update () {
	
	}
}
