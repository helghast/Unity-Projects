using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Time for R&D and typing: 2hs */
public class QuadMesh : MonoBehaviour
{
	[SerializeField] private QuadsProperties quadProperties;
	[SerializeField] private GameObject[] movablePoints = null;
	[SerializeField] private bool editMode = false;

	private string QuadName = "Quad Mesh";
	private Vector3[] vertices, normals;
	private Mesh meshRef = null;
	private MeshCollider meshColliderRef = null;

	private bool hasChanged = false;

	// Start is called before the first frame update
	void Start()
    {
		CreateQuad(quadProperties);

		meshRef = GetComponent<MeshFilter>().mesh;
		vertices = meshRef.vertices;
		normals = meshRef.normals;

		for (int i = 0; i < vertices.Length && i < movablePoints.Length; ++i)
		{
			movablePoints[i].transform.position = vertices[i];
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (editMode)
		{
			for (int i = 0; i < vertices.Length && i < movablePoints.Length; ++i)
			{
				Vector3 position = movablePoints[i].transform.position;
				if (!vertices[i].Equals(position))
				{
					vertices[i] = position;
					hasChanged = true;
				}
			}

			if (hasChanged)
			{
				meshRef.vertices = vertices;
				meshRef.RecalculateBounds();
				meshRef.RecalculateNormals();

				meshColliderRef = gameObject.GetComponent<MeshCollider>();
				if (meshColliderRef == null)
				{
					meshColliderRef = gameObject.AddComponent<MeshCollider>();
				}
				else
				{
					meshColliderRef.sharedMesh = null;
					meshColliderRef.sharedMesh = meshRef;
				}

				hasChanged = false;
			}
		}
    }

	private void CreateQuad(QuadsProperties quad)
	{
		MeshFilter mFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer mRenderer = gameObject.AddComponent<MeshRenderer>();
		MeshCollider mCollider = null;

		Mesh localMesh = BuildQuad(quad.size);
		localMesh.name = QuadName;

		mFilter.mesh = localMesh;

		if (quad.bCollider)
		{
			mCollider = gameObject.AddComponent<MeshCollider>();
			mCollider.sharedMesh = localMesh;
		}

		mRenderer.material = quad.material;
		localMesh.RecalculateBounds();
		localMesh.RecalculateNormals();
	}

	private static Mesh BuildQuad(Vector2 size)
	{
		Mesh localMesh = new Mesh();

		// vertices
		Vector3[] vertices = new Vector3[4] {
			Vector3.zero,
			new Vector3(size.x, 0, 0),
			new Vector3(0, size.y, 0),
			new Vector3(size.x, size.y, 0)
		};

		// triangles
		int[] tris = new int[6] { 0, 2, 1, 2, 3, 1 };

		// normals
		Vector3[] normals = new Vector3[4];
		for (int i = 0; i < normals.Length; ++i)
		{
			normals[i] = -Vector3.forward;
		}

		// UVs
		Vector2[] uvs = new Vector2[4] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

		// Create quad
		localMesh.vertices = vertices;
		localMesh.triangles = tris;
		localMesh.normals = normals;
		localMesh.uv = uvs;

		return localMesh;
	}
}
