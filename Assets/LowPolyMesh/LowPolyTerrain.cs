using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class LowPolyTerrain : LowPolyMesh
{
	public TerrainData terrainData;
	protected float heightScale;

	public Gradient steepnessPallet;
	[Range(0,360)]
	public float angle = 0.0f;
	public Vector3 direction {get {return new Vector3(Mathf.Cos(angle*Mathf.Deg2Rad),0,Mathf.Sin(angle*Mathf.Deg2Rad));} }

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

	protected override float GetHeight (int x, int y)
	{
		return terrainData.GetHeight((int)(mapRelation*pos.x), (int)(mapRelation*pos.y))/sampleScale;
	}

	public void Flatten(float height)
	{
		for(int i = 0; i < verts.Length; i++)
		{
			verts[i] = new Vector3(verts[i].x, height, verts[i].z);
		}
		mesh.vertices = verts;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(transform.position, transform.position+(direction*100));
	}
}
