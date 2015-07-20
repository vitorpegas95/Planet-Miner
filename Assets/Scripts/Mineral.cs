using UnityEngine;
using System.Collections;

public class Mineral : MonoBehaviour 
{
    public const float minDrillAmount = 0.1f;
    public const float error = 0.03f;

    public float durability;
    public string mineralName;

    public bool allowDrill = true;

    private BoxCollider2D box;
	
	void Start () 
	{
        box = GetComponent<BoxCollider2D>();
        mineralName = gameObject.name.Replace("(Clone)", "").Trim().ToLower();
        box.size = new Vector2(0.32f, 0.32f);
	}

    public void Drilled(float drillAmount, string side)
    {
        if (!allowDrill)
            return;

		if (side.Equals ("top")) 
		{
            if (box.size.y > minDrillAmount + error)
            {
                Vector2 newSize = box.size - new Vector2(0, drillAmount * (1 / durability));
                box.size = newSize;
            }
            else
            {
                Debug.Log("die " + box.size.y);
                Die();
            }
		} 
        else 
		{
            if (box.size.x >minDrillAmount + error)
            {
                Vector2 newSize = box.size - new Vector2(drillAmount * (1 / durability), 0);
                box.size = newSize;
            }
            else
            {
                Die();
            }
		}
    }

    public void LateUpdate()
    {
        if (box.size.y <= minDrillAmount + error)
        {
            Die();
        }

        if (box.size.x <=minDrillAmount + error)
        {
            Die();
        }
    }

    public void Die()
    {
        GameController.gc.player.AddMineral(mineralName);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
