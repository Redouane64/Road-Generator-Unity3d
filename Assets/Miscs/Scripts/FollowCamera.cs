using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    public float damping = 4f;
    public Transform target;
    public float distance = 9f;
    public float height = 3f;

    private Vector3 cameraPosition;
    private Vector3 newFollowPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!target)
            return;

        cameraPosition = new Vector3(0, -height, distance);
        newFollowPosition = target.position - cameraPosition;
        transform.position = Vector3.Slerp(transform.position, newFollowPosition, damping);
	}
}
