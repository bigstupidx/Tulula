using UnityEngine;

using System.Collections.Generic;
using System.Collections;

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

    Dictionary<GameObject, GameObject> _config = new Dictionary<GameObject, GameObject>();

    int _count = 0;

    public void Init(string broken, string full, string objects)
    {
        _broken = broken;
        _full = full;
        _objects = objects;
    }

	public void Start ()
    {
        var objs = GameObject.FindObjectsOfType<GameObject>();

        var trim = _objects.Replace(" ", string.Empty);
        var pairs = trim.Split(',');
        
        foreach(var pair in pairs)
        {
            var cfg = pair.Split(':');

            if(cfg.Length < 2)
            {
                continue;
            }

            var first = Utils.GetObjectByName(objs, cfg[0]);
            var second = Utils.GetObjectByName(objs, cfg[1]);

            if(first && second)
            {
                first.transform.parent = gameObject.transform;
                _config.Add(first, second);
            }
        }

        _brokenObj = Utils.GetObjectByName(objs, _broken);
        _fullObj = Utils.GetObjectByName(objs, _full);

        _count = 0;
        gameObject.transform.localScale = new Vector3(0, 0, 0);
	}

    public void Update ()
    {
	
	}
}