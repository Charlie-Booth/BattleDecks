using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{ //Episode 1 out of 6(all 6 were used) of the tutorial series to make this found here https://www.youtube.com/watch?v=cX_KrK8RQ2o by Games Programming academy"Unity Tutorial - Tactics Movement - Part 1", uploaded Nov 9 2017
    // Ep 2 https://www.youtube.com/watch?v=cK2wzBCh9cg
    //Ep 3 https://www.youtube.com/watch?v=2NVEqBeXdBk
    //Ep 4 https://www.youtube.com/watch?v=mTNL0EXU7kU
    //Ep 5 https://www.youtube.com/watch?v=W2OQbHeQs7U
    //Ep 6 https://www.youtube.com/watch?v=3M3FcJ4r0GE
    public bool turn = false;
    public bool attackingSelected = true;
    public bool movingSelected = false;
    public bool dashing = false;
    List<Tile> selectableTiles = new List<Tile>();
    List<Tile> attackTiles = new List<Tile>();
    List<Tile> dashTiles = new List<Tile>();
    List<Tile> outOfSightTiles = new List<Tile>();

    //public bool firstTurn = false;
    
    public GameObject[] tiles;

    public bool Thief;
    public bool Warrior;
    public bool archer;

    Stack<Tile> path = new Stack<Tile>();
    public Tile currentTile;

    public bool moving;
    public bool check;

    public int move = 5;
    public int sightDistance;
    public int dashMove = 7;
    public int meleeRange = 1;
    public int rangedRange = 3;
    public int attackRange = 3;
    public int actions = 2;
    public int initialActions = 2;
    public float jumpHeight = 2f;
    public float moveSpeed = 2f;
    public float jumpDampner = 3.0f;
    public float jumpVelocity = 4.5f;

    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;

    Vector3 jumpTarget;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;

    public Tile actualTargetTile;

    protected void Initialise()
    {
        //sightDistance = dashMove + 1;
        tiles = GameObject.FindGameObjectsWithTag("Tile"); // is added to the array of tiles in the world
        halfHeight = GetComponent<Collider>().bounds.extents.y; // finds half the height of the movements collider

        //TurnManager.AddUnit(this);
    }
    
    public void GetCurrentTile()
    {
        // use this to do attackingTiles
        AssignCurrentUnit();// sets the current tiles unit to this unit
        currentTile.current = true;// sets the tile as current
    }
    public void AssignCurrentUnit()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.unit = this;
        currentTile.enemyController = this.gameObject.GetComponent<EnemyController>();

    }
    public Tile GetTargetTile(GameObject target) // finds the current tile gameobject by shooting a raycast down
    {
        RaycastHit hit;
        Tile tile = null;
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }
    public void ComputeAdjacencyLists(float jumpHeight, Tile target) // finds all the adjacent tiles for one another
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbours(jumpHeight, target);
        }
    }
    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists(jumpHeight, null);// finds adjacent tiles that are in the jump height of one another
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);// adds the current tile to the queue and sets it to visited
        currentTile.visited = true;

        while(process.Count > 0)// while there are still things to process
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);// add the current tile to the selectable tiles list
            t.selectable = true;
            if (t.distance < move)// if the distance of the next tile is less than the move distance
            {
                
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)// if its not been visited
                    {
                        tile.parent = t;// will make the new tiles parent the current tile
                        tile.visited = true;// will visit the new tile
                        tile.distance = 1 + t.distance;// will increase the distance
                        process.Enqueue(tile);// will loop through with another tile to process
                    }
                }
                
            }
        }
        /*if (actions >= 2)
        {
            FindDashableTiles();
        }*/
        
    }
    public void FindMeleeTiles() //essentially the same as finding selectable tiles but instead will only make the tile able to be attacked if the tile has a unit on it
    {
        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            attackTiles.Add(t);
            if (t.unit == null)
            {
                t.attackSelectable = true;
            }
            if (t.distance < attackRange)
            {

                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }
   /* public void FindOutOfSightTiles() // original way of making a fog of war, may return to this in the future
    {
        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            outOfSightTiles.Add(t);
            t.attackSelectable = false;
            if (t.distance < sightDistance )
            {

                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    } */

    public void FindDashableTiles() // works the same as finding selectable tiles but instead will remove tiles already found in the selectable tiles list
    {
        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            dashTiles.Add(t);
            t.dashSelectable = true;
            if (t.distance < dashMove )
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
            foreach (Tile tile in selectableTiles)
            {
               
                tile.selectable = true;
                if (tile.dashSelectable)
                {
                    dashTiles.Remove(tile);
                    tile.dashSelectable = false;
                }
            }
        }
    }
    public void MoveToTile(Tile tile)
    {
        path.Clear();// clears the stack of tiles to get tot the target tile
        tile.target = true;
        moving = true;
        Tile next = tile;
        
        while (next != null)// if the next tile isnt empty, it will push the next tile onto the stack and make the parent the next tile 
        {
            path.Push(next);
            next = next.parent;
        }
    }
    public void Move()
    {
        if(path.Count > 0) // if the stack isnt empty
        {
            Tile t = path.Peek();// finds the first tile
            Vector3 target = t.transform.position;
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y; // finds if it is within the jumping range

            if(Vector3.Distance(transform.position, target)>= 0.05f)
            {
                bool jump = transform.position.y != target.y;
                if (jump)
                {
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
                
            }
            else
            {
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveDashableTiles();
            RemoveSelectableTiles();
            RemoveAttackTiles(); 
            
            moving = false;

            if (dashing)
            {
                actions -= 2;
            }
            if (!dashing)
            {
                actions -= 1;
            }
           //TurnManager.EndTurn();

        }
    }
    protected void RemoveSelectableTiles() // removes all tiles from the selectable tiles list and resets the tiles
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }
        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
           
        }
        selectableTiles.Clear();
    }
    protected void RemoveDashableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;

            currentTile = null;
        }
        foreach (Tile tile in dashTiles)
        {
            tile.Reset();
            
        }
        dashTiles.Clear();
        
    }
    protected void RemoveAttackTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }
        foreach (Tile tile in attackTiles)
        {
            tile.Reset();
            
        }
        attackTiles.Clear();

    }
    void CalculateHeading(Vector3 target) // makes the movement face towards the target 
    {
        heading = target - transform.position;
        heading.Normalize();
    }
    void SetHorizontalVelocity() // sets how fast the player moves
    {
        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target) // checks what the player is doing on a tile if it needs to jump
    {
        if (fallingDown)
        {
            FallDown(target);
        }
        else if(jumpingUp)
        {
            JumpUp(target);
        }
        else if (movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }
    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;

        target.y = transform.position.y;

        CalculateHeading(target);

        if(transform.position.y > targetY) // if the movement is higher than the tile it is trying to get to
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;
            jumpTarget = transform.position + (target - transform.position) / 2.0f; // makes the target the edge of the current tile
        }
        else // otherwise the player needs to jump up
        {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            velocity = heading * moveSpeed / jumpDampner; // adds a dampner onto the players jump so it doesnt over hit

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0f); // sets the players jump velocity 
        }
    }
    void FallDown(Vector3 target) // makes the player fall down to the bottom tile and stops them moving in the air
    {
        velocity += Physics.gravity * Time.deltaTime;
        if (transform.position.y <= target.y)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = false;

            Vector3 currentPos = transform.position;
            currentPos.y = target.y;
            transform.position = currentPos;

            velocity = new Vector3();
        }
    }
    void JumpUp(Vector3 target) // makes the player jump up to the next tile
    {
        velocity += Physics.gravity * Time.deltaTime;
        if(transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }
    void MoveToEdge()// movement moves to the edge of the target before it starts falling down
    {
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();
        }
        else
        {
            movingEdge = false;
            fallingDown = true;

            velocity /= jumpDampner;
            velocity.y = 1.5f; 
        }
    }



    public Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach( Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest; // finds the lowest cost tile in the list
    }

    public Tile FindEndTile(Tile t) //assumed here is where i will have to do ai attack and dash
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }
        if (dashing)
        {
            if (tempPath.Count <= dashMove)
            {
                //actions -= 1;
                return t.parent;
            }
        }
        else
        {
            if (tempPath.Count <= move)
            {
                //actions -= 1;
                return t.parent;
            }
            else
            {
                //actions = 0;
            }
        }

        Tile endTile = null;
        if (dashing)
        {
            for (int i = 0; i <= dashMove; i++)
            {
                endTile = tempPath.Pop();
            }
            //actions -= 2;
        }
        else
        {
            for (int i = 0; i <= move; i++)
            {
                endTile = tempPath.Pop();
            }
            //actions -= 1;
        }
        //actions -= 1;
        
        return endTile;
    }
    public void FindPath(Tile target) // finds a path to the target tile by visiting each tile and finding the lowest cost to get there
    {
        ComputeAdjacencyLists(jumpHeight, target);

        GetCurrentTile();
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);

        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position); // finds the distance from the processed tile to the destination
        currentTile.f = currentTile.h;

        while(openList.Count > 0)
        {
            Tile t = FindLowestF(openList); // finds the lowest cost tile in the open list and says it has been visited

            closedList.Add(t);

            if(t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile); // moves the movement to the tile
               
                return;
            }

            foreach(Tile tile in t.adjacencyList)
            {
                if (closedList.Contains(tile))
                {
                    // do nothing
                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;
                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.f;

                    openList.Add(tile);
                }
            }
        }
        
        Debug.Log("path not found");

    }

    public void BeginTurn()
    {
        
        turn = true;
    }
    public void EndTurn()
    {
        turn = false;
        actions = initialActions; // resets the actions
    }
}
