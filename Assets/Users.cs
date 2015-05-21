using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Users : MonoBehaviour
{
	public string massage = "", load = "";
	char[] cArray = new char[50];
	int arrL;
	NetworkView netView;
	bool ifMas = false;
	string nick;
	public InputField field;

	// Use this for initialization
	void Start ()
	{
		nick = Camera.main.GetComponent<ServerChat> ().nickname + ": ";
		netView = GetComponent<NetworkView> ();
	}

	
	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting) {
			bool flag = ifMas || (ServerChat.ifServer && load != "");
			stream.Serialize (ref flag);
			if (flag) {
				//stream.Serialize (ref ifMas);
				if (ifMas) {
					massage = nick + massage;
					cArray = massage.ToCharArray ();
				} else {
					cArray = load.ToCharArray ();
				}
				arrL = cArray.Length;
				stream.Serialize (ref arrL);
				for (int i=0; i<arrL; i++) {
					stream.Serialize (ref cArray [i]);
				}
				if (ifMas)
					ServerChat.text += massage + "\n";
				massage = "";
				ifMas = false;
			}
		} else {
			stream.Serialize (ref ifMas);
			if (ifMas) {
				massage = "";
				stream.Serialize (ref arrL);
				for (int i=0; i<arrL; i++) {
					stream.Serialize (ref cArray [i]);
					print (cArray [i]);
					massage += cArray [i];
				}
				ServerChat.text += massage + "\n";
				load = massage;
				massage = "";
				ifMas = false;
			}
		}
	}
}