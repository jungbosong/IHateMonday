using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using static Define;

public class Utils
{
    public static GameObject CreateDoor(Vector3 position, bool isLocked)
    {
        float nearDistance = float.MaxValue;
        Room nearRoom = null;
        foreach (Room room in Managers.Map.roomList)
        {
            float near = Mathf.Abs(room.center.x - position.x) - room.width / 2f;
            near = Mathf.Min(near, Mathf.Abs(room.center.y - position.y) - room.height / 2f);

            if (near < nearDistance)
            {
                nearDistance = near;
                nearRoom = room;
            }
        }

        if (nearRoom is null)
        {
            return null;
        }

        GameObject go;
        if (nearDistance == Mathf.Abs(nearRoom.center.x - position.x) - nearRoom.width / 2f)
        {
            go =  Managers.Resource.Instantiate("Env/HorizonDoor" , position);
        }
        else
        {
            go = Managers.Resource.Instantiate("Env/VerticalDoor" , position);
        }

        Door door = go.GetComponent<Door>();
        door.SetNearRoom(nearRoom);
        nearRoom.OnBattleStart += door.BattleStart;
        nearRoom.OnBattleEnd += door.BattleEnd;
        return go;
    }
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            Transform transform = go.transform.Find(name);
            if (transform != null)
                return transform.GetComponent<T>();
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform != null)
            return transform.gameObject;
        return null;
    }

    public static GameObject FindNearestObject(string tag, Vector3 pos)
    {
        GameObject nearestGo = null;
        float distance = float.MaxValue;
        float minDistance = 0.001f;
        GameObject[] Gos = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject Go in Gos)
        {
            float distance2 = Mathf.Abs((Go.transform.position - pos).magnitude);
            if (Go != null && distance2 < distance && minDistance < distance2)
            {
                nearestGo = Go;
                distance = distance2;
            }
        }

        return nearestGo;
    }
}