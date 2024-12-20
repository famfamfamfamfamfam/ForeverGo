using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Rigidbody rb;

    const float GRAVITY = 9.81f;
    float expectHeight = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 5);
    }

    bool jumped;
    Vector3 currentVelocity;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jumped)
        {
            rb.velocity += new Vector3(0, Mathf.Sqrt(2f * GRAVITY * expectHeight), 0);
            currentVelocity = rb.velocity;
            jumped = true;
        }
        if (jumped)
        {
            currentVelocity.y -= Time.deltaTime * GRAVITY;
            currentVelocity.y = Mathf.Max(-GRAVITY, currentVelocity.y);
            rb.velocity = currentVelocity;
            if (rb.velocity.y == -GRAVITY)
                jumped = false;
        }
    }
}
