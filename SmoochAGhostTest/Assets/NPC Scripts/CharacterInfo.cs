using UnityEngine;
using System.Collections.Generic;

public class CharacterInfo {

    public static CharacterInfo instance = new CharacterInfo();

    public Dictionary<Characters,Character> CharacterList;

	private CharacterInfo ()
    {
        CharacterList = new Dictionary<Characters, Character>();
        CharacterList.Add(Characters.Ghost,new Character(Characters.Ghost));
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
