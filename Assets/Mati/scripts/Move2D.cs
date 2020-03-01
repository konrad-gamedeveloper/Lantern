using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2D : MonoBehaviour
{
    public bool crouch = false;
    public float moveSpeed = 5f;
    public bool isGrounded = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Vector3 movment = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movment * Time.deltaTime * moveSpeed;
        if (Input.GetKeyDown("s") && crouch == false)
        {

            moveSpeed = moveSpeed / 2;
            crouch = true;

        }
        if (Input.GetKeyDown("w") && crouch == true)
        {
            moveSpeed = moveSpeed * 2;
            crouch = false;
        }
        if (Input.GetKeyDown("space") && crouch == true)
        {
            moveSpeed = moveSpeed * 2;
            crouch = false;
        }
        void Jump()
        {
            if (Input.GetButtonDown("Jump") && isGrounded == true)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);

            }
        }

    }
}
