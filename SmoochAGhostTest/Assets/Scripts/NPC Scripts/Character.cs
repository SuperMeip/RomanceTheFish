using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Character {

    public Characters Name;
    public int Favor;
    /// <summary>
    /// HAPPY, EXCITED, SAD, BASHFUL
    /// </summary>
    public Dictionary<Emotes, Sprite> EmoteSprites;
    public Dictionary<Facing, Sprite> FaceSprites;

    public Character(Characters name)
    {
        Name = name;
        Favor = 0;
        Sprite[] tempSprites;
        FaceSprites = new Dictionary<Facing, Sprite>();
        EmoteSprites = new Dictionary<Emotes, Sprite>();
        tempSprites = Resources.LoadAll<Sprite>("Character Sprites/" +Name.ToString() + "Sprites");
        foreach(Sprite c in tempSprites)
        {
            EmoteSprites.Add((Emotes)Enum.Parse(typeof(Emotes), c.name), c);
            Debug.Log(c.name);
        }
        tempSprites = Resources.LoadAll<Sprite>("Overworld Sprites/" + Name.ToString() + "SpriteSheet");
        int i = 0;
        foreach (Sprite c in tempSprites)
        {
            switch(i)
            {
                case 1:
                    FaceSprites.Add(Facing.Down, c);
                    break;
                case 4:
                    FaceSprites.Add(Facing.Left, c);
                    break;
                case 7:
                    FaceSprites.Add(Facing.Right, c);
                    break;
                case 10:
                    FaceSprites.Add(Facing.Up, c);
                    break;
            }
            Debug.Log(c.name);
            i++;
        }
    }


}
