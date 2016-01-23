using UnityEngine;

namespace Bridger
{
    public class RopePart : BridgePart
    {
        const float PIECES_PER_GRIDSIZE = 1f;
        public GameObject ropePiece;

        float pieceLength = 0.5f;
        int pieces = 1;

        public HingeJoint2D[] ropeConnections;

        public override bool EndStretch()
        {
            if (base.EndStretch())
            {
                pieces = Mathf.CeilToInt(PIECES_PER_GRIDSIZE * partLength / Grid.gridSize);
                pieceLength = partLength / pieces;

                foreach(Transform t in transform)
                {
                    Destroy(t.gameObject);
                }
                Rigidbody2D joint = originConnection.anchor.rigid;
                for (int i = 0; i < pieces; i++)
                {
                    GameObject piece = Instantiate<GameObject>(ropePiece);
                    piece.transform.localScale = new Vector3(pieceLength, 1, 1);
                    piece.transform.parent = transform;
                    piece.transform.position = Vector3.Lerp(partOrigin + partDirection * pieceLength * 0.5f, partEnd - partDirection * pieceLength * 0.5f, (float)i / pieces);
                    piece.transform.rotation = transform.rotation;
                    rigid.mass = 0;

                    joint = piece.GetComponent<Rigidbody2D>();
                    joint.
                }

                return true;
            }
            return false;
        }
    }
}
