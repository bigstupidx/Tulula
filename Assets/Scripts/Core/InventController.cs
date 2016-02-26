using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public enum InventState
{
    Invent, Drop, Picked
}

class InventController : MonoBehaviour
{
    InventState _state;
    BezierSpline _spline;

    public InventState state
    {
        get { return _state; }
    }

    public void Update()
    {

    }

    public void Invent(Vector3 targetPosition)
    {
        var gameObject = new GameObject();

        _spline = gameObject.AddComponent<BezierSpline>();
        _spline.Reset();

        _state = InventState.Invent;

        _spline.SetControlPoint(1, targetPosition);
        _spline.SetControlPoint(3, gameObject.transform.position);
    }
}
