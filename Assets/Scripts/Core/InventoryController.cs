using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class InventoryController : MonoBehaviour
{
    List<GameObject> _objects = new List<GameObject>();

    Rect _bounds = new Rect(0, -320, 510, 70);

    void Start()
    {
        var objs = GameObject.FindObjectsOfType<GameObject>();

        foreach(var obj in objs)
        {
            var controller = obj.GetComponent<GameObjectController>();

            if(controller && controller.hasInventoryPair)
            {
                var invent = Utils.GetObjectByName(objs, controller.invent);

                if(invent)
                {
                    controller.touchEnabled = true;
                }

                controller = invent.GetComponent<GameObjectController>();

                if(controller)
                {
                    controller.alpha = 0;
                }
            }
        }
    }

    public void Update()
    {

    }

    public void Invent(GameObject obj)
    {
        var controller = obj.GetComponent<GameObjectController>();

        if (controller && controller.hasInventoryPair)
        {
            var objs = GameObject.FindObjectsOfType<GameObject>();
            var invent = Utils.GetObjectByName(objs, controller.invent);

            controller.alpha = 0;

            if (invent)
            {
                invent.transform.position = controller.center;
                AddToInventory(invent);
            }

            controller = invent.GetComponent<GameObjectController>();

            if (controller)
            {
                controller.alpha = 1;
            }
        }
        else
        {
            AddToInventory(obj);
        }
    }

    private void AddToInventory(GameObject obj)
    {
        var controller = obj.AddComponent<InventController>();

        var go = new GameObject();
        var collider = go.AddComponent<BoxCollider2D>();
        collider.size = _bounds.size / Config.kPixelsPerUnit;
        go.transform.position = _bounds.position / Config.kPixelsPerUnit;

        controller.Invent(GetFreeSlot());
        _objects.Add(obj);
    }

    Vector3 GetFreeSlot()
    {
        var x = _bounds.width / Config.kMaxInventory * (_objects.Count + 1);
        var y = _bounds.height / 2;

        return new Vector3(x, y) / Config.kPixelsPerUnit;
    }
}

