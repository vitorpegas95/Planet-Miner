using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour 
{
    public bool isSigned;

    public Button btnLogin;
	
	void Start () 
	{
		isSigned = GameJolt.API.Manager.Instance.CurrentUser != null;
	}
	
	void Update () 
	{
        btnLogin.interactable = !isSigned;
	}
}
