using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
namespace Bridger
{
	[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
	public class BridgePart : MonoBehaviour, IReloadable, IRevertable
	{
		TransformData resetTransform;
		public bool editing = true;
		public BridgePartType materialType;

		private Rigidbody2D rigid;

		private Vector2 _partOrigin;
		public Vector2 partOrigin 	{ get{return _partOrigin;} private set{_partOrigin = value;} }
		public Vector2 partEnd 		{ get{return Grid.ToGrid(transform.position + transform.right * partLength/2);} }

		public float partLength	{ get{return transform.localScale.x;} }
		public float partMass	{ get{return partLength * materialType.massPerLength;} }

		public BridgeJoint[] connectedTo{ get {return _connectedTo.ToArray();} }
		List<BridgeJoint> _connectedTo = new List<BridgeJoint>();
		
		void Start()
		{
			materialType.LoadType(gameObject);
		}

		public static BridgePart Create(BridgePartType type, Vector2 position)
		{
			BridgePart instance = Instantiate<GameObject>(type.model).AddComponent<BridgePart>();
			instance.partOrigin = Grid.ToGrid(position);
			instance.materialType = type;
			instance.rigid = instance.GetComponent<Rigidbody2D>();
			instance.rigid.isKinematic = true;
			instance.transform.position = (Vector3)instance.partOrigin;
			AudioSource.PlayClipAtPoint(type.placementSound,Camera.main.transform.position);
			return instance;
		}

		public void Strech(Vector2 strech)
		{
			editing = true;
			Vector2 relation = Grid.ToGrid (Vector2.ClampMagnitude (strech - partOrigin, materialType.maxLength));
			transform.position = (partOrigin + relation / 2);
			transform.rotation = Quaternion.AngleAxis (Angles.Angle (Vector2.right, relation, false), Vector3.forward);
			transform.localScale = new Vector3 (relation.magnitude, 0.25f, 2);
		}
		public void EndStrech()
		{
			editing = false;
			if(partLength < Grid.gridSize)
			{
				Destroy(gameObject);
				return;
			}
			SetupJoint(partOrigin);
			SetupJoint(partEnd);
			rigid.mass = partMass;
			resetTransform = new TransformData(transform);
			Level.AddToLevel(this);
		}

		GameObject CreateJointCollider(Vector2 position)
		{
			GameObject joint = Instantiate<GameObject>(ConstructionHandler.instance.jointBase);
			joint.transform.position = (Vector3)position + Vector3.back*9;
			joint.transform.SetParent(transform, true);
			joint.transform.localScale = Vector3.one*(Grid.gridSize*4.0f/partLength) + Vector3.up*8;
			joint.transform.localRotation = Quaternion.identity;


//			GameObject go = new GameObject("joint");//TODO set UI sprite as well
//			go.AddComponent<CircleCollider2D>().radius = Grid.gridSize/4;
//			go.layer = 9; //JOINT
//			go.transform.position = (Vector3)position;
//			go.transform.SetParent(transform, true);
//			go.transform.localScale = Vector3.one*(Grid.gridSize*2.0f/partLength);
			return joint;
		}

		/// <summary>
		///	Places joint for this part at position,
		/// if a joint is already at specified position, connect to that joint.
		/// </summary>
		/// <param name="part">Part.</param>
		/// <param name="position">Position.</param>
		public void SetupJoint(Vector2 position)
		{
			BridgeJoint joint;
			Collider2D[] joints = Physics2D.OverlapPointAll(position, 1<<9);
			if (joints.Length > 0)
			{
				joint = joints[0].transform.parent.gameObject.AddComponent<BridgeJoint>();
				joint.ConnectPart(this.rigid, position);
				_connectedTo.Add(joint);
			}
			else
			{
				CreateJointCollider(position);
			}
		}

		public void Detach(BridgeJoint bridgejoint)
		{
			if(_connectedTo.Contains(bridgejoint))
			{
				bridgejoint.enabled = false;
				bridgejoint.joint.enabled = false;
			}
		}

		public void DetachAll()//TODO rename function
		{
			foreach(BridgeJoint bridgejoint in _connectedTo)
			{
				bridgejoint.enabled = false;
				bridgejoint.joint.enabled = false;
			}
		}

		public void Attach(BridgeJoint bridgejoint)
		{
			if(_connectedTo.Contains(bridgejoint))
			{
				bridgejoint.enabled = true;
				bridgejoint.joint.enabled = true;
			}
		}

		public void AttachAll()//TODO rename function
		{
			foreach(BridgeJoint bridgejoint in _connectedTo)
			{
				bridgejoint.enabled = true;
				bridgejoint.joint.enabled = true;
			}
		}

		public void Reset()
		{
			resetTransform.Reload(transform);

			rigid.isKinematic = true;
			rigid.velocity = Vector2.zero;
			rigid.angularVelocity = 0.0f;

			foreach (BridgeJoint bj in gameObject.GetComponents<BridgeJoint>())
			{
				bj.Reset();
			}
		}
		public void StartPhysics()
		{
			rigid.isKinematic = false;
		}
		public bool Undo()
		{
			DetachAll();
			gameObject.SetActive(false);
			return !gameObject.activeInHierarchy;
		}
		public bool Redo()
		{
			AttachAll();
			gameObject.SetActive(true);
			return gameObject.activeInHierarchy;
		}
		public void Remove()
		{
			DetachAll();
			Destroy(gameObject);
		}
	}
}
