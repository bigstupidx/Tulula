using UnityEngine;

using System;
using System.Collections.Generic;
using System.Collections;

public class SceneController : MonoBehaviour {

    GameObject _picked = null;
    GameObject _touched = null;

    InventoryController _inventory = null;
    CompoundController _compound = null;

	void Start ()
    {
        var tc = gameObject.AddComponent<TouchController>();

        tc.onTouchBegan += OnTouchBegan;
        tc.onTouchMoved += OnTouchMoved;
        tc.onTouchEnded += OnTouchEnded;

        _inventory = gameObject.AddComponent<InventoryController>();
	}
	
	void Update ()
    {
	
	}

    void OnTouchBegan(TouchEvent touch)
    {
        _touched = GetObjectAtTouch(touch);

        if(_touched)
        {
            var controller = _touched.GetComponent<GameObjectController>();
            if(controller)
            {
                controller.OnTouchBegan();

                if (controller.hasInventoryPair)
                {
                    Invent(_touched);
                }

                var invent = _touched.GetComponent<InventStateController>();

                if(invent != null)
                {
                    if(invent.state == InventState.Inventory)
                    {
                        invent.state = InventState.Picked;
                        _picked = _touched;
                    }
                }
            }
        }
    }

    void OnTouchMoved(TouchEvent touch)
    {
        var location = Camera.main.ScreenToWorldPoint(touch.location);
        
        if (_picked)
        {
            _picked.transform.position = new Vector3(location.x, location.y, -1);

            var obj = GetObjectAtTouch(touch);

            if(obj)
            {
                var controller = obj.GetComponent<GameObjectController>();
                if (controller)
                {
                    controller.OnTouchMovedWithObject(_picked);
                }
            }
        }
    }

    void OnTouchEnded(TouchEvent touch)
    {
        if(_picked)
        {
            _inventory.Drop(_picked);

            var obj = GetObjectAtTouch(touch);

            if(obj)
            {
                var controller = obj.GetComponent<GameObjectController>();
                if (controller)
                {
                    controller.OnTouchEndedWithObject(_picked);
                }
            }

            _picked = null;
        }
        else if(_touched)
        {
            var obj = GetObjectAtTouch(touch);

            if (obj == _touched)
            {
                var controller = obj.GetComponent<GameObjectController>();
                if (controller)
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

        var objects = Array.ConvertAll(colliders, item => item.gameObject);

        Array.Sort<GameObject>(objects, (one, two) => one.transform.position.z.CompareTo(two.transform.position.z));

        foreach (var obj in objects)
        {
            if(obj == _picked)
            {
                continue;
            }

            var controller = obj.GetComponent<GameObjectController>();

            if (controller)
            {
                 if (controller.aviableForTouch)
                {
                    return obj;
                }
            }
        }

        return null;
    }

    public void Invent(GameObject obj)
    {
        _inventory.Invent(obj);
    }

    public void DeInvent(GameObject obj)
    {
        _inventory.DeInvent(obj);
    }

    public void OnOpenCompound(CompoundController compound)
    {
        var position = compound.transform.position;

        if (_compound)
        {
            _compound.Close();
        }
        
        position.z = 0;

        _compound = compound;
        _compound.transform.position = position;
    }

    public void OnCloseCompound(CompoundController compound)
    {
        _compound = null;
    }
}