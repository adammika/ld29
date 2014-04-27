#define USE_BLIP


using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[SerializeField] private GameObject rootPrefab;
	private ArrayList _visRoots;
	private float _movementNoise = 0.02f;

	// Use this for initialization
	void Start () {
		_visRoots = new ArrayList();

		GameObject newRoot = Instantiate(rootPrefab) as GameObject;
		newRoot.transform.position = this.transform.position;
		newRoot.transform.parent = this.transform;
		_visRoots.Add(newRoot);
	}

	void FixedUpdate() {
		if (GameManager._stopped) return;

		float move = Input.GetAxis("Horizontal");

		float newRootY = this.transform.position.y;
		float minY = float.MaxValue;

		ArrayList rootsToDestroy = new ArrayList();

		foreach (GameObject root in _visRoots) {
			// This is no longer the first root, remove the rigidBody and collider if present
			Destroy(root.rigidbody2D);
			Destroy (root.collider2D);

			root.transform.Translate(-(Vector3.down * Time.deltaTime));
			root.transform.Translate(Vector3.left * move * Time.fixedDeltaTime);

			if (root.transform.position.y < minY) {
				minY = root.transform.position.y;
			} 

			if (root.transform.position.y > GameManager._height) {
				rootsToDestroy.Add(root);
			}
		}

		foreach (GameObject rootToDestroy in rootsToDestroy) {
			_visRoots.Remove(rootToDestroy);
			DestroyObject(rootToDestroy);
		}

		if (minY >= newRootY) {
			GameObject newRoot = Instantiate(rootPrefab) as GameObject;
			newRoot.transform.position = new Vector2(this.transform.position.x + Random.Range(-_movementNoise, _movementNoise),
			                                         minY - newRoot.renderer.bounds.size.y / 2.0f);
			newRoot.transform.parent = this.transform;
			_visRoots.Add(newRoot);
		}
	}
}