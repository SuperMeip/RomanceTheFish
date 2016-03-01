using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour
{
    /// <summary>
    /// Don't touch this
    /// </summary>
    GameObject Player;
    /// <summary>
    /// The character that will be talking
    /// </summary>
    public Characters character;
    public Facing Direction;
    /// <summary>
    /// Appears when this bool is true
    /// </summary>
    public string BoolOn;
    /// <summary>
    /// don't touch this
    /// </summary>
    MessageManager Manager;
    /// <summary>
    /// The ID of the message this instance of the character will start it's dialog on
    /// </summary>
    public int DialogStartID;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<MessageManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ExistsWhile(BoolOn, this.gameObject);
        if (Input.GetKeyDown(KeyCode.E) && !Manager.DialogOpen)
        {
            if (isPlayerFocus())
                Manager.DialogStart(DialogStartID);
        }
    }

    bool isPlayerFocus()
    {
        switch (Player.GetComponent<PlayerMove>().direction)
        {
            case Facing.Up:
                Vector3 up = transform.position + Vector3.down * 0.32f;
                if (Player.transform.position == up)
                    return true;
                else
                    return false;
            case Facing.Down:
                if (Player.transform.position == (transform.position + Vector3.up * 0.32f))
                    return true;
                else
                    return false;
            case Facing.Left:
                if (Player.transform.position == (transform.position + Vector3.right * 0.32f))
                    return true;
                else
                    return false;
            case Facing.Right:
                if (Player.transform.position == (transform.position + Vector3.left * 0.32f))
                    return true;
                else
                    return false;
        }
        return true;
    }

    void ExistsWhile(string Boolean, GameObject Character)
    {
        if (Boolean == "")
            return;
        if (Booleans.instance.Dictionary.ContainsKey(Boolean))
        {
            Character.SetActive(Booleans.instance.Dictionary[Boolean]);
        }
        else
        {
            Debug.Log("NoSuchBoolAs: " + Boolean);
        }
    }
}