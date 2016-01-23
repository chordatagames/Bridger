using UnityEngine;

namespace Bridger
{
    public class RopePart : BridgePart
    {
        public new void EndStretch()
        {
            base.EndStretch();

        }
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			//Update the visual representation
		}

    }
}
