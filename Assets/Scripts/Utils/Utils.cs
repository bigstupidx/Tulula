﻿using System;
using System.Collections;
using UnityEngine;

public class Utils
{
    public static Hashtable Hash(params object[] args)
    {
        Hashtable hashTable = new Hashtable(args.Length / 2);
        if (args.Length % 2 != 0)
        {
            return null;
        }
        else {
            int i = 0;
            while (i < args.Length - 1)
            {
                hashTable.Add(args[i], args[i + 1]);
                i += 2;
            }
            return hashTable;
        }
    }

    public static double RandRange(double min, double max)
    {
        System.Random r = new System.Random();

        return r.NextDouble() * (max - min) + min;
    }

    public static GameObject GetObjectByName(GameObject[] objs, string name)
    {
        foreach (GameObject obj in objs)
        {
            if (obj.name == name)
            {
                return obj;
            }
        }
        return null;
    }
}
