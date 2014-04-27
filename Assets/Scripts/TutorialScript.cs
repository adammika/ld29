using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			if (this.tag == "Tutorial1") {
				Application.LoadLevel("Tutorial2");
			}
			if (this.tag == "Tutorial2") {
				Application.LoadLevel("Tutorial3");
			}
			if (this.tag == "Tutorial3") {
				Application.LoadLevel("GameScene");
			}
		}
	}
}
