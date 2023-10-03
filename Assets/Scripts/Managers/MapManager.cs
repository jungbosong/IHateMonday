using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MapManager : MonoBehaviour
{
    public List<Room> roomList = new List<Room>();
    public int[,] roomCounts = new int[,]
    {
        {1, 1, 2, 2, 1, 1},
        {4, 1, 4, 4, 2, 1},
        {3, 1, 6, 3, 2, 1},
    };
    public int currentFloor = 2;
}