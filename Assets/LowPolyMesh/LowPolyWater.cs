using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class LowPolyWater : LowPolyMesh
{
	public int size = 100;

	Vector3[] normals;

	protected override void Start ()
	{
		resolution = size;

		base.Start ();
		normals = mesh.normals;
	}

	void Update()
	{
		Vector3 waveDir = new Vector3(0.0f,0.1f * Mathf.Sin(Time.time),0.0f);
		for (int vert = 2; vert < verts.Length; vert+=3)
		{
			verts[vert-2] += waveDir * Mathf.PerlinNoise(Time.time+vert+verts[vert-2].x/resolution, Time.time+vert+verts[vert-2].y/resolution*sampleScale);
			verts[vert-1] += waveDir * Mathf.PerlinNoise(Time.time+verts[vert-1].x/resolution, Time.time+verts[vert-1].y/resolution*sampleScale);
			verts[vert]   += waveDir * Mathf.PerlinNoise(Time.time+vert+verts[vert].x/resolution, Time.time+vert+verts[vert].y/resolution*sampleScale);

			colors[vert] = colorPallet.Evaluate((verts[vert].y+Random.value*colorRandomness)/height);
			colors[vert-1] = colors[vert];
			colors[vert-2] = colors[vert];
		}
		mesh.vertices = verts;
		mesh.colors = colors;
//		yield return null;
	}
}