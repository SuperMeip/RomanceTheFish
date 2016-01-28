using UnityEngine;
using System.Collections;
 
public class Test : MonoBehaviour {
    [TextArea(10, 10)]
    public string testString = "Ghost:1\n<HAPPY>%Hi I'm a test ghost\n<SAD>%I'm not in the game, I'm just hear to test... but here! Take this!\n[TAKE: APPLE]\n<HAPPY>%I just found it lying around, feel free to try it.\n#EAT%2:DONT EAT%3";
    // Use this for initialization
    void Start () {
        Message testMessage = new Message(testString);
        Debug.Log(testMessage.ToString());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
