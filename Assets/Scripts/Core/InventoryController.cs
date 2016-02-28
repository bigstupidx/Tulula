using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Actions;

delegate void InventoryHandler(GameObject obj);

class InventoryController : MonoBehaviour
{
    List<GameObject> _objects = new List<GameObject>();

    static Rect kBounds = new Rect(-322, -364, 510, 70);
    static float kSpeed = 10.0f;

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

                    controller = invent.GetComponent<GameObjectController>();

                    if (controller)
                    {
                        controller.alpha = 0;
                    }
                }
            }
        }
    }

    public void Update()
    {
        for(int i = 0; i < _objects.Count; ++i)
        {
            var obj = _objects[i];
            var controller = obj.GetComponent<InventStateController>();

            if(controller)
            {
                if(controller.state == InventState.Inventory)
                {
                    var slot = GetSlotAt(i);
                    var delta = slot - obj.transform.position;

                    obj.transform.position += (delta * 0.3f);
                }
            }
        }
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
                controller.touchEnabled = true;
                controller.alpha = 1;
            }
        }
        else
        {
            AddToInventory(obj);
        }
    }

    public void DeInvent(GameObject obj)
    {
        _objects.Remove(obj);
    }

    public void Drop(GameObject obj)
    {
        for(int i = 0; i < _objects.Count; ++i )
        {
            if(_objects[i] == obj)
            {
                var slot = GetSlotAt(i);
                var position = obj.transform.position;

                var controller = obj.GetComponent<InventStateController>();

                position.z = -1;

                float f = slot.x < obj.transform.position.x ? 1.0f : -1.0f;

                if (Vector3.Distance(position, slot) > 1.0f)
                {
                    controller.state = InventState.Drop;

                    Vector3[] points = { position, slot,
                                        position + Quaternion.Euler(0, 0, 0) * new Vector3(0, 1, -1),
                                        slot + Quaternion.Euler(0, 0, 45 * f) * new Vector3(0, 1, -1)};

                    RunSplineAction(points, obj);
                }
                else
                {
                    controller.state = InventState.Inventory;
                }
            }
        }
    }

    void AddToInventory(GameObject obj)
    {
        var slot = GetFreeSlot();

        float f = slot.x < obj.transform.position.x ? 1.0f : -1.0f;

        var position = obj.transform.position;

        var controller = obj.AddComponent<InventStateController>();
        controller.state = InventState.Invent;

        position.z = -1;

        Vector3[] points = { position, slot,
                            position + Quaternion.Euler(0, 0, 0) * new Vector3(0, 2, -1),
                            slot + Quaternion.Euler(0, 0, 45 * f) * new Vector3(0, 2, -1)};

        _objects.Add(obj);
        RunSplineAction(points, obj);
    }

    Vector3 GetSlotAt(int i)
    {
        var x = kBounds.width / Config.kMaxInventory * (i + 0.5f);
        var y = kBounds.height / 2;

        return new Vector3(x + kBounds.x, y + kBounds.y, -1.0f) / Config.kPixelsPerUnit;
    }

    Vector3 GetFreeSlot()
    {
        return GetSlotAt(_objects.Count);
    }

    void OnInventDone(SplineAction action)
    {
        var obj = action.gameObject;

        var controller = obj.GetComponent<InventStateController>();
        if(controller)
        {
            controller.state = InventState.Inventory;
        }

        Destroy(action.spline);
    }

    void RunSplineAction(Vector3[] points, GameObject obj)
    {
        int[] indices = { 0, 3, 1, 2 };

        var spline = gameObject.AddComponent<BezierSpline>();

        spline.Reset();

        for (int i = 0; i < indices.Length; ++i)
        {
            var idx = indices[i];

            spline.SetControlPoint(idx, points[i]);
        }

        float d = Vector3.Distance(points[0], points[1]) / kSpeed;

        var action = SplineAction.Create(obj, spline, d, false);
        action.onDestroy += OnInventDone;
    }
}