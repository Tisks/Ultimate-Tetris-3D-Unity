using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using SocketIO;
using UnityEngine;
using static BGobjects;

public class BGWebSocket : MonoBehaviour
{
    private const string API_URL = "http://localhost:8000/";
    public SocketIOComponent socket;
    private float datito = 0;
    private float id_player = 0;
    private float id_videogame = 1;
    private float id_modifiable_mechanic = 1;
    private float data = 1;

    public Boomlagoon.JSON.JSONObject videogameInfo;
    AttributeResPlayer newPlayer;
    AttributePlayer attibute;

    public float Datito { get => datito; set => datito = value; }

    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        Debug.Log("30");
        Debug.Log(go);
        socket = go.GetComponent<SocketIOComponent>();
        Debug.Log("31");
        Debug.Log(socket);
        //suscripción al web socket
        socket.On("AllSensors",OnAllSensors);
        socket.On("Smessage",OnSmessage);
        socket.On("Imessage",OnImessage);
        videogameInfo = new  Boomlagoon.JSON.JSONObject();
        videogameInfo.Add("id_videogame",id_videogame);
        videogameInfo.Add("id_modifiable_mechanic",id_modifiable_mechanic);
        videogameInfo.Add("data",data);  
        Debug.Log(videogameInfo);
        StartCoroutine(ConnectToServer());
    }

    IEnumerator ConnectToServer(){
        var json = new Boomlagoon.JSON.JSONObject();
        json.Add("room","Ultimate-Tetris-3D-Unity");
        json.Add("name","Ultimate-Tetris-3D-Unity");
        String data = json.ToString();
        yield return new WaitForSeconds(0.5f);
        socket.Emit("join_sensor_videogame",new JSONObject(data));
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Se conecto BGWebSocket ?");
    }
    private void OnAllSensors(SocketIOEvent socketIOevent)
    {
        string data = socketIOevent.data.ToString();
        var json = Boomlagoon.JSON.JSONObject.Parse(data);
        Debug.Log("Entro al All Sensors "+ data);
    }
    private void OnImessage(SocketIOEvent socketIOevent){
        string data = socketIOevent.data.ToString();
        Debug.Log(data);
        var jj = socketIOevent.data;
        var json = Boomlagoon.JSON.JSONObject.Parse(data);
        Debug.Log(json);
    }
    
    private void OnSmessage(SocketIOEvent socketIOevent)
    {
        string data = socketIOevent.data.ToString();
        var jj = socketIOevent.data;
        var json = Boomlagoon.JSON.JSONObject.Parse(data);
        Debug.Log("Entro al All SMessage: "+ data);
        Debug.Log("Esta es la version json: "+ json);
        Debug.Log("Esta es el datito "+ (float)json.GetNumber("message"));

        Datito = (float)json.GetNumber("message");
        
        Debug.Log("DateTime.Now.Millisecond TIEMPOOOOOO: "+(new TimeSpan(DateTime.Now.Ticks)).TotalMilliseconds);
    }

    public void GetAllSensors(){
        socket.Emit("AllSensors");
    }
    public void ConnectToSensor(string sensorRoom, string nameGame){
        var json = new Boomlagoon.JSON.JSONObject();
        json.Add("room",sensorRoom);
        json.Add("name",nameGame);
        String data = json.ToString();
        socket.Emit("join_sensor",new JSONObject(data));
    }

    private static BGWebSocket s_Instance = null;

    public static BGWebSocket instance
    {
        get
        {
        
            if (s_Instance == null)
                s_Instance = FindObjectOfType(typeof(BGWebSocket)) as BGWebSocket;

            if (s_Instance == null)
            {
                GameObject obj = new GameObject("Game Controller");
                s_Instance = obj.AddComponent(typeof(BGWebSocket)) as BGWebSocket;
                Debug.Log("Could not locate an BGWebSocket object. BGWebSocket was Generated Automaticly.");
            }

            return s_Instance;
        }
    }


    // Update is called once per frame
    void Update()
    {   
        /* print("ALOOOO ?");
        Debug.Log("Esta cosa se envia nuevamente");
        socket.Emit("AllSensors");*/
        //Debug.Log("ESTA COSA ESTA CONECTADA?"+socket.autoConnect);
        //Datito = UnityEngine.Random.Range(0.0f, 100.0f);
        //socket.Emit("AllSensors");
    }

    
    public AttributePlayer JSONstrToAttribute(JSONObject jsonData){
        Debug.Log("DESCOMPONER EL JSON EN UN ATTRIBUTOOOOOOOOOOOOOOOOOOOO:");
        string data = jsonData.ToString();
        var json = Boomlagoon.JSON.JSONObject.Parse(data);
        Debug.Log("Descomprimiendo JSON: "+ data);
        string dato = (json["message"]).ToString(); //GetNumber("data");
        json = Boomlagoon.JSON.JSONObject.Parse(dato);
        Debug.Log("Dato relacionado al JSON: "+ json["data"]);
        AttributePlayer AttAux = new AttributePlayer((int)json.GetNumber("id_player"), json.GetString("nameat"), json.GetString("namecategory"),(int)json.GetNumber("data"), json.GetString("data_type"), json.GetString("input_source"), json.GetString("date_time"));
        return AttAux;
    }
    public AttributeResPlayer JSONstrToResAttribute(JSONObject jsonData){
        string data = jsonData.ToString();
        var json = Boomlagoon.JSON.JSONObject.Parse(data);
        Debug.Log("Descomprimiendo JSON: "+ data);
        string dato = (json["message"]).ToString(); //GetNumber("data");
        json = Boomlagoon.JSON.JSONObject.Parse(dato);
        Debug.Log("Dato relacionado al JSON: "+ json["data"]);
        AttributeResPlayer AttAux = new AttributeResPlayer((int)json.GetNumber("id_player"), json.GetString("nameat"),(int)json.GetNumber("data"), json.GetString("data_type"), json.GetString("date_time"));
        return AttAux;
    }


}