using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 -> Need bottom door
    // 2 -> Need top door
    // 3 -> Need left door
    // 4 -> Need right door

    private RoomTemplates _templates;
    public bool _spawned = false;

    private void Start()
    {
        _templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn",0.5f);
    }

    private void Spawn()
    {
        if(_spawned) return;
        
        switch (openingDirection)
        {
            case 1:
                int rand = Random.Range(0, _templates.bottomRooms.Length);
                var room = Instantiate(_templates.bottomRooms[rand],transform.position,Quaternion.identity);
                room.transform.parent = gameObject.transform.parent.transform.parent;
                break;
            case 2:
                rand = Random.Range(0, _templates.topRooms.Length);
                room = Instantiate(_templates.topRooms[rand],transform.position,Quaternion.identity);
                room.transform.parent = gameObject.transform.parent.transform.parent;
                break;
            case 3:
                rand = Random.Range(0, _templates.leftRooms.Length);
                room = Instantiate(_templates.leftRooms[rand],transform.position,Quaternion.identity);
                room.transform.parent = gameObject.transform.parent.transform.parent;
                break;
            case 4:
                rand = Random.Range(0, _templates.rightRooms.Length);
                room =Instantiate(_templates.rightRooms[rand],transform.position,Quaternion.identity);
                room.transform.parent = gameObject.transform.parent.transform.parent;
                break;
        }

        _spawned = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>()._spawned == false && _spawned == false)
            {
                Instantiate(_templates.closedRooms, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            _spawned = true;
        }
    }
}
