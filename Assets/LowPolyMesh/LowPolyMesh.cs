using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public abstract class LowPolyMesh : MonoBehaviour
{
	protected MeshFilter filter;
	protected MeshRenderer rend;
	protected MeshCollider col;

	private int _resolution;
	public int resolution{ get{return _resolution;} protected set{_resolution = value;} }
	
	public float mapScale = 1f;
	public float sampleScale = 1f;
	public float height = 60f;
	
	public Gradient colorPallet; //normalized by height
	public float colorRandomness;
	
	protected float mapRelation;

	protected Mesh mesh;
	protected Vector3[] verts;
	protected Vector3[] normals;
	protected int[] triangles;
	protected Color[] colors;

	protected int index = 0;
	protected Vector2 pos = Vector2.zero;

	public bool validMesh{get {return mesh != null;}}

	public virtual void CreateMesh()
	{	
		mesh = new Mesh();
		filter = GetComponent<MeshFilter>();
		filter.mesh = mesh;

		col = GetComponent<MeshCollider>();
		col.sharedMesh = mesh;

		int arraySize = Mathf.RoundToInt(_resolution * _resolution*6); //two triangles per sample

		triangles = new int[arraySize]; 
		verts = new Vector3[arraySize];
		normals = new Vector3[arraySize];
		colors = new Color[arraySize];


		Generate();
		mesh.Optimize();
		mesh.RecalculateNormals();
		normals = mesh.normals;
	}

	protected void Generate()
	{
		while(pos.x != _resolution)
		{
			while (pos.y != _resolution)
			{
				AssignSample(); //[0,0] 0
				
				IteratorMove(0, 1);
				AssignSample(); //[0,1] 1
				
				IteratorMove(1, -1);
				AssignSample(); //[1,0] 2
				
				//NEW TRIANGLE
				IteratorMove(0, 0);
				AssignSample(); //[1,0] 0
				
				IteratorMove(-1, 1);
				AssignSample(); //[0,1] 1
				
				IteratorMove(1, 0);
				AssignSample(); //[1,1] 2
				
				//NEW TRIANGLE
				IteratorMove(-1, 0);
			}
			pos += -Vector2.up*_resolution + Vector2.right;
		}
		mesh.vertices = verts;
		mesh.triangles = triangles;

		index = 0;
		pos = Vector2.zero;

		CompleteMesh();
	}

	public virtual void ReloadColor()
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

	protected abstract void CompleteMesh();

	protected virtual float GetHeight(int x, int y)
	{
		return 0;
	}

	void AssignSample()
	{
		verts[index] = new Vector3(pos.x, GetHeight((int)pos.x, (int)pos.y), pos.y)*sampleScale*mapScale;
		triangles[index] = index;
	}

	void IteratorMove(int x, int y)
	{
		pos += new Vector2(x, y);
		index++;
	}

}
