using UnityEngine;

public class GreenPigScript : MonoBehaviour
{
    private bool isCaught = false;
    private UIScript uiScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiScript = FindAnyObjectByType<UIScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < - 6)
        {
            uiScript.AddScore(-1);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Net"))
        {
            isCaught = true;
            Destroy(this.gameObject);
            uiScript.AddScore(1);
        }
    }

    public bool GetIsCaught()
    {
        return isCaught;
    }
}
