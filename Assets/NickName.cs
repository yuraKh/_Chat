using UnityEngine;
using System.Collections;

public class NickName : MonoBehaviour
{

	public float nameplankahgt = 1f;
	public string npc_name ;// Имя игрока
	char[] cArray = new char[20];
	int arrL;



	// Use this for initialization
	void Start ()
	{
		npc_name = GameObject.Find ("Camera").GetComponent<Server> ().nickname;
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
		// Якщо персонаж наш то відправляємо дані на сервер
		
		if (stream.isWriting) {
			cArray = npc_name.ToCharArray ();
			arrL = cArray.Length;
			stream.Serialize (ref arrL);
			for (int i=0; i<arrL; i++) {
				stream.Serialize (ref cArray [i]);
			}

		} 
		// В протилежному випадку, якщо персонаш не наш, то 
		// читаємо координати з сервера 
		else {
			npc_name = "";
			stream.Serialize (ref arrL);
			for (int i=0; i<arrL; i++) {
				stream.Serialize (ref cArray [i]);
				print (cArray [i]);
				npc_name += cArray [i];
			}
		}

	}
	// Update is called once per frame
	void Update ()
	{
	
	}
}
