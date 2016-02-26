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
        var go = new GameObject();

        double a = 125 * Math.PI / 180.0;
        double lenght = 2.0;

        Vector3 point = new Vector3((float)(lenght * Math.Sin(a)), (float)(lenght * Math.Cos(a)), 0);

        _spline = go.AddComponent<BezierSpline>();
        _spline.Reset();

        _state = InventState.Invent;

        _spline.SetControlPoint(0, targetPosition);
        _spline.SetControlPoint(1, targetPosition - point);
        _spline.SetControlPoint(3, gameObject.transform.position);

        Debug.Log(targetPosition);
    }
}
