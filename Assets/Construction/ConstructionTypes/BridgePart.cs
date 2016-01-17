using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
namespace Bridger
{
	[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
	public class BridgePart : MonoBehaviour, IReloadable, IRevertable
	{
		public static bool showStress = true;

		public bool editing = true;
        public GameObject jointPrefab;
        public BridgePartType partType;

		public Rigidbody2D rigid;

		private Vector2 _partOrigin;
		public Vector2 partOrigin 	{ get{return _partOrigin;} private set{_partOrigin = value;} }
		public Vector2 partEnd 		{ get{return Grid.ToGrid(transform.position + transform.right * partLength/2);} }

		public float partLength	{ get{return transform.localScale.x;} }
		public float partMass	{ get{return partLength * partType.massPerLength;} }

        public BridgeJoint originConnection;
        public BridgeJoint endConnection;

		TransformData resetTransform;

		void Start()
		{
		}

		void Update()
		{
			
		}

		public static BridgePart Create(BridgePart part, Vector2 position)
		{
			BridgePart instance = Instantiate<BridgePart>(part);
			instance.partOrigin = Grid.ToGrid(position);
			instance.rigid = instance.GetComponent<Rigidbody2D>();
			instance.rigid.isKinematic = true;
			instance.transform.position = (Vector3)instance.partOrigin;
			return instance;
		}

		public void Stretch(Vector2 strech)
		{
			editing = true;
			Vector2 relation = Grid.ToGrid (Vector2.ClampMagnitude (strech - partOrigin, partType.maxLength));
			transform.position = (partOrigin + relation / 2);
			transform.rotation = Quaternion.AngleAxis (Angles.Angle (Vector2.right, relation, false), Vector3.forward);
			transform.localScale = new Vector3 (relation.magnitude, 1, 1);
		}
		public void EndStretch()
		{
			editing = false;
			if(partLength < Grid.gridSize)
			{
				Destroy(gameObject);
				return;
			}
            originConnection = SetupConnectionAtPosition(partOrigin);
            endConnection = SetupConnectionAtPosition(partEnd);


            rigid.mass = partMass;
			resetTransform = new TransformData(transform);
			Level.AddToLevel(this);
		}
        
		BridgeJoint CreateBridgeJoint(Vector2 position)
		{
			BridgeJoint joint = Instantiate<GameObject>(jointPrefab).GetComponent<BridgeJoint>(); //TODO remove the connection though ConstructionHandler
            joint.transform.position = (Vector3)position + Vector3.back * 9;
            joint.transform.parent = transform;
            joint.transform.localScale = new Vector3(1 / partLength, 1, 1);
			joint.transform.localRotation = Quaternion.identity;
			return joint;
		}

        BridgeJoint SetupConnectionAtPosition(Vector2 pos)
        {
            Collider2D[] joints = Physics2D.OverlapPointAll(pos, 1 << 9);
            BridgeJoint connection;
            if (joints.Length > 0)
            {
                connection = joints[0].GetComponent<BridgeJoint>();//Obtain an already placed bridgeparts connections.
                connection.ConnectPart(joints[0].transform.parent.GetComponent<BridgePart>(), this, pos);
                return connection;
            }
            else
            {
                return CreateBridgeJoint(Grid.ToGrid(pos)); //Create a joint
            }
        }

        public void Detach(BridgeJoint bridgejoint)
		{
			//if(_connectedTo.Contains(bridgejoint))
			//{
			//	bridgejoint.enabled = false;
			//	bridgejoint.joint.enabled = false;
			//}
		}

		public void DetachAll()//TODO rename function
		{
			//foreach(BridgeJoint bridgejoint in _connectedTo)
			//{
			//	bridgejoint.enabled = false;
			//	bridgejoint.joint.enabled = false;
			//}
		}

		public void Attach(BridgeJoint bridgejoint)
		{
			//if(_connectedTo.Contains(bridgejoint))
			//{
			//	bridgejoint.enabled = true;
			//	bridgejoint.joint.enabled = true;
			//}
		}

		public void AttachAll()//TODO rename function
		{
			//foreach(BridgeJoint bridgejoint in _connectedTo)
			//{
			//	bridgejoint.enabled = true;
			//	bridgejoint.joint.enabled = true;
			//}
		}

        void FixedUpdate()
        {
            if (!editing) //TODO maybe add a check for build/play-mode
            {
                if (showStress)
                {
                    // get average stress
                    float forceSum = 0.0f;
                    foreach (HingeJoint2D connection in originConnection.connections)
                    {
                        forceSum += connection.GetReactionForce(Time.fixedDeltaTime).magnitude;
                    }
                    float average = forceSum / originConnection.connections.Count;

                    forceSum = 0.0f;
                    foreach (HingeJoint2D connection in endConnection.connections)
                    {
                        forceSum += connection.GetReactionForce(Time.fixedDeltaTime).magnitude;
                        
                        ///PHYSICSPART===========================
                        float jointStrength = Mathf.Min(partType.strength, connection.GetComponent<BridgePart>().partType.strength);
                        if (partType.strength > 0)
                        {
                            Vector3 force = connection.GetReactionForce(Time.deltaTime);
                            if (force.magnitude > jointStrength)
                            {
                                connection.enabled = false;
                                Level.Slowmo();
                            }
                        }
                        ///PHYSICSPART===========================

                    }
                    average += forceSum / endConnection.connections.Count;
                    float fraction = (average / 2) / partType.strength;

                    foreach (MeshRenderer r in gameObject.GetComponentsInChildren<MeshRenderer>())
                    {
                        r.material.color = new Color(fraction, 1 - fraction, 0);
                    }
                }
                foreach (HingeJoint2D joint in endConnection.connections)
                {
                    
                }
            }
        }

		public void Reset()
		{
			resetTransform.Reload(transform);

			rigid.isKinematic = true;
			rigid.velocity = Vector2.zero;
			rigid.angularVelocity = 0.0f;

            foreach (HingeJoint2D joint in endConnection.connections)
            {
                joint.enabled = true;
            }
            //foreach (BridgeJoint bj in gameObject.GetComponents<BridgeJoint>())
            //{
            //	bj.Reset();
            //}
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
