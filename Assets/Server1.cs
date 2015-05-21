using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Server1 : MonoBehaviour
{


	public bool conect = false;
	public static string text = "";
	public GameObject Client;
	public static bool ifServer = false;

	//

	public InputField ip;
	public InputField port;
	public InputField nickname;
	public Text con;

	void Start ()
	{
		ip.text = "127.0.0.1";
		port.text = "5000";
	}

	public void ConnectServer ()
	{
		if (!conect) {
			Network.Connect (ip.text, int.Parse (port.text));
			ifServer = false;
		}
	}

	public void StartServer ()
	{
		Network.InitializeServer (5, int.Parse (port.text), false);
		ifServer = true;
	}

	public void DisconnectServer ()
	{
		Network.Disconnect (200);
	}

	void OnConnectedToServer ()
	{
		conect = true;
		Network.Instantiate (Client, Vector3.zero, Quaternion.identity, 0);
	}
	
	void OnServerInitialized ()
	{
		conect = true;
		Network.Instantiate (Client, Vector3.zero, Quaternion.identity, 0);
	}
	
	void OnDisconnectedFromServer (NetworkDisconnection nd)
	{
		conect = false;
		Application.LoadLevel (Application.loadedLevel);
	}
	
	void OnPlayerDisconnected (NetworkPlayer np)
	{
		Network.RemoveRPCs (np);
		Network.DestroyPlayerObjects (np);
	}


	void Update ()
	{
		con.text = Network.connections.Length.ToString ();
	}
}
