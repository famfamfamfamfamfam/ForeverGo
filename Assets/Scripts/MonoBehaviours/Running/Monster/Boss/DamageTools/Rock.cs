using UnityEngine;

public class Rock : MonoBehaviour
{
    int playerLayerMask, groundLayerMask;
    void Start()
    {
        playerLayerMask = LayerMask.GetMask("Player");
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    float h;
    void Update()
    {
        h = Mathf.Abs(transform.position.y);
        if (h == 0)
            h = 0.1f;
        transform.position -= Vector3.up * Mathf.Sqrt(2 * 9.81f * h) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayerMask)
        {
            GameManager.instance.OnAttack(MonstersManager.instance.boss, other.gameObject);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == groundLayerMask)
            gameObject.SetActive(false);
    }
}
