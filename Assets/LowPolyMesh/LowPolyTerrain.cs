using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class LowPolyTerrain : LowPolyMesh
{
	public TerrainData terrainData;

	public Gradient steepnessPallet;

	protected override void Start ()
	{
		sampleScale = Mathf.Clamp(sampleScale,terrainData.size.x/104, terrainData.size.x);
		resolution = Mathf.RoundToInt(terrainData.size.x/sampleScale);
		
		mapRelation = ((float)terrainData.heightmapResolution)/resolution;
		heightScale = height/sampleScale;

		base.Start ();
	}

	protected override void CompleteMesh ()
	{
		ReloadColor();
	}

	protected override float GetHeight (int x, int y)
	{
		return terrainData.GetHeight((int)(pos.x*mapRelation), (int)(pos.y*mapRelation))*heightScale;
	}

	public void GenerateTerrain()
	{
		index = 0;
		pos = Vector2.zero;
		StartCoroutine("Generate",60000);
	}
	public void ReloadColor()
	{
		Vector3 trianglePos = Vector3.zero;
		float highest=0, lowest=float.MaxValue;
		Color steepness;
		for (int vert = 2; vert < verts.Length; vert+=3)
		{
			trianglePos = (verts[vert-2] + verts[vert-1] + verts[vert])/3;

			for(int i=0; i < 3; i++)
			{
				lowest = (verts[vert-i].y < lowest) ? verts[vert-i].y : lowest;
				highest = (verts[vert-i].y > highest) ? verts[vert-i].y : highest;
			}
			steepness = steepnessPallet.Evaluate(trianglePos.y/(highest-lowest+0.001f));
			colors[vert] = colorPallet.Evaluate((trianglePos.y/height+Random.value*colorRandomness)/terrainData.heightmapHeight);
			colors[vert] += steepness * steepness.a;
			colors[vert-1] = colors[vert];
			colors[vert-2] = colors[vert];

		}
		mesh.colors = colors;
	}

	public void Flatten(float height)
	{
		for(int i = 0; i < verts.Length; i++)
		{
			verts[i] = new Vector3(verts[i].x, height, verts[i].z);
		}
		mesh.vertices = verts;
	}
}
