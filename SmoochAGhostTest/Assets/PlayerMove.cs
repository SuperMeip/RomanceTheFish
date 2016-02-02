using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Facing {Up, Down, Left, Right}

public class PlayerMove : MonoBehaviour {

    public Facing direction;
    public float moveSpeed;
    public Vector2 location;
    public Transform tr;
    MessageManager Manager;
    public Vector3 position;

    // Use this for initialization
    void Start () {
        direction = Facing.Down;
        Manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<MessageManager>();
        position = transform.position;
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Manager.DialogOpen)
        {
            if (Input.GetKey(KeyCode.D) && tr.position == position)
            {
                position += Vector3.right * 0.32f;
                direction = Facing.Right;
            }
            else if (Input.GetKey(KeyCode.A) && tr.position == position)
            {
                position += Vector3.left * 0.32f;
                direction = Facing.Left;
            }
            else if (Input.GetKey(KeyCode.W) && tr.position == position)
            {
                position += Vector3.up * 0.32f;
                direction = Facing.Up;
            }
            else if (Input.GetKey(KeyCode.S) && tr.position == position)
            {
                position += Vector3.down * 0.32f;
                direction = Facing.Down;
            }

            transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * moveSpeed);
        }
    }

}
