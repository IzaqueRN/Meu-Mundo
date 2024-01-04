using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao : MonoBehaviour
{
    public float velocidade = 3;
    public float velocidadeMouse = 2;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    
    void Update()
    {



        //transform.Rotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

        transform.localEulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * velocidadeMouse;

        if (Input.GetKey(KeyCode.Space)) {

            transform.position += new Vector3(0, velocidade * Time.deltaTime,0);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {

            transform.position -= new Vector3(0, velocidade * Time.deltaTime, 0);
        }

        /*
        if(Input.GetKey(KeyCode.D)) { transform.position += new Vector3(velocidade * Time.deltaTime, 0, 0);}

        if (Input.GetKey(KeyCode.A)) { transform.position -= new Vector3(velocidade * Time.deltaTime, 0, 0); }

        if (Input.GetKey(KeyCode.W)) { transform.position += new Vector3(0, 0, velocidade * Time.deltaTime); }

        if (Input.GetKey(KeyCode.S)) { transform.position -= new Vector3(0, 0, velocidade * Time.deltaTime); }
        */

        if (Input.GetKey(KeyCode.W)) { transform.position += new Vector3(0, 0, velocidade * Time.deltaTime) + transform.forward; }

        if (Input.GetKey(KeyCode.S)) { transform.position -= new Vector3(0, 0, velocidade * Time.deltaTime) + transform.forward; }

    

    }

}
