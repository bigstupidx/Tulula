using UnityEngine;

using System.Collections.Generic;
using System.Collections;

enum CompoundState
{
    Open, Close
}

public delegate void CompoundEvent(CompoundController controller);

public class CompoundController : MonoBehaviour
{
    [SerializeField]
    string _objects;
    [SerializeField]
    string _broken;
    [SerializeField]
    string _full;

    GameObject _brokenObj = null;
    GameObject _fullObj = null;
    SceneController _scene = null;

    Dictionary<GameObject, GameObject> _config = new Dictionary<GameObject, GameObject>();

    int _count = 0;
    CompoundState _state;

    public event CompoundEvent onCompoundComplete;

    public void Init(string broken, string full, string objects)
    {
        _broken = broken;
        _full = full;
        _objects = objects;
    }

    public void Start()
    {
        var objs = GameObject.FindObjectsOfType<GameObject>();

        var trim = _objects.Replace(" ", string.Empty);
        var pairs = trim.Split(',');

        foreach (var pair in pairs)
        {
            var cfg = pair.Split(':');

            if (cfg.Length < 2)
            {
                continue;
            }

            var first = Utils.GetObjectByName(objs, cfg[0]);
            var second = Utils.GetObjectByName(objs, cfg[1]);

            if (first && second)
            {
                var controller = first.GetComponent<GameObjectController>();

                if(controller)
                {
                    controller.alpha = 0;
                    controller.onTouchEndedWithObject += OnDropObject;
                }

                first.transform.parent = gameObject.transform;
                _config.Add(first, second);
            }
        }

        _brokenObj = Utils.GetObjectByName(objs, _broken);
        _fullObj = Utils.GetObjectByName(objs, _full);

        if(_brokenObj && _fullObj)
        {
            _count = 0;
            _state = CompoundState.Close;

            gameObject.transform.localScale = new Vector3(0, 0, 0);

            var controller = _brokenObj.GetComponent<GameObjectController>();

            if (controller)
            {
                controller.onTouchEnded += OnOpenObject;
                controller.onTouchMovedWithObject += OnTouchMovedWithObject;
                controller.touchEnabled = true;
            }

            controller = gameObject.GetComponent<GameObjectController>();

            if (controller)
            {
                controller.onTouchEnded += OnCloseObject;
                controller.onTouchEndedWithObject += OnDropObject;
                controller.touchEnabled = true;
            }
        }

        var scene = Utils.GetObjectByName(objs, "Scene");
        
        if(scene)
        {
            _scene = scene.GetComponent<SceneController>();
        }
    }

    void OnTouchMovedWithObject(GameObject obj, GameObject picked)
    {
        foreach(var config in _config)
        {
            if(config.Value == picked)
            {
                Open();
            }
        }
    }

    void OnOpenObject(GameObject obj)
    {
        Open();
    }

    void OnCloseObject(GameObject obj)
    {
        Close();
    }

    void OnDropObject(GameObject obj, GameObject picked)
    {
        foreach(var config in _config)
        {
            if(config.Value == picked)
            {
                var controller = config.Key.GetComponent<GameObjectController>();

                if (controller && controller.IsIntersect(picked))
                {
                    controller.alpha = 1;

                    controller = picked.GetComponent<GameObjectController>();

                    if (controller)
                    {
                        controller.alpha = 0;
                    }

                    ++_count;

                    if (_count >= _config.Count)
                    {
                        Complete();
                    }
                    _scene.DeInvent(picked);
                }
            }
        }
    }

    public void Open()
    {
        if(_state == CompoundState.Close)
        {
            _state = CompoundState.Open;
            iTween.ScaleTo(gameObject, Utils.Hash("scale", new Vector3(1, 1, 0), "time", 0.3f, "easetype", iTween.EaseType.easeOutBack));

            if(_scene)
            {
                _scene.OnOpenCompound(this);
            }
        }
    }

    public void Close()
    {
        if(_state == CompoundState.Open)
        {
            _state = CompoundState.Close;
            iTween.ScaleTo(gameObject, Utils.Hash("scale", new Vector3(0, 0, 0), "time", 0.3f, "easetype", iTween.EaseType.easeInBack));

            if(_scene)
            {
                _scene.OnCloseCompound(this);
            }
        }
    }

    void Complete()
    {
        var controller = _brokenObj.GetComponent<GameObjectController>();

        if(controller)
        {
            controller.touchEnabled = false;
            controller.alpha = 0;
        }

        controller = _fullObj.GetComponent<GameObjectController>();

        if(controller)
        {
            controller.alpha = 1.0f;
        }

        if(onCompoundComplete != null)
        {
            onCompoundComplete(this);
        }

        Close();
    }
}