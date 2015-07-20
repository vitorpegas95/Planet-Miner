using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameController : MonoBehaviour 
{
    public static GameController gc;

    public Player player;

    public Dictionary<string, int> prices = new Dictionary<string, int>();

    public GameObject[] prefabs;

    public Dictionary<string, GameObject> menus = new Dictionary<string,GameObject>();

    public List<Text> texts = new List<Text>();

	void Awake()
    {
        if(gc == null)
        {
            gc = this;
            DontDestroyOnLoad(this);
        }
        else if(gc != this)
        {
            Destroy(gameObject);
        }
    }

    public void ShowMenu(string menu)
    {
        menus[menu].SetActive(true);
        player.AllowMovement(false);
    }

    public void HideMenu(string menu)
    {
        menus[menu].SetActive(false);
        player.AllowMovement(true);
    }

	void Start ()
    {
        if(player == null)
        {
            if(GameObject.FindWithTag("Player") != null)
            {
                player = GameObject.FindWithTag("Player").GetComponent<Player>() ;
            }
        }
        SetPrices();

        GameObject panel = GameObject.Find("panelGas");

        menus.Add("gas", panel);
        menus["gas"].SetActive(false);

        panel = GameObject.Find("panelShop");

        menus.Add("shop", panel);
        texts.Add(GameObject.Find("txtInv").GetComponent<Text>());
        texts.Add(GameObject.Find("txtBtnSell").GetComponent<Text>());
        menus["shop"].SetActive(false);


        panel = GameObject.Find("panelMechanic");
        menus.Add("mech", panel);
        menus["mech"].SetActive(false);
	}

    void SetPrices()
    {
        prices.Add("dirt", 5);
        prices.Add("stone", 10);
        prices.Add("tin", 23);
        prices.Add("copper", 25);
        prices.Add("lead", 30);
        prices.Add("iron", 54);
        prices.Add("silver", 70);
        prices.Add("nickel", 75);
        prices.Add("coal", 80);
        prices.Add("gold", 120);
        prices.Add("titanium", 130);
        prices.Add("chrome", 140);
        prices.Add("thungsten", 145);
        prices.Add("emerald", 300);
        prices.Add("diamond", 500);
    }
	
    public void Upgrade(string u)
    {
        player.Upgrade(u.Split(',')[0], int.Parse(u.Split(',')[1]), int.Parse(u.Split(',')[2]));
    }

    public void SellInventory()
    {
        player.Sell();
    }

    public void BuyFuel(string s)
    {
        
        player.BuyFuel(int.Parse(s.Split(',')[0]), int.Parse(s.Split(',')[1]));
    }

    public void LoadScene(int id)
    {
        Application.LoadLevel(id);
    }
}
