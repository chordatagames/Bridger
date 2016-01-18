using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class LowPolyWater : LowPolyMesh
{
	public int size = 100;
	public float waveScale = 1, waveHeight = 1;

	void Start()
	{
		CreateMesh();
	}

	public override void CreateMesh()
	{
		sampleScale = Mathf.Clamp(sampleScale,size/104, size);
		resolution = Mathf.RoundToInt(size/sampleScale);
//		resolution = res;


		base.CreateMesh();
	}

	protected override void CompleteMesh ()
	{
		ReloadColor();
	}
	protected override float GetHeight (int x, int y)
	{
		float xCoord = (Time.time+x) / size * waveScale;
		float yCoord = (Time.time+y) / size * waveScale;
		return Mathf.PerlinNoise(xCoord, yCoord);
	}

	public override void ReloadColor ()
	{
		Vector3 trianglePos = Vector3.zero;
		float highest=0, lowest=float.MaxValue;
		
		for (int vert = 2; vert < verts.Length; vert+=3)
		{
			trianglePos = (verts[vert-2] + verts[vert-1] + verts[vert])/3;
			
			for(int i=0; i < 3; i++)
			{
				lowest = (verts[vert-i].y < lowest) ? verts[vert-i].y : lowest;
				highest = (verts[vert-i].y > highest) ? verts[vert-i].y : highest;
			}
			colors[vert] = colorPallet.Evaluate((trianglePos.y+Random.value*colorRandomness)/height);
			colors[vert-1] = colors[vert];
			colors[vert-2] = colors[vert];
			
		}
		mesh.colors = colors;
	}

	void Update()
	{
		Generate();
		//		Vector3 waveDir = new Vector3(0.0f,0.01f,0.0f);
	}
}