using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public static float _width;
	public static float _height;
	public static bool _stopped;
	private ArrayList _visChunks;
	private static float _score;	

	[SerializeField] private GameObject[] _chunkPrefabs;
	[SerializeField] private Sprite[] _chunkBackgrounds;
	[SerializeField] private GUIText _scoreLabel;
	[SerializeField] private GameObject _topObject;
	
	// MonoBehaviour
	
	void Awake() {
		_height = Camera.main.orthographicSize * 2.0f;
		_width = _height * Screen.width / Screen.height;
		
		_visChunks = new ArrayList();
		
		GameObject chunk = GenerateChunk(0);
		chunk.transform.position = new Vector2(0, 0);
		chunk.transform.parent = this.transform;
		_visChunks.Add(chunk);

		_score = 0.0f;
		_stopped = false;
	}

	void FixedUpdate() {
		if (_stopped) return;

		float h = Input.GetAxis("Horizontal");
		this.Move(h);

		_score += Time.deltaTime;
	}

	void OnGUI() {
		this._scoreLabel.text = "SCORE " + (int)_score;
	}

	// Public

	public static void Stop() {
		_stopped = true;

		int score = (int)_score;
		PlayerPrefs.SetInt("GameScore", score);

		int highScore = PlayerPrefs.GetInt("HighScore");
		if (score > highScore) {
			PlayerPrefs.SetInt("HighScore", score);
		}
	}	

	public GameObject GenerateChunk() {
		return this.GenerateChunk(this.GetNumberOfObstaclesToSpawn());
	}

	public GameObject GenerateChunk(int numberOfObstacles) {
		GameObject chunk = new GameObject();
		SpriteRenderer renderer = chunk.AddComponent<SpriteRenderer>();
		renderer.sprite = this.RandomBackgroundSprite();

		for (int i = 0; i < numberOfObstacles; i++) {
			GameObject obj = RandomPrefab();
			obj.transform.position = RandomCoordinate(obj);
			obj.transform.parent = chunk.transform;
		}
		
		return chunk;
	}
	
	// Private
	
	private GameObject RandomPrefab() {
		int randomNumber = Random.Range(0, _chunkPrefabs.Length);
		return Instantiate(_chunkPrefabs[randomNumber]) as GameObject;
	}
	
	private Vector2 RandomCoordinate(GameObject prefab) {
		float x = Random.Range(-(_width / 2.0f), _width / 2.0f);
		float y = Random.Range(-(_height / 2.0f), _height / 2.0f);

		float minX = -(_width / 2.0f) + (prefab.renderer.bounds.size.x / 2.0f);
		float minY = -(_height / 2.0f) + (prefab.renderer.bounds.size.y / 2.0f);

		x = Mathf.Clamp(x, minX, Mathf.Abs(minX));
		y = Mathf.Clamp(y, minY, Mathf.Abs(minY));

		return new Vector2(x, y);
	}
	
	private Sprite RandomBackgroundSprite() {
		int randomNumber = Random.Range(0, _chunkBackgrounds.Length);
		return Instantiate(_chunkBackgrounds[randomNumber]) as Sprite;
	}
	
	private void Move(float move) {

		if (_topObject != null) {
			if (_topObject.transform.position.y > _height) {
				DestroyObject(_topObject);
			} 
			else {
				_topObject.transform.Translate(-(Vector3.down * Time.deltaTime));
				_topObject.transform.Translate(Vector3.left * move * Time.fixedDeltaTime);
			}
		}

		// Move the chunks
		ArrayList chunksToDestroy = new ArrayList();
		ArrayList translatedChunks = new ArrayList();
		foreach (GameObject chunk in _visChunks) {
			
			// Destroy chunks that go off top of screen
			if (chunk.transform.position.y > _height) {
				chunksToDestroy.Add(chunk);
				continue;
			}
			
			chunk.transform.Translate(-(Vector3.down * Time.deltaTime));
			chunk.transform.Translate(Vector3.left * move * Time.fixedDeltaTime);
			translatedChunks.Add(chunk);

//			Debug.Log(string.Format("Chunk Frame: {0},{1} {2}x{3}",
//			                        chunk.renderer.transform.position.x,
//			                        chunk.renderer.transform.position.y,
//			                        chunk.renderer.bounds.size.x,
//			                        chunk.renderer.bounds.size.y));
		}
		
		foreach (GameObject chunkToDestroy in chunksToDestroy) {
			_visChunks.Remove(chunkToDestroy);
			DestroyObject(chunkToDestroy);
		}
		
		_visChunks = translatedChunks;
		
		// Add any chunks to scene if needed
		float minX, minY;
		minX = minY = float.MaxValue;
		float  maxX, maxY;
		maxX = maxY = float.MinValue;
		foreach (GameObject chunk in _visChunks) {
			if (chunk.transform.position.x < minX) {
				minX = chunk.transform.position.x;
			}
			if (chunk.transform.position.y < minY) {
				minY = chunk.transform.position.y;
			}
			if (chunk.transform.position.x + _width > maxX) {
				maxX = chunk.transform.position.x + _width;
			}
			if (chunk.transform.position.y > maxY) {
				maxY = chunk.transform.position.y;
			}
		}
		
//		Debug.Log(string.Format("MinX: {0} MinY: {1} MaxX: {2} MaxY: {3}", minX, minY, maxX, maxY));
		
		if (minX > 0) {
			// Create chunk to left
			GameObject chunk = this.GenerateChunk();
			chunk.transform.position = new Vector2(minX - _width, maxY);
			chunk.transform.parent = this.transform;
			_visChunks.Add(chunk);
			
			float y = maxY - _height;
			while (y >= minY) {
				GameObject chunk2 = this.GenerateChunk();
				chunk2.transform.position = new Vector2(minX - _width, y);
				chunk2.transform.parent = this.transform;
				_visChunks.Add(chunk2);
				y -= _height;
			}
			
			minX -= _width;
		}
		if (maxX < _width) {
			// Create chunk to right
			GameObject chunk = this.GenerateChunk();
			chunk.transform.position = new Vector2(maxX, maxY);
			chunk.transform.parent = this.transform;
			_visChunks.Add(chunk);
			
			float y = maxY - +_height;
			while (y >= minY) {
				GameObject chunk2 = this.GenerateChunk();
				chunk2.transform.position = new Vector2(maxX, y);
				chunk2.transform.parent = this.transform;
				_visChunks.Add(chunk2);
				y -= _height;
			}
		}
		if (minY > 0) {
			
			float x = minX;
			while (x + _width < Camera.main.transform.position.x) {
				x += _width;
			}
			
			GameObject chunk = this.GenerateChunk();
			chunk.transform.position = new Vector2(x, minY - _height);
			chunk.transform.parent = this.transform;
			_visChunks.Add(chunk);
			
			if (minX + _width < maxX) {
				GameObject chunk2 = this.GenerateChunk();
				chunk2.transform.position = new Vector2(x + _width, minY - _height);
				chunk2.transform.parent = this.transform;
				_visChunks.Add(chunk2);
			}
		}
	}

	private int GetNumberOfObstaclesToSpawn() {
		int additionalObstaclesFromDifficulty = (int)(_score / 10.0f);
		return Random.Range(5, 8) + additionalObstaclesFromDifficulty;
	}
}
