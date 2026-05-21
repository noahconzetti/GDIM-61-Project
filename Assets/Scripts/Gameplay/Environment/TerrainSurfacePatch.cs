using UnityEngine;

namespace Gameplay.Environment {
    public class TerrainSurfacePatch : MonoBehaviour {
        [SerializeField] private AnimationCurve depthCurve;
        [SerializeField] private int samples = 10;
        [SerializeField] private float heightAbove = 5f;
        [SerializeField] private float raycastDistance = 10f;
        [SerializeField] private LayerMask groundMask;
 
        public void Generate(Vector2 areaLeft, Vector2 areaRight, bool slow) {
            Vector3[] topVerts = new Vector3[samples];
            Vector3[] bottomVerts = new Vector3[samples];

            float spacing = (areaRight.x - areaLeft.x) / (samples - 1);

            for (int i = 0; i < samples; i++) {
                float currentX = areaLeft.x + spacing * i;
                float percentX = i / (float)(samples - 1);
                float startY = areaLeft.y + heightAbove;
                Vector2 startPos = new(currentX, startY);
                RaycastHit2D raycastHit = Physics2D.Raycast(startPos, Vector2.down, raycastDistance, groundMask);
                
                if (!raycastHit) {
                    Debug.LogWarning("Raycast from " + startPos + " did not hit the ground :(");
                    continue;
                }

                topVerts[i] = raycastHit.point;
                bottomVerts[i] = raycastHit.point + new Vector2(0, depthCurve.Evaluate(percentX));
                Transform t = new GameObject("vert").transform;
                t.position = topVerts[i];
                new GameObject("start").transform.position = startPos;

            }

            GenerateMesh(topVerts, bottomVerts);
            if (slow) {
            }
        }

        private void GenerateMesh(Vector3[] top, Vector3[] bottom) {
            
        }
    }
}