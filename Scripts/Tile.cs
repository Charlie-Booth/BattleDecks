using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public bool current = false; //is current if something is stood on it
    public bool target = false;
    // state of the tile
    public bool selectable = false;
    public bool dashSelectable = false;
    public bool attackSelectable = false;
    public bool attackingSelected =false;
    public bool attackingSquare = false;
    public Color grassGreen = new Color(0.59f, 0.98f, 0.59f, 1);
    public Color blueSelect;
    public Color dashSelect;
    public Color attackSelect;
    //list of adjacent tiles
    public List<Tile> adjacencyList = new List<Tile>();
    public List<Tile> attackAdjList = new List<Tile>();

    public Movement unit;
    public Movement currentActiveTurn;
    public EnemyController enemyController;

    public bool visited = false;
    public bool unitFound = false;
    public Tile parent = null;
    public int distance = 0;


    public float f = 0; // cost (g+h)
    public float g = 0; // cost from parent to current tile
    public float h = 0; // heuristic cost (from the processed tile to the destination)


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // changes the colour of the tile
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
        else if (selectable && !dashSelectable)
        {
            GetComponent<Renderer>().material.color = blueSelect;
        }
        else if (dashSelectable&& !selectable)
        {
            GetComponent<Renderer>().material.color = dashSelect;
        }
        else if(dashSelectable && selectable)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (attackSelectable)
        {
            GetComponent<Renderer>().material.color = attackSelect;
            
        }
        
        else
        {
            GetComponent<Renderer>().material.color = grassGreen;
        }
        
    }
    public void Reset() // resets the tile and removes it from the adjacency list
    {
        adjacencyList.Clear();
        unit = null;
        current = false;
        target = false;
        selectable = false;
        dashSelectable = false;
        attackSelectable = false;
        attackingSquare = false;
        attackSelectable = false;


        currentActiveTurn = null;
        unit = null;

        visited = false;
        parent = null;
        distance = 0;

        f = 0;
        g = 0;
        h = 0;

    }

    public void FindNeighbours(float jumpHeight, Tile target) // checks in 4 directions of the current tile
    {
        Reset();
        CheckTile(Vector3.forward,jumpHeight,target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        
    }

    public void CheckTile(Vector3 dir, float jumpHeight, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f); // finds the halfway out from the tile
        Collider[] colliders = Physics.OverlapBox(transform.position + dir, halfExtents); // adds any colliders to an array

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>(); // if the item has a tile
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;

                if (attackingSelected) // if attacking has been selected
                {
                    //selectable = false;
                    if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) && unit == null) //if there is nothing on top of this tile it is attack selectable and added to the adjacency list
                    {
                        adjacencyList.Add(tile);
                    }
                    else if (Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1)) // if the raycast hits a collider
                    {
                        if (hit.collider.GetComponent<Movement>()) // if the raycast hits a movement unit its added to the adjacency list
                        {
                            //tile.selectable = false;
                            adjacencyList.Add(tile); 
                           
                            if (attackingSquare)
                            {
                                //unit = hit.collider.GetComponent<Movement>();
                                //enemyController = unit.gameObject.GetComponent<EnemyController>();
                                //attackingSquare = false;
                            }
                        }
                    }
                }

                else if (!Physics.Raycast(tile.transform.position,Vector3.up,out hit, 1)&& unit == null|| (tile == target)) // if the raycast hits nothing it is added to the adjacency list of this tile
                {
                    adjacencyList.Remove(tile);
                    adjacencyList.Add(tile);
                }
                
               
            }

        }
    }
    
}
