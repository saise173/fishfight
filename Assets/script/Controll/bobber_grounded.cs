using UnityEngine;

public class bobber_grounded : MonoBehaviour
{
    public bool grounded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            grounded = true;
        }
        if (collision.gameObject.CompareTag("wall"))
        {
            grounded = true;
        }          
    }    
}
