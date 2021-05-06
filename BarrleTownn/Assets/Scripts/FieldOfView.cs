using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FieldOfView : MonoBehaviour
{
	[SerializeField] private LayerMask layerMask;
	private Mesh mesh;
	[SerializeField] MeshRenderer meshRenderer;
	private Vector3 origin;
	private float startingAngle;

	public bool isNightTime;
	[SerializeField] float fov = 365.25f;
	[SerializeField]int rayCount;
	[SerializeField] float viewDistance = 5f;

	[Header("References")]
	public GameObject black;
	[SerializeField] Camera camera;
	UniversalAdditionalCameraData camData;
	public static UnityEngine.Rendering.RenderPipelineAsset renderPipelineAsset;
	// Start is called before the first frame update
	void Start()
	{
		camData = camera.gameObject.GetComponent<UniversalAdditionalCameraData>();

		camData.SetRenderer(0);

		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		black.SetActive(false);
		isNightTime = false;

	}
	private void LateUpdate()
	{
		if (meshRenderer.enabled != isNightTime)
			meshRenderer.enabled = isNightTime;

		if (isNightTime)
		{


			origin = Vector3.zero;
			
			float angle = 0f;
			float angleIncrease = fov / rayCount;
		

			Vector3[] vertices = new Vector3[rayCount + 1 + 1];
			Vector2[] uv = new Vector2[vertices.Length];
			int[] triangles = new int[rayCount * 3];

			vertices[0] = origin;

			int vertexIndex = 1;
			int triangleIndex = 0;
			for (int i = 0; i < rayCount; i++)
			{
				Vector3 vertex;
				RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
				if (raycastHit2D.collider == null)
				{
					//no hit
					vertex = origin + GetVectorFromAngle(angle) * viewDistance;
				}
				else
				{
					//hit object
					vertex = raycastHit2D.point;
				}



				vertices[vertexIndex] = vertex;
				if (i > 0)
				{
					triangles[triangleIndex + 0] = 0;
					triangles[triangleIndex + 1] = vertexIndex - 1;
					triangles[triangleIndex + 2] = vertexIndex;

					triangleIndex += 3;
				}
				vertexIndex++;

				angle -= angleIncrease;
			}


			//vertices[1] = new Vector3(50,0);
			//vertices[2] = new Vector3(0, -50);

			//triangles[0] = 0;
			//triangles[1] = 1;
			//triangles[2] = 2;

			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.triangles = triangles;


		}
	}

	public void SetNightFOV(bool _IsWerewolf)
	{
		camData.SetRenderer(0);
		black.SetActive(true);
		isNightTime = true;
		if (_IsWerewolf)
		{
			viewDistance = 2.5f;
		}
		else
		{
			viewDistance = 1f;
		}




	}

	public void SetDayFOV()
	{
		camData.SetRenderer(1);
		black.SetActive(false);
		isNightTime = false;
	}

	public Vector3 GetVectorFromAngle(float angle)
	{
		// angle = 0 -> 360
		float angleRad = angle * (Mathf.PI / 180f);
		return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
	}


	public void SetOrigin(Vector3 origin)
	{
		this.origin = origin;
	}
	public void SetAimDirection(Vector3 aimDirection)
	{

	}

}
