using UnityEngine;
using System.Collections;

public class Gizmo : MonoBehaviour
{

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.blue; //draw gizmos in blue color
		Gizmos.DrawWireCube (transform.position, transform.localScale);
	}
}