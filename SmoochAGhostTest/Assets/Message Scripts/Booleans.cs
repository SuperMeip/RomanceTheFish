using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Booleans
{
    public static Booleans instance = new Booleans();
    private Booleans()
    {
        Dictionary = new Dictionary<string, bool>();
        Inventory = new Dictionary<string, bool>();

        //BOOLS
        string[] bools = File.ReadAllLines("Assets/Resources/Presets/Bools.txt");
        foreach(string c in bools)
        {
            string[] parts = c.Trim().Split(':');
            Dictionary.Add(parts[0].Trim(), (parts[1].Trim().ToUpper() == "TRUE"? true :false));
        }

        //ITEMS
        string[] items = File.ReadAllLines("Assets/Resources/Presets/Items.txt");
        foreach (string c in items)
        {
            string[] parts = c.Trim().Split(':');
            Inventory.Add(parts[0].Trim(), (parts[1].Trim().ToUpper() == "TRUE" ? true : false));
        }
    }
    public static Booleans GetInstance
    {
        get
        {
            return instance;
        }
    }
    public Dictionary<string, bool> Dictionary;
    public Dictionary<string, bool> Inventory;

}
