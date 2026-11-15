using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public Item itemPrefab;
    public NioController NioPrefab;
    public ItemType[] itemTypes;
    int itemTypeWeightSum;
    public List<Item> loot = new List<Item>();
    public Tilemap floorTmap;
    public Tilemap wallTmap;
    public Tilemap trailTmap;
    public PlayerController player;
    public GoodDict<TileBase> tiles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemTypeWeightSum = 0;
        for(int i = 0; i < itemTypes.Length; i++)
        {
            itemTypeWeightSum += itemTypes[i].weight;
        }
        InitializeLevel();
    }
    void InitializeLevel()
    {
        long pathTilesPlaced = 0;
        Walker[] walkers = new Walker[5];
        int walkerCount = 2;
        for (int i = 0; i < walkers.Length; i++)
        {
            if(i < walkerCount)
            {
                walkers[i] = new Walker(Vector3Int.zero);
            } else {
                walkers[i] = new Walker();
            }
        }
        float angCos;
        float angSin;
        float dirRange;
        while(pathTilesPlaced < 200000)
        {
            foreach(Walker walker in walkers)
            {
                if (walker.exists)
                {
                    for(int i = -8; i < 8; i++)
                    {
                        for(int j = -13; j < 13; j++)
                        {
                            if(wallTmap.GetTile(walker.position + new Vector3Int(i, j, 0)) == null)
                            {
                                wallTmap.SetTile(walker.position + new Vector3Int(i, j, 0), tiles.get("wall"));
                            }
                        }
                    }
                    bool madeItem = false;
                    for (int i = -3; i <= 3; i++)
                    {
                        for (int j = -3; j <= 3; j++)
                        {
                            if ((i < 3 || j % 2 == 0) && wallTmap.GetTile(walker.position + new Vector3Int(i, j, 0)) != tiles.get("g"))
                            {
                                if (Global.RandomChance(10))
                                {
                                    floorTmap.SetTile(walker.position + new Vector3Int(i, j, 0), tiles.get("floorAlt"+UnityEngine.Random.Range(1,3)));
                                } else
                                {
                                    floorTmap.SetTile(walker.position + new Vector3Int(i, j, 0), tiles.get("floor"));
                                }
                                wallTmap.SetTile(walker.position + new Vector3Int(i, j, 0), tiles.get("g"));
                                pathTilesPlaced++;
                                if (!madeItem && Global.RandomChance(1000))
                                {
                                    Item newItem = Instantiate(itemPrefab);
                                    int weight = UnityEngine.Random.Range(0, itemTypeWeightSum);
                                    int w = 0;
                                    while (itemTypes[w].weight < weight && w < itemTypes.Length - 1)
                                    {
                                        weight -= itemTypes[w].weight;
                                        w++;
                                    }
                                    newItem.Initialize(itemTypes[w].sprite, UnityEngine.Random.Range(itemTypes[w].minCost, itemTypes[w].maxCost));
                                    newItem.transform.position = floorTmap.GetCellCenterWorld(walker.position + new Vector3Int(i, j, 0));
                                    madeItem = true;
                                    loot.Add(newItem);
                                }
                                if (Global.RandomChance(3000))
                                {
                                    NioController newNio = Instantiate(NioPrefab);
                                    newNio.transform.position = floorTmap.GetCellCenterWorld(walker.position + new Vector3Int(i, j, 0));
                                    newNio.manager = this;
                                }
                                //Debug.Log(walker.position + new Vector3Int(i, j, 0));
                            }
                        }
                    }
                    // I create a number from one to (cos(angle) + sin(angle)) and if it's less than cos(angle) it moves in the x direction, otherwise it moves in the y direction
                    angCos = Mathf.Cos(walker.angle);
                    angSin = Mathf.Sin(walker.angle);
                    dirRange = Mathf.Abs(angCos) + Mathf.Abs(angSin);
                    if (UnityEngine.Random.Range(0, dirRange) < Mathf.Abs(angCos))
                    {
                        walker.position.x += 2 * Math.Sign(angCos);
                    }
                    else
                    {
                        walker.position.y += 2 * Math.Sign(angSin);
                    }
                    if (Global.RandomChance(20))
                    {
                        walker.angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
                    }
                    if (Global.RandomChance(100))
                    {
                        for(int i = 0; i < walkers.Length; i++)
                        {
                            if (!walkers[i].exists)
                            {
                                walkers[i] = new Walker(walker.position);
                                walkerCount++;
                                break;
                            }
                        }
                    }
                    if (Global.RandomChance(150) && walkerCount>1)
                    {
                        walkerCount--;
                        walker.exists = false;
                    }
                }
            }
        }
        wallTmap.SwapTile(tiles.get("g"), null);
    }

    // Update is called once per frame
    void Update()
    {
        float dist;
        bool newNearest;
        player.nearestItem = null;
        foreach(Item x in loot){
            newNearest = false;
            dist = (x.transform.position - player.transform.position).magnitude;
            if (dist < 1.2f){
                if(player.nearestItem == null || Vector3.Distance(player.nearestItem.transform.position,player.transform.position)>dist)
                {
                    if(player.nearestItem != null)
                    {
                        player.nearestItem.sprite.color = Color.white;
                    }
                    player.nearestItem = x;
                    x.sprite.color = new Color(0.95f, 0.92f, 0.7f);
                    newNearest = true;
                }
            }
            if(!newNearest)
            {
                if (!x.sprite.color.Equals(Color.white))
                {
                    x.sprite.color = Color.white;
                }
            }
        }
    }
}
