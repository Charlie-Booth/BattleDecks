using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour //UNUSED SCRIPT, CAUSED ERRORS WHEN LOADING INTO A NEW SCENE, MAY BE USED IN THE FUTURE WHEN EXPANDING THE GAME
{
    //followed a tutorial series EP 1 https://www.youtube.com/watch?v=cX_KrK8RQ2o: by Game Programming academy, uploaded Nov 9 2017
    public static Dictionary<string, List<Movement>> units = new  Dictionary<string, List<Movement>>();
    public static Queue<string> turnKey = new Queue<string>();
    public static Queue<Movement> turnTeam = new Queue<Movement>();

    List<Movement> list;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
    }

    public static void InitTeamTurnQueue()
    {
        List<Movement> teamList = units[turnKey.Peek()];

        foreach(Movement unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }
        StartTurn();
    }
    public static void StartTurn()
    {
        if(turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
            //turnTeam.Peek().actions = turnTeam.Peek().initialActions;
        }
        else
        {
            //endgameHere
        }
    }
    public static void EndTurn()
    {
        Movement unit = turnTeam.Dequeue();
        unit.EndTurn();
        if (turnTeam.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(Movement unit)
    {
        List<Movement> list;
        if (!units.ContainsKey(unit.tag))
        {
            list = new List<Movement>();
            units[unit.tag] = list;

            if (!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }
    public static void RemoveUnit(Movement unit)
    {
        List<Movement> list;

        if (units.ContainsKey(unit.tag))
        {            
            list = units[unit.tag];
            list.Remove(unit);
        }
        

        //TO FINISH, REMOVE THE UNIT FROM ALL THE LISTS
    }
    public static void ChangeTurn(Movement unit)
    {
        List<Movement> list;
        List<Movement> newList;
        if (units.ContainsKey(unit.tag))
        {
            newList = new List<Movement>();
            list = units[unit.tag];
            list.Remove(unit);
            newList.Add(unit);
            foreach (Movement item in units[unit.tag])
            {
                newList.Add(item);
            }
            units[unit.tag] = newList;
        }
    }
}
