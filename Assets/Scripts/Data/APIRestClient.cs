using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using SocketIO;
using UnityEngine;
using static BGobjects;

public class APIRestClient : MonoBehaviour
{
    private const string API_URL = "http://localhost:8000/";
    public SocketIOComponent socket;
    private float datito = 1;
    AttributeResPlayer newPlayer;
    AttributePlayer attibute;

    public float Datito { get => datito; set => datito = value; }

    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        //suscripción al web socket
        socket.On("AllSensors",OnAllSensors);
        socket.On("Smessage",OnSmessage);

        StartCoroutine(ConnectToServer());
    }

    IEnumerator ConnectToServer(){
        var json = new Boomlagoon.JSON.JSONObject();
        json.Add("room","SensorCerebral");
        json.Add("name","Juego_Pong");
        String data = json.ToString();
        yield return new WaitForSeconds(0.5f);
        socket.Emit("join_sensor",new JSONObject(data));
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Se conecto ?");
    }
    private void OnAllSensors(SocketIOEvent socketIOevent)
    {
        string data = socketIOevent.data.ToString();
        var json = Boomlagoon.JSON.JSONObject.Parse(data);
        Debug.Log("Entro al All Sensors "+ data);
    }
    
    private void OnSmessage(SocketIOEvent socketIOevent)
    {
        string data = socketIOevent.data.ToString();
        var jj = socketIOevent.data;
        var json = Boomlagoon.JSON.JSONObject.Parse(data);
        Debug.Log("Entro al All SMessage: "+ data);
        string dato = (json["message"]).ToString(); //GetNumber("data");
        json = Boomlagoon.JSON.JSONObject.Parse(dato);
        Debug.Log("Entro al All SMessage 2 obtiene data: "+ json["data"]);
        json = Boomlagoon.JSON.JSONObject.Parse(dato);
        Datito = (float)json.GetNumber("data");
        
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

    private static APIRestClient s_Instance = null;

    public static APIRestClient instance
    {
        get
        {
        
            if (s_Instance == null)
                s_Instance = FindObjectOfType(typeof(APIRestClient)) as APIRestClient;

            if (s_Instance == null)
            {
                GameObject obj = new GameObject("Game Controller");
                s_Instance = obj.AddComponent(typeof(APIRestClient)) as APIRestClient;
                Debug.Log("Could not locate an APIRestClient object. APIRestClient was Generated Automaticly.");
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
        //Datito = UnityEngine.Random.Range(0.0f, 100.0f);
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