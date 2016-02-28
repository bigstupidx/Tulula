using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public enum Order
{
    Scene = 3, Hud = 2, Globals = 1
}

public class Global
{
    static public string kSourcesPath = "F:/RR/work/projects/Tulula/Metadata";
    static public string kAssetsPath = "Assets/Sprites/";

    static public string kInventoryPath = kAssetsPath + "Inventory/";

    public static Vector2 VectorFromString(string obj, params char[] separator)
    {
        string trim = obj.Trim('{', '}');
        string[] split = trim.Split(separator);

        if (split.GetLength(0) < 2)
        {
            return new Vector2();
        }

        try
        {
            int x = Int32.Parse(split[0]);
            int y = Int32.Parse(split[1]);

            return new Vector2(x, y);
        }
        catch (FormatException e)
        {
            Console.WriteLine(e.Message);
        }

        return new Vector2();
    }

    public static Rect RectFromString(string obj)
    {
        string[] split = obj.Split(',');

        if (split.Length < 4)
        {
            return new Rect();
        }

        for (int i = 0; i < split.Length; ++i)
        {
            split[i] = split[i].Trim('{', '}');
        }

        try
        {
            int x = Int32.Parse(split[0]);
            int y = Int32.Parse(split[1]);

            int w = Int32.Parse(split[2]);
            int h = Int32.Parse(split[3]);

            return new Rect(x, y, w, h);
        }
        catch (FormatException e)
        {
            Console.WriteLine(e.Message);
        }

        return new Rect();
    }
}

