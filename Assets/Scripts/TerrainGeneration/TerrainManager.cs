using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;
using Spline = UnityEngine.Splines.Spline;

namespace TerrainGeneration {
    public class TerrainManager : MonoBehaviour {
        [SerializeField] private float extraGenerationDistance = 20f;
        [SerializeField] private TerrainSettings settings;
        [SerializeField] private SplineContainer splineContainerBase;
        [SerializeField] private SpriteShapeController spriteShapeController;
        [SerializeField] private float heightBelow = 100f;
        [SerializeField] private float splineTension = 0.33f;
        [SerializeField] private Transform terrainParent;

        private Vector2 _lastPosition = Vector2.zero;
        public static event Action<Vector2> OnTerrainGenerationComplete;

        private void OnEnable() {
            GameManager.OnGameStart += HandleGameStart;
        }

        private void OnDisable() {
            GameManager.OnGameStart -= HandleGameStart;
        }

        private void HandleGameStart(RaceInfo raceInfo) {
            GenerateChunks(raceInfo.RaceDistance);
        }

private void GenerateChunks(float width) {
            float generationProgress = 0f;
            bool baseWidthMet = false;

            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(0, -heightBelow));
            
            // Note: If you ever change where the terrain starts, you'll want this 
            // to be _lastPosition instead of a hardcoded Vector2(0,0)
            points.Add(new Vector2(0, 0)); 
            
            while (generationProgress < width + extraGenerationDistance) {
                TerrainBlock currentBlockPrefab = ChooseRandomBlock();
                GameObject newBlock = Instantiate(currentBlockPrefab.gameObject, Vector3.zero, Quaternion.identity, terrainParent);
                TerrainBlock currentBlock = newBlock.GetComponent<TerrainBlock>();
                
                Vector2 newBlockOffset = currentBlock.StartPosition;
                newBlock.transform.position = _lastPosition - newBlockOffset; 
                
                points.AddRange(GetTerrainBlockSplinePoints(currentBlock));

                _lastPosition = (Vector2)newBlock.transform.position + currentBlock.EndPosition;
                
                generationProgress += currentBlock.Width;
                Debug.DrawLine(newBlock.transform.position, currentBlock.transform.position, Color.red);

                if (!baseWidthMet && generationProgress > width) {
                    baseWidthMet = true;
                    OnTerrainGenerationComplete?.Invoke(_lastPosition);
                }
            }
            
            points.Add(new Vector2(_lastPosition.x, _lastPosition.y - heightBelow));
            
            splineContainerBase.Spline.Clear();
            splineContainerBase.Spline.AddRange(PointsToKnots(points));
            splineContainerBase.Spline.SetTangentMode(new SplineRange(0, points.Count), TangentMode.AutoSmooth);
            
            UpdateSpline(spriteShapeController, splineContainerBase.Spline);
        }
        private IEnumerable<float3> PointsToKnots(List<Vector2> points) {
            return points.Select(p => p.ToFloat3());
        }

        private void UpdateSpline(SpriteShapeController controller, Spline spline) {
            controller.spline.Clear();
            int count = spline.Count;

            for (int i = 0; i < count; i++) {
                controller.spline.InsertPointAt(i, spline[i].Position.ToVector3());
            }

            for (int i = 1; i < count - 1; i++) {
                controller.spline.SetTangentMode(i, ShapeTangentMode.Continuous);

                Vector3 pPrev = spline[i - 1].Position;
                Vector3 pNext = spline[i + 1].Position;
        
                Vector3 direction = (pNext - pPrev).normalized;
                
                float distToPrev = Vector3.Distance(spline[i].Position, pPrev);
                float distToNext = Vector3.Distance(spline[i].Position, pNext);
                
                controller.spline.SetLeftTangent(i, -direction * distToPrev * splineTension);
                controller.spline.SetRightTangent(i, direction * distToNext * splineTension);
            }

            controller.UpdateSpriteShapeParameters();
        }
        
        private IEnumerable<Vector2> GetTerrainBlockSplinePoints(TerrainBlock block) {
            Spline spline = block.groundSpline.Spline;
            Vector2 pos = block.transform.position;
            for (int i = 1; i < spline.Count; i++) {
                yield return new Vector2(spline[i].Position.x + pos.x, spline[i].Position.y + pos.y);
            }
        }

        private TerrainBlock ChooseRandomBlock() {
            return ExtraRandom.WeightedChoice(settings.blocks, x => x.weight);
        }
    }
}