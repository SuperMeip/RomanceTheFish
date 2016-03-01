using System;
using System.Collections.Generic;

public enum Emotes { HAPPY, EXCITED, SAD, BASHFUL, ANGRY, NEUTRAL }
public enum ItemMethod {TAKE, GIVE }


public struct BoolChange
{
    public bool Value;
    public string BoolName;
    public BoolChange(bool value, string boolName)
    {
        Value = value;
        BoolName = boolName;
    }
    public BoolChange(string RawBool)
    {
        string[] parts = RawBool.TrimStart('*').Split('=');
        Value = (parts[1].Trim() == "TRUE");
        BoolName = parts[0].Trim();
    }
}

public struct FavorCheck
{
    public Characters CharName;
    public int Value;
    public int AboveID;
    public int BelowID;
    public FavorCheck(string RawFavor)
    {
        string[] parts = RawFavor.Trim().TrimStart('#').TrimStart('=').Split(':');
        CharName = (Characters)Enum.Parse(typeof(Characters), parts[0].Trim());
        Value = Convert.ToInt32(parts[1].Trim());
        AboveID = Convert.ToInt32(parts[2].Trim());
        BelowID = Convert.ToInt32(parts[3].Trim()); 
    }
}

public struct CharachterFavorChange
{
    public int Value;
    public Characters CharName;
    public CharachterFavorChange(int value, Characters character)
    {
        Value = value;
        CharName = character;
    }
    public CharachterFavorChange(string RawFavorChange)
    {
        CharName = (Characters)Enum.Parse(typeof(Characters),RawFavorChange.Substring(1).Trim());
        Value = RawFavorChange[0] == '+' ? 1 : -1;
    }
}

public struct Telegram
{
    public string Text;
    public Emotes Emotion;

    public Telegram(string text, Emotes emotion) { Text = text; Emotion = emotion; }
    //Line Format <EMOTION>% MESSAGE
    public Telegram(string RawLine)
    {
        string[] parts = RawLine.Split('>');
        Emotion = (Emotes)Enum.Parse(typeof(Emotes), (parts[0].Trim().TrimStart('<')));
        Text = parts[1].Trim();
    }
    public Telegram(ItemTransfer item, Emotes emotion)
    {
        Emotion = emotion;
        Text = item.ToString();
    }

    public string ToString()
    {
        return "\n[" + Emotion.ToString() + "]\n" + Text;
    }
}

public struct Choice
{
    public struct Choices
    {
        public int Number;
        public int JumpID;
        public string Text;
        public Choices(int number, int jumpID, string text)
        {
            Number = number;
            JumpID = jumpID;
            Text = text;
        }
    }
    public Choices[] ChoiceList;
    int NumberOfChoices;
    //Choice Syntax
    //#CHOICE %ID:CHOICE %ID:CHOICE %ID
    //Up to 4 choices
    public Choice(string RawChoice)
    {
        string choiceString = RawChoice.TrimStart('#').TrimStart('?');
        string[] parts = choiceString.Split(':');
        ChoiceList = new Choices[parts.Length];
        NumberOfChoices = 0;
        for(int index = 0; index < ChoiceList.Length && index < 4; index++)
        {
            string[] choiceSplit = parts[index].Split('%');
            ChoiceList[index] = new Choices(index,Convert.ToInt32(choiceSplit[1].Trim()),choiceSplit[0].Trim());
            NumberOfChoices++;
        }
    }
    public string ToString()
    {
        string final = "\nYOURE GIVEN A CHOICE:\n[";
        foreach(Choices c in ChoiceList)
        {
            final += c.Number + ") " + c.Text + " (ID:" + c.JumpID + ") | ";
        }
        final += ']';
        return final;
    }
    public string ListChoices()
    {
        string final = "Choose:\n";
        foreach (Choices c in ChoiceList)
        {
            final += c.Number+1 + ") " + c.Text + '\n';
        }
        return final;
    }
    public int ChoiceCount()
    {
        return ChoiceList.Length;
    }
}

public struct BooleanJump
{
    public int TrueID;
    public int FalseID;
    string Bool;
    public BooleanJump(int True, int False, string BoolName)
    {
        TrueID = True;
        FalseID = False;
        Bool = BoolName;
    }
    public BooleanJump(string RawBool)
    {
        string boolString = RawBool.TrimStart('#').TrimStart('!');
        string[] parts = boolString.Split(':');
        Bool = parts[0];
        TrueID = Convert.ToInt32(parts[1]);
        FalseID = Convert.ToInt32(parts[2]);
    }
    public bool Test()
    {
        if (Booleans.instance.Dictionary.ContainsKey(Bool))
            return Booleans.instance.Dictionary[Bool];
        else if (Booleans.instance.Inventory.ContainsKey(Bool))
            return Booleans.instance.Inventory[Bool];
        else
            return false;
    }
    public string ToString()
    {
        return "\nIF " + Bool + " is TRUE go to " + TrueID + ". If FALSE go to " + FalseID + ".]";
    }

}

public struct ItemTransfer
{
    public ItemMethod Method;
    public int LinePreceeding;
    public string Item;
    //Item Format [METHOD: ITEM]
    public ItemTransfer(string RawItem, int line)
    {
        string[] parts = RawItem.Split(':');
        Method = (ItemMethod)Enum.Parse(typeof(ItemMethod), (parts[0].TrimStart('[')));
        Item = parts[1].Trim().TrimEnd(']').Trim();
        //Will be item grabbing method eventually
        LinePreceeding = line-1;
    }
    public string ToString()
    {
        return "You " + (Method == ItemMethod.GIVE ? "gave them " : "received ") + "the " + Item;
    }
}

public class Message {
    public int ID;
    public int Length;
    public Characters Character;
    public bool EndsInChoice;
    public bool JumpsAtEnd;
    public bool BoolJump;
    public bool FavorJump;
    public bool JustEnds;
    public int JumpID;
    public Choice EndChoice;
    public BooleanJump JumpBool;
    public FavorCheck JumpFavor;
    public List<Telegram> Lines;
    public List<ItemTransfer> ItemTransfers;
    public List<CharachterFavorChange> FavorChanges;
    public List<BoolChange> BoolChanges;
    //First line structutre
    //CHARACTER NAME: MESSAGE ID
    //End line structure:
    //## is end without jump or choice
    //#<ID> jumps to message of that ID
    //and a choice structure ends it in a choice
    public Message(string rawDialog)
    {
        Lines = new List<Telegram>();
        ItemTransfers = new List<ItemTransfer>();
        FavorChanges = new List<CharachterFavorChange>();
        BoolChanges = new List<BoolChange>();
        int index = 0;
        int line = 0;
        JumpID = 0;
        EndsInChoice = false;
        JustEnds = false;
        JumpsAtEnd = false;
        BoolJump = false;
        string[] allLines = rawDialog.Split('\n');
        foreach (string c in allLines)
        {
            if (c != null && c != "")
            {
                //if first line: Get ID and CHARACTER
                if (index == 0)
                {
                    Character = (Characters)Enum.Parse(typeof(Characters), c.Split(':')[0].Trim());
                    ID = Convert.ToInt32(c.Split(':')[1].Trim());
                    index++;
                }
                //if Item line: Get the item transfer info
                else if (c[0] == '[')
                {
                    ItemTransfer item = new ItemTransfer(c, line);
                    ItemTransfers.Add(item);
                    Lines.Add(new Telegram(item, Lines[Lines.Count-1].Emotion));
                    index++;
                }
                else if (c[0] == '/' && c[1] == '/' )
                {
                    //Comment
                }
                //if dialog line add the dialog to the list
                else if (c[0] == '<')
                {
                    Lines.Add(new Telegram(c));
                    line++;
                    index++;
                }
                //For favors
                // add, subtract
                else if(c[0] == '+' || c[0] == '-')
                {
                    FavorChanges.Add(new CharachterFavorChange(c));
                }
                //For Bools
                else if(c[0] == '*')
                {
                    BoolChanges.Add(new BoolChange(c));
                }
                //if end line check end paramaters
                else if (c[0] == '#')
                {
                    if (c[1] == '<')
                    {
                        JumpsAtEnd = true;
                        JumpID = Convert.ToInt32(c.Trim().TrimStart('#').TrimStart('<').TrimEnd('>'));
                    }
                    else if (c[1] == '#')
                    {
                        JustEnds = true;
                    }
                    else if (c[1] == '?')
                    {
                        EndsInChoice = true;
                        EndChoice = new Choice(c);
                    }
                    else if (c[1] == '!')
                    {
                        BoolJump = true;
                        JumpBool = new BooleanJump(c);
                    }
                    else if(c[1] == '=')
                    {
                        FavorJump = true;
                        JumpFavor = new FavorCheck(c);
                    }
                }
            }
        }
        Length = line;
    }

    public string ToString()
    {
        string message = "MESSAGE FROM: " + Character.ToString() + ", ID: " + ID;
        int currentItemLine = 0;
        int itemsCount = 0;
        int currentItem = 0;
        if (ItemTransfers.Count > 0 )
        {
            currentItemLine = ItemTransfers[0].LinePreceeding;
            itemsCount = ItemTransfers.Count;
        }
        for (int i = 0; i < Lines.Count; i++)
        {
            message += Lines[i].ToString();
            if (i == currentItemLine && itemsCount != 0)
            {
                message += ItemTransfers[currentItem].ToString();
                itemsCount--;
                if(itemsCount!=0)
                {
                    currentItem++;
                    currentItemLine = ItemTransfers[currentItem].LinePreceeding;
                }
            }
        }
        if(JumpsAtEnd)
        {
            message += "\nJUMP TO MESSAGE: " + JumpID;
        }
        else if(JustEnds)
        {
            message += "\nEND OF MESSAGE CHAIN."; 
        }
        else if(EndsInChoice)
        {
            message += EndChoice.ToString();
        }
        else if(BoolJump)
        {
            message += JumpBool.ToString();
        }
        else if(FavorJump)
        {

        }
        message += "\n===============================\n";
        foreach(BoolChange c in BoolChanges)
        {
            message += "\nSet " + c.BoolName + " To " + c.Value;
        }
        foreach(CharachterFavorChange c in FavorChanges)
        {
            message += "\n" + c.CharName.ToString() + "'s Favor" + (c.Value > 0 ? " Increased!" : " Deacreased!");
        }
        message += "\n===============================\n";
        return message;
    }
    public int GetLines()
    {
        return Lines.Count;
    }
}
