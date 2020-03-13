using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class Network : MonoBehaviour
{

    static SocketIOComponent socket;
    public GameObject playerPrefab;
    public GameObject localPlayer;
    Dictionary<string, GameObject> players;

    // Start is called before the first frame update
    void Start()
    {
        players = new Dictionary<string, GameObject>();
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("spawn", On Spawned);
        socket.On("move", OnMove);
        socket.On("disconnected", OnDisconnected);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);
    }

    private void OnUpdatePosition(SocketIOEvent obj)
    {
        var id = obj.data["id"].ToString();
        var player = players[id];
        var pos = new Vector3(GetFloatFrpmJson(obj.data, "x"), 0, GetFloatFrpmJson(obj.data, "z"));
        player.transform.position = pos;
    }

    void OnMove(SocketIOEvent e)
    {
        Debug.Log("Network Player Is Moving " + e.data);
        var id = e.data["id"].ToString();
        var player = players[id];
        Debug.Log(player.name);
        var pos = new Vector3(GetFloatFrpmJson(e.data, "x"), 0, GetFloatFrpmJson(e.data, "z"));
        var navigatePos = player. GetComponent<NavigateToPos>();

        navigatePos.navigateTo(pos);
    }

    private void OnDisconnected(SocketIOEvent obj)
    {
        // Debug.Log("Disconnected " + obj.data);
        var player = players[obj.data["id"].ToString()];
        Destroy(player);
        players.Remove(obj.data["id"].ToString());
    }

    private void OnRequestPosition(SocketIOEvent obj)
    {
        //sends local players position to server to update on login
        socket.Emit("updatePosition", new JSONObject(VectorToJson(localPlayer.transform.position)));
    }

    float GetFloatFrpmJson(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"", ""));
    }

    public static string VectorToJson(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"",""z"":""{1}""}}", vector.x, vector.z);
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
        var player = Instantiate(playerPrefab);
        Debug.Log("Spawned " + e.data);
        players.Add(e.data["id"].ToString(), player);
        Debug.Log(players.Count);
    }

}
