using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class LowPolyTerrain : LowPolyMesh
{
	public TerrainData terrainData;
	protected float heightScale;

	public Gradient steepnessPallet;

	public override void CreateMesh()
	{
		sampleScale = Mathf.Clamp(sampleScale,terrainData.size.x/104, terrainData.size.x);
		resolution = Mathf.RoundToInt(terrainData.size.x/sampleScale);
		
		mapRelation = ((float)terrainData.heightmapResolution)/resolution;
//		heightScale = 

		base.CreateMesh();
	}

	protected override void CompleteMesh ()
	{
		ReloadColor();
	}

	protected override float GetHeight (int x, int y)
	{
		return terrainData.GetHeight((int)(pos.x*mapRelation), (int)(pos.y*mapRelation))/sampleScale;
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
