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

        float f = targetPosition.x < gameObject.transform.position.x ? 1.0f : -1.0f;

        _spline = go.AddComponent<BezierSpline>();
        _spline.Reset();

        _spline.SetControlPoint(0, targetPosition);
        _spline.SetControlPoint(3, gameObject.transform.position);
        _spline.SetControlPoint(1, targetPosition + Quaternion.Euler(0, 0, 45 * f) * new Vector3(0, 2));
        _spline.SetControlPoint(2, gameObject.transform.position + Quaternion.Euler(0, 0, 0) * new Vector3(0, 2));
  
        _state = InventState.Invent;
    }
}
