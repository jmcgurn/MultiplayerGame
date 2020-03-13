using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMove : MonoBehaviour
{

    public GameObject player;
    

    void Start()
    {
        
    }

    public void OnClick(Vector3 pos)
    {
        var moveTo = player.GetComponent<NavigateToPos>();
        var netMove = player.GetComponent<NetworkMove>();

        moveTo.navigateTo(pos);
        netMove.OnMove(pos);
    }
  
}
