using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace Bridger
{
	[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
	public class BridgePart : MonoBehaviour, IReloadable, IRevertable
	{
		
		public void Reset()
		{
			throw new NotImplementedException();
		}

		public void StartPhysics()
		{
			throw new NotImplementedException();
		}
		public void Remove()
		{
			throw new NotImplementedException();
		}

		public bool Redo()
		{
			throw new NotImplementedException();
		}
		
		public bool Undo()
		{
			throw new NotImplementedException();
		}
	}
}
