using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using SocketIO;
using System.Threading;
using System.Threading.Tasks;

public class Movement : MonoBehaviour {
	public float timestep = 1F; 
	float time;
    private BGWebSocket APIREST;
    private BGobjects.AttributePlayer attAux;
    public float datito = 1; 
	public float speed = 0.05F; 
	public float dato = 0;


	//The actual group which can rotate and will move down
	public GameObject actualGroup; 

	public void startGame(){
		actualGroup = this.gameObject.GetComponent<GroupSpawner> ().spawnNext ();
		BGWebSocket.instance.socket.On("Smessage",OnSmessag);
	}
	 private void OnSmessag(SocketIOEvent socketIOevent)
    {
        Debug.Log("ENTRA EN EL CUSTIOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        attAux = BGWebSocket.instance.JSONstrToAttribute(socketIOevent.data);
        string data = attAux.Dato.ToString();
        Debug.Log("EL DATO QUE LLEGO EeeeeeeeeeeeeeeeEESS: "+ data);
    }
	//Move down in interval of timestep
	void Update () {
		time += Time.deltaTime; 
		if (time > timestep) {
			time = 0; 
			if (actualGroup != null) {
				move (Vector3.down); 
			}
		}
		checkForInput (); 
	}

	void checkForInput(){
		if (Input.GetKeyDown (KeyCode.R)) {
			actualGroup.GetComponent<Rotation>().rotateRight (); 
		} else if (Input.GetKeyDown (KeyCode.L)) {
			actualGroup.GetComponent<Rotation>().rotateLeft (); 
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			move (Vector3.left);
		} else if (Input.GetKeyDown (KeyCode.D)) {
			move (Vector3.right);
		}
		if (Input.GetKey (KeyCode.S)) {
			timestep = 0.05F; 
		} else {
			setNewSpeed(); 
		}
		gameObject.GetComponent<CubeArray> ().getCubePositionFromScene ();
	}


	//Speed increasement found at http://www.colinfahey.com/tetris/tetris.html 5.10 
	public void setNewSpeed(){
		dato = BGWebSocket.instance.Datito;
		if(dato != 0){
			var cancellationTokenSource = new CancellationTokenSource();
			var cancellationToken = cancellationTokenSource.Token;
			speed = 0.25F;
			dato = 0;
			BGWebSocket.instance.Datito = 0;

			Task.Delay(6000).ContinueWith(async (t) =>
			{
				speed = 0.05F;		
			}, cancellationToken);     
		}
		timestep = ((10 - gameObject.GetComponent<Highscore> ().level) * speed);
	}

	void move(Vector3 pos){
		actualGroup.transform.position += pos; 
		if (!gameObject.GetComponent<CubeArray> ().getCubePositionFromScene ()) {
			actualGroup.transform.position -= pos; 
			GameObject.Find("CantMove").GetComponent<AudioSource>().Play();
			if(pos == Vector3.down){
				spawnNew (); 
			}
		}
	}

	//Handle spawning a new group and check if there is any intersection after spawning
	private void spawnNew(){
		actualGroup.GetComponent<Rotation> ().isActive = false; 
		actualGroup = gameObject.GetComponent<GroupSpawner> ().spawnNext ();
		actualGroup.GetComponent<Rotation> ().isActive = true;
		if (!gameObject.GetComponent<CubeArray> ().getCubePositionFromScene ()) {
			// Game over :/
			Application.LoadLevel (Application.loadedLevelName); 
		} else {
			gameObject.GetComponent<CubeArray> ().checkForFullLine ();
		} 
	}
}