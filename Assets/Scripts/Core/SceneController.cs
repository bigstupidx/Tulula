using UnityEngine;

using System.Collections.Generic;
using System.Collections;

public class SceneController : MonoBehaviour {

    GameObject _picked = null;
    GameObject _touched = null;

    List<GameObject> _inventory = new List<GameObject>();

	void Start ()
    {
        var tc = gameObject.AddComponent<TouchController>();

        tc.onTouchBegan += OnTouchBegan;
        tc.onTouchMoved += OnTouchMoved;
        tc.onTouchEnded += OnTouchEnded;
	}
	
	void Update ()
    {
	
	}

    void OnTouchBegan(TouchEvent touch)
    {
        _touched = GetObjectAtTouch(touch);

        if(_touched != null)
        {
            var controller = _touched.GetComponent<GameObjectController>();
            if(controller != null)
            {
                var invent = _touched.GetComponent<InventController>();

                controller.OnTouchBegan();

                if(invent)
                {
                    invent.Invent();
                }
            }
        }
    }

    void OnTouchMoved(TouchEvent touch)
    {

    }

    void OnTouchEnded(TouchEvent touch)
    {
        if (_touched != null)
        {
            var obj = GetObjectAtTouch(touch);

            if(obj == _touched)
            {
                var controller = _touched.GetComponent<GameObjectController>();
                if (controller != null)
                {
                    controller.OnTouchEnded();
                }
            }

            _touched = null;
        }
    }

    GameObject GetObjectAtTouch(TouchEvent touch)
    {
        var location = Camera.main.ScreenToWorldPoint(touch.location);
        var colliders = Physics2D.OverlapPointAll(new Vector2(location.x, location.y));

        foreach (var collider in colliders)
        {
            var obj = collider.transform.gameObject;
            var controller = obj.GetComponent<GameObjectController>();

            if (controller)
            {
                if (controller.isAviableForTouch)
                {
                    return obj;
                }
            }
        }

        return null;
    }
}