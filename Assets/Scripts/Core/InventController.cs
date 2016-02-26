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

    static float kSpeed = 1.0f;

    float _time = 0;
    float _duration = 0;

    public InventState state
    {
        get { return _state; }
    }

    public void Update()
    {
        if(_spline)
        {
            _time += Time.deltaTime;

            Debug.Log(_time);
            transform.localPosition = _spline.GetPoint(_time / 5.0f);

            if(_time >= 5.0f)
            {
                _spline = null;
            }
        }
    }

    public void Invent(Vector3 targetPosition)
    {
        var go = new GameObject();

        float f = targetPosition.x < transform.position.x ? 1.0f : -1.0f;

        _spline = gameObject.AddComponent<BezierSpline>();
        _spline.Reset();

        _spline.SetControlPoint(0, targetPosition);
        _spline.SetControlPoint(3, transform.position);
        _spline.SetControlPoint(1, targetPosition + Quaternion.Euler(0, 0, 45 * f) * new Vector3(0, 2));
        _spline.SetControlPoint(2, transform.position + Quaternion.Euler(0, 0, 0) * new Vector3(0, 2));
          
        _state = InventState.Invent;

        _time = 0;
        _duration = Vector3.Distance(targetPosition, transform.position) / kSpeed;
    }
}
