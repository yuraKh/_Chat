using UnityEngine;
using System.Collections;

public class CameraControl: MonoBehaviour
{
	//zoom variables
	public Vector2 zoomRange = new Vector2 (-5, 5);   //possible range to zoom up and down
	public float zoomSpeed = 1f;    //zoom speed factor
	private float currentZoom = 0;  //old zoom factor
	private Vector3 initPos;    //transform start position

	//rotation variables
	public float xRotSpeed = 100f;     //rotation speed on x axis
	public float yRotSpeed = 100f;     //rotation speed on y axis
	public float rotDamp = 5f;   //damping factor
	public float yMinRotLimit = -10f;  //y limit upwards
	public float yMaxRotLimit = 60f;  //y limit downwards
	private float xDeg;     //Mouse X axis input var
	private float yDeg;     //Mouse Y axis input var
	private Quaternion desiredRotation;     //returned rotation based on xDeg and yDeg
	private Quaternion currentRotation;     //current camera rotation
	private Quaternion rotation;    //rotation with damping ( final rotation )
	private Quaternion desiredRotationCamera;
	private Quaternion currentRotationCamera;
	private Quaternion rotationCamera;


	void Start ()
	{
		initPos = transform.position;   
		rotation = Camera.main.transform.rotation;
		currentRotation = transform.rotation;
		desiredRotation = Camera.main.transform.rotation;

		//grab current angle values as starting points, clamp to return positive values
		xDeg = ClampAngle (transform.eulerAngles.y, 0f, 360f);
		yDeg = ClampAngle (Camera.main.transform.eulerAngles.x, 0f, 360f);
	}
    #region ROTATION
	public void Rotate ()
	{
		//ROTATION
		if (Input.GetMouseButton (1)) {
			//Значення осей перемножеємо на швидкість та уповільнюємо
			xDeg += Input.GetAxis ("Mouse X") * xRotSpeed * 0.02f;
			yDeg -= Input.GetAxis ("Mouse Y") * yRotSpeed * 0.02f;

			//перевіряємо чи yDeg не перевищує граничні значення    
			yDeg = ClampAngle (yDeg, yMinRotLimit, yMaxRotLimit);

			//поворот контейнера навколо осі Y
			desiredRotation = Quaternion.Euler (0, xDeg, 0);

			//поворот камери навколо осі X
			desiredRotationCamera = Quaternion.Euler (yDeg, 0, 0);

			//отримуємо теперішній поворот контейнера в глобальних координатах
			currentRotation = transform.rotation;

			//отримуємо теперішній поворот камери в локальних координатах
			currentRotationCamera = Camera.main.transform.localRotation;

			// отримуємо кінцевий поворот контейнера
			rotation = Quaternion.Lerp (currentRotation, desiredRotation, Time.deltaTime * rotDamp);

			//отримуємо кінцевий поворот камери 
			rotationCamera = Quaternion.Lerp (currentRotationCamera, desiredRotationCamera, Time.deltaTime * rotDamp);

			//призначаємо кінцевий поворот контейнера в глобальних координатах
			transform.rotation = rotation;

			//призначаємо поворот камери в локальних координатах
			Camera.main.transform.localRotation = rotationCamera;

		}
	}
    #endregion


	void Update ()
	{

		//ZOOM IN/OUT

		currentZoom -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * 1000 * zoomSpeed;
		//make sure that zoom position is still in zoomRange limit
		currentZoom = Mathf.Clamp (currentZoom, zoomRange.x, zoomRange.y);
		//get current camera position and calculate new zoomed in/out position based on old positions
		Vector3 pos = transform.position;
		pos.y -= (transform.position.y - (initPos.y + currentZoom)) * 0.1f;
		//assign new position to camera position
		transform.position = pos;
        
		if (Input.GetMouseButton (0)) {
			float fx = -Input.GetAxis ("Mouse X") * 0.1f;
			float fy = -Input.GetAxis ("Mouse Y") * 0.1f;


			if (!CheckBounds (-transform.right)) {
				fx = Mathf.Clamp (fx, 0, 1);
			}
			if (!CheckBounds (transform.right)) {
				fx = Mathf.Clamp (fx, -1, 0);
			}
			if (!CheckBounds (-transform.forward)) {
				fy = Mathf.Clamp (fy, 0, 1);
			}
			if (!CheckBounds (transform.forward)) {
				fy = Mathf.Clamp (fy, -1, 0);
			}
			transform.position += transform.TransformDirection (new Vector3 (fx, 0, fy));

		}


		Rotate ();

	}

	private static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}

	private bool CheckBounds (Vector3 direction)
	{
		if (Physics.Raycast (transform.position, direction, 5)) {
			return false;
		} else
			return true;
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay (transform.position, transform.right * 5);
		Gizmos.DrawRay (Camera.main.transform.position, Camera.main.transform.right * 5);

		Gizmos.color = Color.blue;
		Gizmos.DrawRay (transform.position, transform.forward * 5);
		Gizmos.DrawRay (Camera.main.transform.position, Camera.main.transform.forward * 5);

		Gizmos.color = Color.green;
		Gizmos.DrawRay (transform.position, transform.up * 5);
		Gizmos.DrawRay (Camera.main.transform.position, Camera.main.transform.up * 5);

		Gizmos.color = Color.cyan;
		Gizmos.DrawRay (transform.position, -transform.forward * 5);
		Gizmos.DrawRay (Camera.main.transform.position, -Camera.main.transform.forward * 5);

		Gizmos.color = Color.yellow;
		Gizmos.DrawRay (transform.position, -transform.right * 5);
		Gizmos.DrawRay (Camera.main.transform.position, -Camera.main.transform.right * 5);

	}
}