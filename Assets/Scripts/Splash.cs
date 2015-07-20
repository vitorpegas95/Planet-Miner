using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour 
{
    public float time = 2f;
	
	void Start () 
	{
        Invoke("exit", time);
	}

    void exit()
    {
        Application.LoadLevel(1);
    }
	
	void Update () 
	{
		
	}
}
