using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Oscillate : MonoBehaviour
{
    static List<Hashtable> actions = new List<Hashtable>();

    double          t = 0;
    double          w = 0;

    double          start = 0;
    float           period = 0;

    bool            randomPhase = false;
    bool            usedelta = false;

    string          what;

    Vector3         amplitude;
    Vector3         prevVal;

    public static void Start(GameObject target, Hashtable args)
    {
        args["target"] = target;
        args["id"] = System.Guid.NewGuid().ToString();

        target.AddComponent<Oscillate>();
        actions.Insert(0, args);
    }

    void Start()
    {
        Hashtable args = null;

        foreach (Hashtable item in actions)
        {
            GameObject obj = item["target"] as GameObject;

            if(obj == gameObject)
            {
                args = item;
            }
        }

        if(args != null)
        {
            what = (string)args["prop"];
            amplitude = (Vector3)args["a"];
            randomPhase = (bool)args["rndphase"];
            usedelta = (bool)args["usedelta"];
            period = (float)args["t"];
            
            t = 0;
            w = (float)(2 * Math.PI / period);
            start = 0;

            if (randomPhase)
            {
                start = UnityEngine.Random.value * Math.PI * 2;
            }
        }
    }

    void Update()
    {
        t += Time.deltaTime;

        float sin = (float)Math.Sin(w * t + start);
        Vector3 val = amplitude;

        val.Scale(new Vector3(sin, sin, sin));

        Vector3 prop = getProperty();

        if (usedelta)
        {
            UpdateProprty(prop + val - prevVal);
            prevVal = val;
        }
        else
        {
            UpdateProprty(val);
        }
    }

    void UpdateProprty(Vector3 val)
    {
        if(what == "scale")
        {
            gameObject.transform.localScale = val;
        }

        if(what == "position")
        {
            gameObject.transform.localPosition = val;
        }

        if(what == "rotation")
        {
            gameObject.transform.localRotation = Quaternion.Euler(val);
        }
    }

    Vector3 getProperty()
    {
        if(what == "scale")
        {
            return gameObject.transform.localScale;
        }

        if(what == "position")
        {
            return gameObject.transform.localPosition;
        }

        if(what == "rotation")
        {
            return gameObject.transform.localRotation.eulerAngles;
        }

        return new Vector3();
    }
}

