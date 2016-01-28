using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum Characters { Player, Ghost, BaraDragon}


public class Character {

    public Characters Name;
    public int Favor;
    /// <summary>
    /// HAPPY, EXCITED, SAD, BASHFUL
    /// </summary>
    public Sprite[] sprites;
    public Dictionary<Emotes, Sprite> Sprites;


    public Character(Characters name)
    {
        Name = name;
        Favor = 0;
        Sprites = new Dictionary<Emotes, Sprite>();
        sprites = Resources.LoadAll<Sprite>("Character Sprites/" +Name.ToString() + "Sprites");
        foreach(Sprite c in sprites)
        {
            Sprites.Add((Emotes)Enum.Parse(typeof(Emotes), c.name), c);
            Debug.Log(c.name);
        }

    }


}
