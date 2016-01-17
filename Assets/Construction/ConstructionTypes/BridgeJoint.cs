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
        public List<HingeJoint2D> connections = new List<HingeJoint2D>();

        /// <summary>
        /// This adds a new hinge joint to the <paramref name="anchor"/> 
        /// and connects it to the <paramref name="connectedBody"/> through the <paramref name="position"/>
        /// </summary>
        /// <param name="anchor"></param>
        /// <param name="connectedBody"></param>
        /// <param name="position"></param>
        public void ConnectPart(BridgePart anchor, BridgePart connectedBody, Vector2 position)
        {
            this.anchor = anchor;
            HingeJoint2D joint = anchor.gameObject.AddComponent<HingeJoint2D>();
            joint.enableCollision = false;
            joint.connectedBody = connectedBody.rigid;
            joint.connectedAnchor = Grid.ToGrid(connectedBody.transform.InverseTransformPoint(position));
            joint.anchor = Grid.ToGrid(anchor.transform.InverseTransformPoint(position));

            connections.Add(joint);

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