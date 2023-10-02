using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeziarCurve
{
    private List<Vector3> _positions = new List<Vector3>();
    private List<Vector3> _rotations = new List<Vector3>();
    private List<Vector3> _scales = new List<Vector3>();

    public void InputTransform(Transform tr)
    {
        InputPosition(tr.position);
        InputRotation(tr.rotation);
        InputScale(tr.localScale);
    }
    public void InputPosition(Vector3 pos)
    {
        _positions.Add(pos);
    }
    public void InputRotation(Quaternion q)
    {
        _rotations.Add(q.eulerAngles);
    }
    public void InputRotation(Vector3 rot)
    {
        _rotations.Add(rot);
    }
    public void InputScale(Vector3 scale)
    {
        _scales.Add(scale);
    }


    //Èì... ¾Ö¸ÅÇÏ³×
    public Vector3 GetBeziarPosition(float time)
    {
        Vector3[] positions = _positions.ToArray();
        while(positions.Length != 1)
        {
            List<Vector3> nexts = new List<Vector3>();
            for (int i = 0 ; i < positions.Length - 1 ; i++)
            {
                nexts.Add(Vector3.Lerp(positions[i], positions[i+1], time));
            }

            positions = nexts.ToArray();
        }

        return positions[0];
    }

    public Quaternion GetBeziarRotation(float time)
    {
        Vector3[] rotations = _rotations.ToArray();
        while (rotations.Length != 1)
        {
            List<Vector3> nexts = new List<Vector3>();
            for (int i = 0 ; i < rotations.Length - 1 ; i++)
            {
                nexts.Add(Vector3.Lerp(rotations[i] , rotations[i + 1] , time));
            }

            rotations = nexts.ToArray();
        }

        return Quaternion.Euler(rotations[0]);
    }

    public Vector3 GetBeziarScale(float time)
    {
        Vector3[] scales = _scales.ToArray();
        while (scales.Length != 1)
        {
            List<Vector3> nexts = new List<Vector3>();
            for (int i = 0 ; i < scales.Length - 1 ; i++)
            {
                nexts.Add(Vector3.Lerp(scales[i] , scales[i + 1] , time));
            }

            scales = nexts.ToArray();
        }

        return scales[0];
    }

}
