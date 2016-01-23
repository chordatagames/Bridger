using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// * have an array of all joints
/// 
/// </summary>
namespace Bridger
{
    /// <summary>
    /// The BridgeJoint class is a holder for reference data of the joints connecting parts to each other.
    /// </summary>
    public class BridgeJoint : MonoBehaviour
    {
        private BridgePart anchor;
        public List<AnchoredJoint2D> connections = new List<AnchoredJoint2D>();

        /// <summary>
        /// This adds a new hinge joint to the <paramref name="anchor"/> 
        /// and connects it to the <paramref name="connectedBody"/> through the <paramref name="position"/>
        /// </summary>
        /// <param name="anchor"></param>
        /// <param name="connectedBody"></param>
        /// <param name="position"></param>
        public void ConnectPart(BridgePart anchor, BridgePart connectedBody, Vector2 position)
        {
			AnchoredJoint2D joint;

			this.anchor = anchor;


			if (anchor is RopePart)
			{
				joint = SetupRope (anchor as RopePart, connectedBody, position);
			}
			else if (connectedBody is RopePart)
			{
				joint = SetupRope (connectedBody as RopePart, anchor, position);
			}
			else
			{
				joint = anchor.gameObject.AddComponent<HingeJoint2D>();
				joint.connectedBody = connectedBody.rigid;

				joint.anchor = Grid.ToGrid(anchor.transform.InverseTransformPoint(position));
				joint.connectedAnchor = Grid.ToGrid(connectedBody.transform.InverseTransformPoint(position));
			}


            joint.enableCollision = false;
            

            connections.Add(joint);
        }

		DistanceJoint2D SetupRope(RopePart anchor, BridgePart other, Vector2 position)
		{
			DistanceJoint2D joint = anchor.gameObject.AddComponent<DistanceJoint2D>();
			joint.connectedBody = other.rigid;

			joint.distance = anchor.partLength * 0.5f;
			joint.maxDistanceOnly = true;

			joint.anchor = Grid.ToGrid(Vector3.zero);
			joint.connectedAnchor = Grid.ToGrid(other.transform.InverseTransformPoint(position));
			return joint;
		}

        //	BridgePart _anchorPart;
        //	public BridgePart anchorPart
        //	{
        //		get
        //		{
        //			if(_anchorPart == null)
        //			{_anchorPart = GetComponent<BridgePart>();}
        //			return _anchorPart;
        //		}
        //	}
        //	BridgePart _connectedPart;
        //	public BridgePart connectedPart
        //	{
        //		get
        //		{
        //			if(_connectedPart == null)
        //			{_connectedPart = joint.connectedBody.GetComponent<BridgePart>();}
        //			return _connectedPart;
        //		}
        //	}

        //	HingeJoint2D _joint;
        //	public HingeJoint2D joint
        //	{
        //		get
        //		{
        //			if(_joint == null)
        //			{_joint = gameObject.AddComponent<HingeJoint2D>();}
        //			return _joint;
        //		}
        //	}

        //	
        //	public float jointMass		{ get{return (anchorPart.partMass + connectedPart.partMass);} }

        //	public void Start()
        //	{

        //	}



        //	void FixedUpdate()
        //	{
        //		if(anchorPart.partType.strength > 0)
        //		{
        //               Vector3 force = joint.GetReactionForce(Time.deltaTime);
        //               if (force.magnitude > jointStrength)
        //			{
        //				Break();
        //				Level.Slowmo();
        //			}
        //               Debug.DrawLine(joint.anchor, force, Color.red,5f);
        //		}
        //	}

        //public void Break()
        //{
        //    joint.enabled = false;
        //}
        //public void Reset()
        //{
        //    foreach (HingeJoint2D joint in connections)
        //    {
        //        joint.enabled = true;
        //    }
        //}
        //}
    }
}