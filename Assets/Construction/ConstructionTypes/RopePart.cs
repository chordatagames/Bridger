using UnityEngine;

namespace Bridger
{
    public class RopePart : BridgePart
    {
        float maxLength = 1f;

        public new void EndStretch()
        {
            base.EndStretch();
            maxLength = partLength;
        }

        void FixedUpdate() //I dont know how to do this stuff I give up
        {
            if (!editing)
            {
                if (partLength < maxLength)
                {

                    Vector3 direction = (partEnd - partOrigin).normalized;
                    Stretch(Vector3.down * Mathf.Abs(direction.y));
                }
            }
        }
    }
}
