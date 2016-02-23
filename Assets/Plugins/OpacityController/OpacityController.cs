using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class OpacityController : MonoBehaviour
{
    private float       _alpha = 1.0f;
        
    public void Start()
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();

        if(renderer != null)
        {
            _alpha = renderer.color.a;
        }
    }

    public void Update()
    {
        var renderer = gameObject.GetComponent<SpriteRenderer>();

        if(renderer != null)
        {
            float alpha = _alpha;
            var parent = gameObject.transform.parent;

            if(parent != null)
            {
                var parentRenderer = parent.GetComponent<SpriteRenderer>();

                if(parentRenderer != null)
                {
                    alpha *= parentRenderer.color.a;
                }
            }

            Color color = renderer.color;
            color.a = alpha;

            renderer.color = color;
        }
    }

    public float alpha
    {
        get{
            return _alpha;
        }

        set{
            _alpha = value;
            Update();
        }
    }
}

