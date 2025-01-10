using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasterEgg : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SaBeyler", 120f);
    }
    void SaBeyler()
    {
        SceneManager.LoadScene(0);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1 * Time.deltaTime, -0.43f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (-1 * Time.deltaTime), -0.43f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x+1*Time.deltaTime, transform.position.y, -0.43f);

        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x + (-1 * Time.deltaTime), transform.position.y, -0.43f);
        }
    }
    public void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "kiz")
        {
            SceneManager.LoadScene(0);
        }
    }
}
