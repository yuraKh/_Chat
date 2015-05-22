using UnityEngine;
using System.Collections;

public class NickName : MonoBehaviour
{

	public float nameplankahgt = 1f;
	public string npc_name, load = "";// Имя игрока
	char[] cArray = new char[20];
	int arrL;
	bool ifMas = false;
    
    
	// Use this for initialization
	void Start ()
	{
		npc_name = GameObject.Find ("Camera").GetComponent<Server> ().nickname;
	}
	void Update ()
	{

	}
	void OnGUI ()
	{
		Vector3 pos = new Vector3 (transform.position.x, transform.position.y + nameplankahgt, transform.position.z);
		Vector3 crd = Camera.main.WorldToScreenPoint (pos);
		crd.y = Screen.height - crd.y;
		GUIStyle style = new GUIStyle ();
		style.fontSize = 12;//Размер 12
		style.normal.textColor = Color.red;//Красный цвет
		style.alignment = TextAnchor.MiddleCenter;
		GUI.Label (new Rect (crd.x - 120, crd.y, 240, 18), npc_name, style);// X позиция, Y позиция, ширина, высота
	}

	
	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting) {
			bool flag = ifMas || (Server.ifServer && load != "");
			stream.Serialize (ref flag);
			if (flag) {
				//stream.Serialize (ref ifMas);
				if (ifMas) {

					cArray = npc_name .ToCharArray ();
				} else {
					cArray = load.ToCharArray ();
				}
				arrL = cArray.Length;
				stream.Serialize (ref arrL);
				for (int i=0; i<arrL; i++) {
					stream.Serialize (ref cArray [i]);
				}
				if (ifMas)
					GameObject.Find ("Camera").GetComponent<Server> ().nickname = npc_name;
				npc_name = "";
				ifMas = false;
			}
		} else {
			stream.Serialize (ref ifMas);
			if (ifMas) {
				npc_name = "";
				stream.Serialize (ref arrL);
				for (int i=0; i<arrL; i++) {
					stream.Serialize (ref cArray [i]);
					print (cArray [i]);
					npc_name += cArray [i];
				}
				GameObject.Find ("Camera").GetComponent<Server> ().nickname = npc_name;
				load = npc_name;
				npc_name = "";
				ifMas = false;
			}
		}
	}
}
