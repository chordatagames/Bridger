using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class LowPolyWater : LowPolyMesh
{
	public int size = 100;

	void Start()
	{
		CreateMesh();
	}

	public override void CreateMesh()
	{
		resolution = size;

		base.CreateMesh();
	}

	protected override void CompleteMesh ()
	{
		return;
	}

	void Update()
	{
		Vector3 waveDir = new Vector3(0.0f,0.01f,0.0f);
		for (int vert = 2; vert < verts.Length; vert+=3)
		{

			colors[vert] = colorPallet.Evaluate(Mathf.PerlinNoise(Time.time+verts[vert].x/resolution,Time.time+verts[vert].y)*colorRandomness/height);
			colors[vert-1] = colors[vert];
			colors[vert-2] = colors[vert];
		}

		mesh.vertices = verts;
		mesh.colors = colors;
	}
}