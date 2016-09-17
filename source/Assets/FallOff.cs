using UnityEngine;
using System.Collections;

public class FallOff : MonoBehaviour {
	public Health td;
	void FixedUpdate() {
		if (transform.position.y < -50) {
			td.Die();
		}
	}

}
