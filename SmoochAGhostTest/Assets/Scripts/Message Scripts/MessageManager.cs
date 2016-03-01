using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.UI;



public class MessageManager : MonoBehaviour {
    public Image CharacterSprite;
    public AudioSource soundSource;
    public AudioClip Typing;
    public GameObject NextArrow;
    //public float lettersPerSecond = 10;
    public GameObject TextCanvas;
    public Text testText, DebugText;
    public float TextSpeed = 0.03f;
    public static Message[] Dialogs;
    public static int messageCount;
    public int currentMessage;
    public int currentDialog;
    public int nextIsItem;
    public string currentText;
    public  bool DialogOpen;
    public  bool ChoiceMode;
    public int startDialog = 001;
    int testerDevise;

    void Awake()
    {
        testerDevise = CharacterInfo.instance.CharacterList.Count;
    }
    // Use this for initialization
    void Start () {
        soundSource = GetComponent<AudioSource>();
        ChoiceMode = false;
        TextCanvas.SetActive(false);
        RetreiveDialogs();
	}

    // Update is called once per frame
    void Update()
    {
        if (DialogOpen)
        {
            DebugText.text = currentDialog + " : " + currentMessage;
            if (!ChoiceMode)
            {
                currentText = PlayLine(Dialogs[currentDialog]);
                if (Input.GetKeyDown(KeyCode.Space) && DialogOpen)
                    upMessage();
            }
            else
            {
                currentText = Dialogs[currentDialog].EndChoice.ListChoices();
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Choose(0);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Choose(1);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) && Dialogs[currentDialog].EndChoice.ChoiceCount() > 2)
                {
                    Choose(2);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) && Dialogs[currentDialog].EndChoice.ChoiceCount() > 3)
                {
                    Choose(3);
                }
            }
        }
    }

    public void Choose(int choice)
    {
        NextArrow.SetActive(true);
        currentMessage = 0;
        currentDialog = Dialogs[currentDialog].EndChoice.ChoiceList[choice].JumpID;
        ChoiceMode = false;
        StopAllCoroutines();
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        soundSource.clip = Typing;
        soundSource.Play();
        for (int i = 0; i < (currentText.Length + 1); i++)
        {
            testText.text = currentText.Substring(0, i);
            yield return new WaitForSeconds(TextSpeed);
        }
        soundSource.Stop();
    }

    public void upMessage()
    {
        if (currentMessage < Dialogs[currentDialog].GetLines())
            currentMessage++;
        else
            currentMessage = 0;
        StopAllCoroutines();
        StartCoroutine(AnimateText());
    }
    public void RetreiveDialogs()
    {
        string allText = File.ReadAllText("Assets/Resources/Dialogs/test.txt");
        string[] rawMessages = allText.Split('@');
        Dialogs = new Message[10000];
        Message current;
        foreach (string message in rawMessages)
        {
            if (message != "")
            {
                current = new Message(message);
                Dialogs[current.ID] = current;
            }
        }
        messageCount = rawMessages.Length - 1;
        for (int i = 0; i < messageCount; i++)
        {
            Debug.Log(Dialogs[i].ToString());
        }
    }

    public string PlayLine(Message dialog)
    {
        if(dialog==null)
        {
            return "NULL DIALOG: " + currentDialog;
        }
        //Message Lines
        if(currentMessage<dialog.GetLines())
        {
            CharacterSprite.sprite = CharacterInfo.instance.CharacterList[dialog.Character].EmoteSprites[dialog.Lines[currentMessage].Emotion];
            return dialog.Lines[currentMessage].Text;
        }
        //End lines
        else if(currentMessage == dialog.GetLines())
        {
            if (dialog.JumpsAtEnd)
            {
                currentMessage = 0;
                currentDialog = dialog.JumpID;
                StopAllCoroutines();
                StartCoroutine(AnimateText());
            }
            else if (dialog.JustEnds)
            {
                DialogEnd();
            }
            else if (dialog.EndsInChoice)
            {
                currentMessage = 0;
                ChoiceMode = true;
                NextArrow.SetActive(false);
            }
            else if (dialog.BoolJump)
            {
                currentMessage = 0;
                currentDialog = dialog.JumpBool.Test() ? dialog.JumpBool.TrueID : dialog.JumpBool.FalseID;
                StopAllCoroutines();
                StartCoroutine(AnimateText());
            }
            else if(dialog.FavorJump)
            {
                currentMessage = 0;
                Character current = CharacterInfo.instance.GetByName(dialog.JumpFavor.CharName);
                currentDialog = current.Favor >= dialog.JumpFavor.Value ? dialog.JumpFavor.AboveID : dialog.JumpFavor.BelowID;
                StopAllCoroutines();
                StartCoroutine(AnimateText());
            }
            // add the actual incorperation of variables and bools and such
            foreach(BoolChange c in dialog.BoolChanges)
            {
                if(Booleans.instance.Dictionary["METGHOST"])
                {
                    bool x = false;
                }
                if (Booleans.instance.Dictionary.ContainsKey(c.BoolName))
                {
                    Booleans.instance.Dictionary[c.BoolName] = c.Value; 
                }
                else
                {
                    Debug.Log("NoSuchBoolAs: " + c.BoolName);
                }
            }
            foreach(CharachterFavorChange c in dialog.FavorChanges)
            {
                Character current = CharacterInfo.instance.GetByName(c.CharName);
                current.Favor += c.Value;
            }
            foreach(ItemTransfer c in dialog.ItemTransfers)
            {
                if(c.Method == ItemMethod.GIVE)
                {
                    if(Booleans.instance.Inventory.ContainsKey(c.Item))
                    {
                        if (Booleans.instance.Inventory[c.Item] == true)
                            Booleans.instance.Inventory[c.Item] = false;
                        else
                            Debug.Log("YouCan'tGiveItem: " + c.Item + " BecauseYouDon'tHaveIt");
                    }
                    else
                    {
                        Debug.Log("NoSuchItemas: " + c.Item);
                    }
                }
                else if(c.Method == ItemMethod.TAKE)
                {
                    if (Booleans.instance.Inventory.ContainsKey(c.Item))
                    {
                        if (Booleans.instance.Inventory[c.Item] == false)
                            Booleans.instance.Inventory[c.Item] = true;
                        else
                            Debug.Log("YouCan'tTakeItem: " + c.Item + " BecauseYouAlreadyHaveIt");
                    }
                    else
                    {
                        Debug.Log("NoSuchItemas: " + c.Item);
                    }
                }
            }
            return "";
        }
        else
             return "ERROR";
    }

    public void DialogEnd()
    {
        TextCanvas.SetActive(false);
        DialogOpen = false;
        currentDialog = 000;
    }

    public void DialogStart()
    {
        NextArrow.SetActive(true);
        TextCanvas.SetActive(true);
        DialogOpen = true;
        currentMessage = 0;
        //StopAllCoroutines();
        StartCoroutine(AnimateText());
    }

    public void DialogStart(int ID)
    {
        currentDialog = ID;
        NextArrow.SetActive(true);
        TextCanvas.SetActive(true);
        DialogOpen = true;
        currentMessage = 0;
        //StopAllCoroutines();
        StartCoroutine(AnimateText());
    }
}
