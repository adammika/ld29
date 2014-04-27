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
				Color fadeColor = new Color();
				fadeColor.r = 27 / 255.0f;
				fadeColor.g = 38 / 255.0f;
				fadeColor.b = 50 / 255.0f;
				fadeColor.a = 1.0f;
				AutoFade.LoadLevel("GameScene", 1.0f, 1.0f, fadeColor);
			}
		}
	}
}
