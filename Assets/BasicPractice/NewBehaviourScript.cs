using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 5);
    }

    bool jump;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jump)
        {
            jump = true;
        }
    }

    const float GRAVITY = -9.81f;

    private void FixedUpdate()
    {
        if (jump)
        {

            if (rb.velocity.y == 0)
            {
                jump = false;
            }
        }
    }
}
