using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour 
{
    public Transform player;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    private Camera cam;

	void Start () 
	{
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GetComponent<Camera>();
	}
	
	void Update () 
	{
        if (player)
        {
            Vector3 point = cam.WorldToViewportPoint(player.position);
            Vector3 delta = player.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
	}
}
