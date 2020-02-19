using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour
{

    static SocketIOComponent socket;
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("spawn", OnSpawned);
    }

    // Update is called once per frame
    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("connected to server");
        JSONObject data = new JSONObject();
        data.AddField("msg", "hello Yall");
        socket.Emit("yolo", data);

    }

    void OnSpawned(SocketIOEvent e)
    {
        Instantiate(playerPrefab);
    }

}
