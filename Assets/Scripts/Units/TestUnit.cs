using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Examples;
using UnityEngine;

public class TestUnit : MonoBehaviour
{
    public Transform destination;
    
    private TurnBasedAI _turnBasedAi;

    private AIPath _aiPath;

    private Path path;
    private Seeker _seeker;
    // Start is called before the first frame update
    void Start()
    {
        _turnBasedAi = GetComponent<TurnBasedAI>();
        _aiPath = GetComponent<AIPath>();
        _seeker = GetComponent<Seeker>();
        
        

        StartCoroutine(CalculatePath());

    }
    
    IEnumerator CalculatePath()
    {
        yield return path = _seeker.StartPath(transform.position, destination.position);
        _aiPath.Move(path.vectorPath[0]);
        yield return null;
        _aiPath.canMove = false;
    }
}
