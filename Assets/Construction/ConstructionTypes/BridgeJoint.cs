using UnityEngine;

namespace Bridger
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class BridgeJoint : MonoBehaviour
	{

		BridgePart _anchorPart;
		public BridgePart anchorPart
		{
			get
			{
				if(_anchorPart == null)
				{_anchorPart = GetComponent<BridgePart>();}
				return _anchorPart;
			}
		}
		BridgePart _connectedPart;
		public BridgePart connectedPart
		{
			get
			{
				if(_connectedPart == null)
				{_connectedPart = joint.connectedBody.GetComponent<BridgePart>();}
				return _connectedPart;
			}
		}

		HingeJoint2D _joint;
		public HingeJoint2D joint
		{
			get
			{
				if(_joint == null)
				{_joint = gameObject.AddComponent<HingeJoint2D>();}
				return _joint;
			}
		}

		public float jointStrength	{ get{return Mathf.Min(anchorPart.materialType.strength, connectedPart.materialType.strength);} }
		public float jointMass		{ get{return (anchorPart.partMass + connectedPart.partMass);} }

		public void Start()
		{

		}



		void FixedUpdate()
		{
			if(anchorPart.materialType.strength > 0)
			{

				if(joint.GetReactionForce(Time.deltaTime).magnitude > jointStrength)
				{
					Break();
				}
			}
		}
		
		public void ConnectPart(Rigidbody2D part, Vector2 position)
		{
			joint.enableCollision = false;
			joint.connectedBody = part;
			joint.connectedAnchor = Grid.ToGrid((Vector2)part.transform.InverseTransformPoint((Vector3)position));
			joint.anchor = (Vector2)transform.InverseTransformPoint(part.transform.TransformPoint((Vector3)joint.connectedAnchor));
		//	ConnectJoint(newJoint);
		//	ConnectConstruction(part);//Connection must be set up for both parts for undo/redo functionality
		}
		public void Break()
		{
			joint.enabled = false;
		}
		public void Reset()
		{
			joint.enabled = true;
		}
	}
}