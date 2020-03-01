using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    public BoxCollider2D player;
    // Start is called before the first frame update
    void Start()
    {
        player = player.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            player.size = new Vector3 (1, 0.5f, 1);
            player.offset = new Vector3(0, -0.25f, 0);
        }
        if (Input.GetKeyDown("w"))
        {
            player.size = new Vector3(1, 1, 1);
            player.offset = new Vector3(0, 0, 0);
        }
        if (Input.GetKey("space"))
        {
            player.size = new Vector3(1, 1, 1);
            player.offset = new Vector3(0, 0, 0);
        }
    }
}
