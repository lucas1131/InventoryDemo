using System;
using UnityEditor;
using UnityEngine;

// This file should be in an Editor only assembly
namespace InventoryDemo.ResourceSystem.Editor
{
    [CustomEditor(typeof(ReplaceTerrainTreesWithRealTrees))]
    public class ReplaceTerrainTreesWithRealTreesEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Replace terrain trees with object trees"))
            {
                Replace((ReplaceTerrainTreesWithRealTrees)target);
            }
        }

        private void Replace(ReplaceTerrainTreesWithRealTrees owner)
        {
            Terrain terrain = Terrain.activeTerrain;
            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPos = terrain.transform.position;

            foreach (TreeInstance treeInstance in terrainData.treeInstances)
            {
                Vector3 worldPos = Vector3.Scale(treeInstance.position, terrainData.size) + terrainPos;
                Quaternion rotation = Quaternion.Euler(0, treeInstance.rotation * 360f, 0);
                GameObject treeObject = Instantiate(owner.TreePrefab, worldPos, rotation);

                treeObject.transform.localScale = Vector3.one * treeInstance.widthScale;
                treeObject.transform.parent = terrain.transform;
            }
            
            terrainData.treeInstances = Array.Empty<TreeInstance>();
        }
    }
}