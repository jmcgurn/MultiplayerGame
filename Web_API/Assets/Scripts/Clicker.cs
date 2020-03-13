using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Clicking Stuff");
            Clicked();
        }
    }

    private void Clicked()
    {
        //generate raycast
        var ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.gameObject);
            Debug.Log(hit.point);
            var clickMove = hit.collider.gameObject.GetComponent<ClickMove>();
            clickMove.OnClick(hit.point);
        }
    }
}
