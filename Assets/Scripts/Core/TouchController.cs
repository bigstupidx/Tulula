using UnityEngine;

using System.Collections.Generic;
using System.Collections;
using System;

public class TouchEvent
{
    Vector2 _startLocation;
    Vector2 _prevLocation;
    Vector2 _currentLocation;

    int _id;

    public TouchEvent(Vector2 location, int id)
    {
        _startLocation = _prevLocation = _currentLocation = location;
        _id = id;
    }

    public Vector2 location
    {
        get { return _currentLocation; }
        set { _currentLocation = value; }
    }

    public Vector2 prevLocation
    {
        get { return _prevLocation; }
        set { _prevLocation = value; }
    }

    public Vector2 startLocation
    {
        get { return _startLocation; }
        set { _startLocation = value; }
    }

    public Vector2 delta
    {
        get { return _prevLocation - _currentLocation; }
    }

    public int id
    {
        get { return _id; }
    }
}

public delegate void TouchHandler(TouchEvent e);

public class TouchController : MonoBehaviour {

    public event TouchHandler onTouchBegan;
    public event TouchHandler onTouchMoved;
    public event TouchHandler onTouchEnded;

    public static int kMinTouchLenght = 5;

    List<TouchEvent> _touches = new List<TouchEvent>();

    void Start ()
    {
	
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TouchEvent touch = new TouchEvent(GetMousePoint(), 0);

            if (onTouchBegan != null)
            {
                onTouchBegan(touch);
            }

            _touches.Add(touch);
        }

        if (Input.GetMouseButton(0))
        {
            foreach (var touch in _touches)
            {
                var prev = touch.location;
                var location = GetMousePoint();
                var delta = location - prev;

                if(delta.magnitude > kMinTouchLenght)
                {
                    touch.prevLocation = prev;
                    touch.location = location;

                    if (onTouchMoved != null)
                    {
                        onTouchMoved(touch);
                    }
                }   
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            foreach (var touch in _touches)
            {
                if (onTouchEnded != null)
                {
                    onTouchEnded(touch);
                }
            }

            _touches.Clear();
        }
    }

    private Vector2 GetMousePoint()
    {
        return Input.mousePosition;
    }
}