using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;

namespace TerrainGeneration {
    public class TerrainBlock : MonoBehaviour {
        [SerializeField] public float weight = 1f;
        [SerializeField] public SplineContainer groundSpline;

        public Vector2 StartPosition {
            get {
                float3 pos = groundSpline.Spline[0].Position;
                return new Vector2(pos.x, pos.y);
            }
        }

        public Vector2 EndPosition {
            get {
                var spline = groundSpline.Spline;
                float3 pos = spline[^1].Position;
                return new Vector2(pos.x, pos.y);
            }
        }

        public float Width => Mathf.Abs(EndPosition.x - StartPosition.x);
        public Vector2 Size => StartPosition - EndPosition;
        
    }
}