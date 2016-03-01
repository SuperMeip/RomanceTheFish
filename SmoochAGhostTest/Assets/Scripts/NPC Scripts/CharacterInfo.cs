using UnityEngine;
using System;
using System.Collections.Generic;

public enum Characters {None, Player, Ghost, Iblis }

public class CharacterInfo {

    public static CharacterInfo instance = new CharacterInfo();

    public Dictionary<Characters,Character> CharacterList;

	private CharacterInfo ()
    {
        CharacterList = new Dictionary<Characters, Character>();
        foreach (string x in Enum.GetNames(typeof(Characters)))
        {
            var chara = (Characters)Enum.Parse(typeof(Characters), x);
            CharacterList.Add(chara, new Character(chara));
        }
    }

    public Character GetByName(Characters CharName)
    {
        return CharacterList[CharName];
    }

    public static CharacterInfo GetInstance
    {
        get
        {
            return instance;
        }
    }
}
