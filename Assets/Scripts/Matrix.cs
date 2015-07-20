using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Matrix : MonoBehaviour 
{
    public List<string> blocks = new List<string>();
	public Vector2 MAP_SIZE;
	public const int CELL_SIZE = 16;

	public int[,] map;

    public Texture2D[] textures;

    public   int ITERATIONS = 1;

	void Start () 
	{
        CreateBlocks();

        ResetMap();
        
        for (int i = 0; i < ITERATIONS; i++)
        {
            GenerateResources();
            //generateMap();
        }

        for (int x = 0; x < MAP_SIZE.x; x++)
        {
            for (int y = 0; y < MAP_SIZE.y; y++)
            {
                GameObject g = Instantiate(GameController.gc.prefabs[map[x, y]], new Vector3(x * 0.32f, y * -0.32f, 0), Quaternion.identity) as GameObject;

                if (y == 0 && x > 2 && x < 6)
                    g.GetComponent<Mineral>().allowDrill = false;
            }
        }
	}

    public void CreateBlocks()
    {
        blocks.Add("none");
        blocks.Add("dirt");
        blocks.Add("stone");
        blocks.Add("tin");
        blocks.Add("copper");
        blocks.Add("lead");
        blocks.Add("iron");
        blocks.Add("silver");
        blocks.Add("nickel");
        blocks.Add("coal");
        blocks.Add("gold");
        blocks.Add("titanium");
        blocks.Add("chrome");
        blocks.Add("thungsten");
        blocks.Add("emerald");
        blocks.Add("diamond");
        GameController.gc.player.ResetInventory();
    }

    public int getBlockID(string name)
    {
        for(int i = 0; i < blocks.Count; i++)
        {
            if(blocks[i].Equals(name))
            {
                return i;
            }
        }

        return 0;
    }

    public void ResetMap()
    {
        map = new int[(int)MAP_SIZE.x, (int)MAP_SIZE.y];

        for (int i = 0; i < MAP_SIZE.x; i++)
        {
            for (int j = 0; j < MAP_SIZE.y; j++)
            {
                if(j < 2)
                {
                    map[i, j] = getBlockID("dirt");
                }
                else
                {
                    map[i, j] = getBlockID("stone");
                }
            }
        }
    }

    public void GenerateResources()
    {
        for (int x = 0; x < MAP_SIZE.x; x++)
        {
            for (int y = 2; y < MAP_SIZE.y; y++)
            {
                int depth = y;

                //Generate block for i,j
                if(map[x,y] == getBlockID("stone"))
                {
                    int block = 0;
                    for(int i = 0; i < 3; i++)
                    {
                        block = GenerateBlock(depth); 
                        if(Distance(new Vector2(x,y), blocks[block], 5))
                        {
                            block = GenerateBlock(depth);    
                        }
                    }

                    map[x, y] = block; 
                }
            }
        }
    }

    public int FindBlockForRandom(int r)
    {
        for(int i = 0; i < blocks.Count; i++)
        {
            int blockRatio = (i+2) * 2;
            int randomRangeSelect = Random.Range(1, r);

            int result = randomRangeSelect / blockRatio;

            if(result > 1)
                return i;
        }

        return 0;
    }

    public void generateMap()
    {
        int a1 = ((int)(MAP_SIZE.y) / 6);  // ratio de layers c/stone , emeralds e diamonds
        int a2 = ((int)(MAP_SIZE.y) / 6); // ratio de layers c/ iron , coal
        int a3 = ((int)(MAP_SIZE.y)/ 20); // ratio de layers c/ dirt

        for (int c = 0; c < MAP_SIZE.x; ++c)
        {
            for (int i = 0; i < MAP_SIZE.y; ++i)
            {
                if (i >= 0 && i < a3) map[c, i] = 1;
                if (i >= a3 && i < a3 + a1) map[c, i] = 2;
                int random = Random.Range(0, 100);
                if (i >= a3 + a1 && i < 2 + a1 + a2) map[c, i] = (random > 15) ? GenerateBlock(i) : 3;
                if (i >= a3 + a1 + a2 && i < 2 + a1 + a2 + a2) map[c, i] = (random > 10) ? GenerateBlock(i) : 4;
                if (i >= a3 + a1 + a2 + a2 && i < 2 + a1 + a2 + a2 + a1) map[c, i] = (random > 7) ? GenerateBlock(i) : 5;
                if (i >= a3 + a1 + a2 + a2 + a1 && i < 2 + a1 + a2 + a2 + a1 + a1) map[c, i] = (random > 3) ? GenerateBlock(i) : 6;
            }
        }
    }


    public int GenerateBlock(int depth)
    {
        int random = Random.Range(0, ( 100 * depth / (int)MAP_SIZE.y ));

        int block = FindBlockForRandom(random);

        block+=2;

        if(block >= blocks.Count-1)
        {
            block = getBlockID("stone") ;
        }

        return block;
    }

    public bool Distance(Vector2 pos, string block, int distance)
    {
        bool rtn = false;
        for (int i = (int)pos.x - distance; i < (int)pos.x + distance; i++)
        {
            for (int j = (int)pos.y; j < (int)pos.y + distance; j++)
            {
                if(i < MAP_SIZE.x && j < MAP_SIZE.y && i >= 0 && j >= 0)
                {
                    if(map[i,j] == getBlockID(block))
                    {
                        rtn = true;
                        break;
                    }
                }
            }
        }

        return rtn;
    }

	
    
    /*
	void OnGUI()
	{
		for(int i = 0; i < MAP_SIZE.x; i++)
		{
			for(int j = 0; j < MAP_SIZE.y; j++)
			{
                GUI.Label(new Rect(0 + (i * CELL_SIZE), 0 + (j * CELL_SIZE), CELL_SIZE, CELL_SIZE), textures[map[i,j]]);
			}
		}
	}
    */
}
