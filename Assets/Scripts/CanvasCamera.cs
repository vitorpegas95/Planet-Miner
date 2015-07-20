using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CanvasCamera : MonoBehaviour 
{
	
	
	void Start () 
	{
		
	}
	
	void Update () 
	{
        if (GetComponent<Canvas>().worldCamera == null)
        {
            GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
	}
}
