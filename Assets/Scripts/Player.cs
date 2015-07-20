using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour 
{
    private Rigidbody2D body;
    public float upSpeed = 6f;
    public float sideSpeed = 4f;
    public float drillSpeed = 0.01f;
    public float maxSpeed;

    public int maxStorage = 50;

    private Dictionary<string, int> tier = new Dictionary<string, int>();

    public Mineral selectedMineralBottom;
	public Mineral selectedMineralLeft;
	public Mineral selectedMineralRight;

	private Vector2 rcBottom;
	private Vector2 rcLeft;
	private Vector2 rcRight;

    private RaycastHit2D hitLeft;
    private RaycastHit2D hitRight;
    private RaycastHit2D hitBottom;

	private float rcDist = 0.01f;
    private float nullDist = 0.16f;

    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    public int money = 100;
    public float altitude = 0;
    public int fuel = 100;

    public int fuelConsumption = 10;
    public float fuelConsTime = 4f;

    public Text txtAlt;
    public Text txtMoney;
    public Text txtFuel;
    public Text txtInv;
    public Text txtSell;

    private Matrix m;
    private int sum = 0;

    public GameObject DieMenu;
    public Text txtScore;

    public bool allowMovement = true;

    public bool dead = false;


	void Start ()
    {
        GameController.gc.player = this;
        body = GetComponent<Rigidbody2D>();
        //ResetInventory(); 
        InvokeRepeating("ConsumeFuel", 2f, fuelConsTime);

        txtInv = GameController.gc.texts[0];
        txtSell = GameController.gc.texts[1];

        tier.Add("drill", 0);
        tier.Add("fuel", 0);
        tier.Add("engine", 0);
        tier.Add("storage", 0);
	}

    public void AllowMovement(bool allow)
    {
        allowMovement = allow;
    }

    public void ResetInventory()
    {
        m = GameObject.FindWithTag("MainCamera").GetComponent<Matrix>();

        if (m.blocks.Count == 0)
        {
            //Invoke("ResetInventory", 1f);
            Debug.Log("No blocks -.-");
            return;
        }

        for (int i = 1; i < m.blocks.Count; i++)
        {
            inventory.Add(m.blocks[i], 0);
        }

        InvokeRepeating("UpdateTexts", 0f, 0.3f);
    }

	void Movement()
	{
        if(allowMovement)
        {
            if (body.velocity.magnitude > maxSpeed)
                body.velocity = new Vector2(maxSpeed, maxSpeed);

            if (Input.GetKey(KeyCode.D))
            {
                body.AddForce(new Vector2(sideSpeed, 0));
                if (selectedMineralRight != null && inventory.Count < maxStorage)
                {
                    selectedMineralRight.Drilled(drillSpeed, "right");
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                body.AddForce(new Vector2(-sideSpeed, 0));
                if (selectedMineralLeft != null && inventory.Count < maxStorage)
                {
                    selectedMineralLeft.Drilled(drillSpeed, "left");
                }
            }

            if (Input.GetKey(KeyCode.W))
            {
                body.AddForce(new Vector2(0, upSpeed));

            }

            if (Input.GetKey(KeyCode.S))
            {
                if (selectedMineralBottom != null && inventory.Count < maxStorage)
                {
                    selectedMineralBottom.Drilled(drillSpeed, "top");
                }
            }
        }
	}

    public void AddMineral(string mineral)
    {
        inventory[mineral]++;
    }

	void CorrectRotation()
	{
		if(!transform.rotation.Equals(Quaternion.Euler(0, 0, 0)))
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}

	void ShootRaycast(string direction)
	{
		if(direction.Equals("left"))
		{
			rcLeft = new Vector2(transform.position.x - 0.16f, transform.position.y);
			hitLeft = Physics2D.Raycast(rcLeft, Vector2.left, rcDist);

            if (hitLeft.collider != null)
            {
                if(hitLeft.transform.tag.Equals("Mineral") && Vector2.Distance(transform.position, hitLeft.point) < 0.25f)
                {
                    if(!hitLeft.transform.gameObject.Equals(selectedMineralLeft))
                    {
                        selectedMineralLeft = hitLeft.transform.gameObject.GetComponent<Mineral>();
                    }
                }
                else
                {
                    selectedMineralLeft = null;
                }
            }


            if(selectedMineralLeft != null)
            {
                if (Vector2.Distance(rcLeft, new Vector2(selectedMineralLeft.transform.position.x, selectedMineralLeft.transform.position.y)) > nullDist)
                {
                    selectedMineralLeft = null;
                }
            }
            
		}
		else if(direction.Equals("right"))
		{
            rcRight = new Vector2(transform.position.x + 0.16f, transform.position.y);
            hitRight = Physics2D.Raycast(rcRight, Vector2.right, rcDist);

            

            if (hitRight.collider != null)
            {
                /*
                Debug.Log(hitRight.transform.tag.Equals("Mineral"));
                Debug.Log(Vector2.Distance(transform.position, hitRight.point));
                Debug.Log(!hitRight.transform.gameObject.Equals(selectedMineralRight));
                */

                if (hitRight.transform.tag.Equals("Mineral") && Vector2.Distance(transform.position, hitRight.point) < 0.32f)
                {
                    if (!hitRight.transform.gameObject.Equals(selectedMineralRight))
                    {
                        selectedMineralRight = hitRight.transform.gameObject.GetComponent<Mineral>();
                    }
                }
                else
                {
                    selectedMineralRight = null;
                }
                //Debug.Log(Vector2.Distance(rcRight, new Vector2(selectedMineralRight.transform.position.x, selectedMineralRight.transform.position.y)));
            }

            if(selectedMineralRight != null)
            {
                //Debug.Log(Vector2.Distance(rcRight, new Vector2(selectedMineralRight.transform.position.x, selectedMineralRight.transform.position.y)));
                if (Vector2.Distance(rcRight, new Vector2(selectedMineralRight.transform.position.x, selectedMineralRight.transform.position.y)) > nullDist)
                {
                    selectedMineralRight = null;
                }
            }
		}
		else if(direction.Equals("bottom"))
		{
            rcBottom = new Vector2(transform.position.x, transform.position.y - 0.32f);
            hitBottom = Physics2D.Raycast(rcBottom, -Vector2.up, rcDist);

            if (hitBottom.collider != null)
            {
                if (hitBottom.transform.tag.Equals("Mineral") && Vector2.Distance(rcBottom, hitBottom.point) < 0.005f)
                {
                    if (!hitBottom.transform.gameObject.Equals(selectedMineralBottom))
                    {
                        selectedMineralBottom = hitBottom.transform.gameObject.GetComponent<Mineral>();
                    }
                }
                else
                {
                    selectedMineralBottom = null;
                }
            }

            if(selectedMineralBottom != null)
            {
                if (Vector2.Distance(rcBottom, new Vector2(selectedMineralBottom.transform.position.x, selectedMineralBottom.transform.position.y)) > nullDist)
                {
                    selectedMineralBottom = null;
                }
            }
            
		}

        
	}
	

    void UpdateTexts() 
    {
        txtAlt.text = "Alt: " + Mathf.Round(altitude*10).ToString() + "m";
        txtMoney.text = money.ToString() + "$";
        txtFuel.text = fuel.ToString() + "%";

        txtInv.text = "Inventory: \n";


        sum = 0;

        for (int i = 1; i < m.blocks.Count; i++)
        {
            int value = 0;
            inventory.TryGetValue(m.blocks[i], out value);
            txtInv.text += "\n " + m.blocks[i] + " x" + value.ToString() + "  ---------------" + (GameController.gc.prices[m.blocks[i]] * value).ToString();

            sum += (GameController.gc.prices[m.blocks[i]] * value);
        }

        txtSell.text = "Sell (" + sum + "$)";
    }

    public void BuyFuel(int percentage, int price)
    {
        if(money >= price)
        {
            if(fuel + percentage > 100)
            {
                fuel = 100;
            }
            else
            {
                fuel += percentage;
            }

            money -= price;
        }
    }

    public void Upgrade(string u, int t, int price)
    {
        if(tier[u] < t && money >= price)
        {
            //Can Upgrade!
            tier[u] = t;

            if(u.Equals("drill"))
            {
                if(t == 1)
                {
                    drillSpeed = 0.1f;
                }
                else if (t == 2)
                {
                    drillSpeed = 0.2f;
                }
                else if (t == 3)
                {
                    drillSpeed = 1f;
                }
                else if (t == 4)
                {
                    drillSpeed = 3f;
                }
            }
            else if (u.Equals("fuel"))
            {
                if (t == 1)
                {
                    fuelConsTime = 1.5f;
                }
                else if (t == 2)
                {
                    fuelConsTime = 2f;
                }
                else if (t == 3)
                {
                    fuelConsTime = 3f;
                }
                else if (t == 4)
                {
                    fuelConsTime = 5f;
                }
            }
            else if (u.Equals("engine"))
            {
                if (t == 1)
                {
                    upSpeed = sideSpeed = 8f;
                }
                else if (t == 2)
                {
                    upSpeed = sideSpeed = 10f;
                }
                else if (t == 3)
                {
                    upSpeed = sideSpeed = 12f;
                }
                else if (t == 4)
                {
                    upSpeed = sideSpeed = 14f;
                }
            }
            else if (u.Equals("storage"))
            {
                if (t == 1)
                {
                    maxStorage = 100;
                }
                else if (t == 2)
                {
                    maxStorage = 200;
                }
                else if (t == 3)
                {
                    maxStorage = 400;
                }
                else if (t == 4)
                {
                    maxStorage = 1000;
                }
            }
        }
    }

    void UpdateAltitude()
    {
        altitude = Vector2.Distance(transform.position, new Vector2(transform.position.x, 0));
    }

    void ConsumeFuel()
    {
        fuel -= fuelConsumption;

        if (fuel < 1)
        {
            Die();
        }
    }

	void Update () 
	{
        if(!dead)
        {
            Movement();
            CorrectRotation();

            ShootRaycast("bottom");
            ShootRaycast("left");
            ShootRaycast("right");

            UpdateAltitude();
        }
		

        
	}

    public void ShowScore()
    {
        Sell();
        int multiplier = tier["drill"] + tier["engine"] + tier["storage"] + tier["fuel"];
        if (multiplier == 0)
            multiplier = 1;
        int score = money * multiplier;
        
        txtScore.text = "Your score: " + score;
        GameJolt.UI.Manager.Instance.ShowLeaderboards();
    }

    void Die()
    {

        if(!dead)
        {
            Sell();

            bool isSignedIn = GameJolt.API.Manager.Instance.CurrentUser != null;

            int multiplier = tier["drill"] + tier["engine"] + tier["storage"] + tier["fuel"];
            if (multiplier == 0)
                multiplier = 1;
            int score = money * multiplier;
            int scoreValue = score; // The actual score.
            string scoreText = "" + score; // A string representing the score to be shown on the website.
            int tableID = 83851; // Set it to 0 for main highscore table.
            string extraData = ""; // This will not be shown on the website. You can store any information.

            if (isSignedIn)
            {
                GameJolt.API.Scores.Add(scoreValue, scoreText, tableID, extraData, (bool success) =>
                {
                    Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
                    ShowScore();
                });
            }
            else
            {
                string guestName = "Guest";

                GameJolt.API.Scores.Add(scoreValue, scoreText, guestName, tableID, extraData, (bool success) =>
                {
                    Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
                    ShowScore();
                });
            }







            AllowMovement(false);
            DieMenu.SetActive(true);
            dead = true;
        }
        

    }

    public void Reload()
    {
        GameController.gc.LoadScene(0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name.Equals("Gas"))
        {
            GameController.gc.ShowMenu("gas");
        }
        else if (other.name.Equals("Shop"))
        {
            GameController.gc.ShowMenu("shop");
        }
        else if (other.name.Equals("Mechanic"))
        {
            GameController.gc.ShowMenu("mech");
        }

    }

    public void Sell()
    {
        Dictionary<string, int> prices = GameController.gc.prices;

        for(int i = 1; i < m.blocks.Count; i++)
        {
            money += inventory[m.blocks[i]] * prices[m.blocks[i]];
            inventory[m.blocks[i]] = 0;
        }
    }
}
