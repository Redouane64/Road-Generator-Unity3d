using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadGenerator : MonoBehaviour {

	// road chunk prefab
	public GameObject roadChunk;

	// distance between edges of the chunk.
	public float chunkLength;

	// number of chunks to be activated at a time.
	public int drawingAmount = 3;

	// reference to player object. it is required to manage chunks on the scene.
	[SerializeField]private Transform player = null;

	// total number of chunks that actually exist in the scene
	[SerializeField]private int numberOfChunks = 7;

	// list of references to chunks in the scence
	private Queue<Transform> chunks;

	// reference to chunk that the player is on
	private Transform currentChunk;
	private int indexOfCurrentChunk;
	private int currentChunkPosition = 0;

	private void Awake()
	{
		InitializeChunksList();
	}

	private void InitializeChunksList()
	{
		chunks = new Queue<Transform>();
		for (int i = 0; i < numberOfChunks; i++)
		{
			GameObject _chunk = Instantiate<GameObject>(roadChunk);
			_chunk.transform.position = NextChunkPosition();
			if (i != 0)
				_chunk.SetActive(false);
			chunks.Enqueue(_chunk.transform);
		}
	}
	 
	private void Start () {
		
	}
	
	private void FixedUpdate () {

		if (!player) return;

		// determine the chunk that the player is on
		currentChunk = GetCurrentChunk();
		indexOfCurrentChunk = GetIndexOfCurrentChunk();

		// Manage chunks based on current chunk that the player is on
		for (int i = indexOfCurrentChunk; i < (indexOfCurrentChunk+drawingAmount); i++)
		{
			i = Mathf.Clamp(i, 0, chunks.Count-1);
			GameObject _chunkGO = (chunks.ToArray()[i]).gameObject;
			if (!_chunkGO.activeInHierarchy)
				_chunkGO.SetActive(true);
		}

		if (indexOfCurrentChunk > 0)
		{
			float _distance = Vector3.Distance(player.position, (chunks.ToArray()[indexOfCurrentChunk - 1]).position);
			if(_distance > (chunkLength * .75f))
				SweepPreviousChunk();
		}

	}

	private void SweepPreviousChunk()
	{
		Transform _chunk = chunks.Dequeue();
		_chunk.gameObject.SetActive(false);
		_chunk.position = NextChunkPosition();
		chunks.Enqueue(_chunk);
	}

	private Vector3 NextChunkPosition()
	{
		float _position = currentChunkPosition;
		currentChunkPosition += (int)chunkLength;
		return new Vector3(0, 0, _position);
	} 

	private Transform GetCurrentChunk()
	{
		Transform current_chunk = null;
		foreach (Transform c in chunks)
		{
			if (Vector3.Distance(player.position, c.position) <= (chunkLength / 2))
			{
				current_chunk = c;
				break;
			}
		}
		return current_chunk;
	}

	private int GetIndexOfCurrentChunk()
	{
		int index = -1;
		for (int i = 0; i < chunks.Count; i++)
		{
			if ((chunks.ToArray()[i]).Equals(currentChunk))
			{
				index = i;
				break;
			}
		}
		return index;
	}

}
