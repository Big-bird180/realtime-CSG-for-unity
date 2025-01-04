﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using InternalRealtimeCSG;

namespace RealtimeCSG
{
	internal sealed partial class CSGModelComponentInspectorGUI
	{
		private static readonly GUIContent ExportLabel							= new GUIContent("Export");
		private static readonly GUIContent ExportOriginLabel					= new GUIContent("Origin");
		private static readonly GUIContent ExportColliderLabel                  = new GUIContent("Export Colliders");
		private static readonly GUIContent ExportToButtonLabel                  = new GUIContent("Export to ...");

		private static readonly GUIContent GenerateColliderContent				= new GUIContent("Generate Collider");
//		private static readonly GUIContent AutoRebuildCollidersContent          = new GUIContent("Auto Rebuild Colliders", "Automatically regenerate colliders when the model has been modified. This may introduce hitches when modifying geometry.");
		private static readonly GUIContent ModelIsTriggerContent				= new GUIContent("Model Is Trigger");
		private static readonly GUIContent ColliderSetToConvexContent			= new GUIContent("Convex Collider", "Set generated collider to convex");
		private static readonly GUIContent ColliderAutoRigidBodyContent			= new GUIContent("Auto RigidBody", "When enabled the model automatically updates the Rigidbody settings, creates it when needed, destroys it when not needed.");
		private static readonly GUIContent DefaultPhysicsMaterialContent		= new GUIContent("Default Physics Material");
		private static readonly GUIContent InvertedWorldContent					= new GUIContent("Inverted world", "World is solid by default when checked, otherwise default is empty");
		private static readonly GUIContent DoNotRenderContent					= new GUIContent("Do Not Render");
#if UNITY_2019_2_OR_NEWER
        private static readonly GUIContent ReceiveGIContent					    = new GUIContent("Receive Global Illumination", "If enabled, this GameObject receives global illumination from lightmaps or Light Probes. To use lightmaps, Contribute Global Illumination must be enabled.");
#endif
        private static readonly GUIContent TwoSidedShadowsContent				= new GUIContent("Two Sided Shadows", "Makes all rendered surfaces two sided when rendering shadows. Note this only works when the surface is visible (Unity limitation).");
		
		private static readonly GUIContent AutoRebuildUVsContent				= new GUIContent("Auto Rebuild UVs", "Automatically regenerate lightmap UVs when the model has been modified. This may introduce hitches when modifying geometry.");
		private static readonly GUIContent PreserveUVsContent                   = new GUIContent("Preserve Uvs", "Specifies whether the authored mesh UVs get optimized for Realtime Global Illumination or will be reserved. When disabled, the authored UVs can get merged, scaled, and packed for optimisation purposes. When enabled, the authored UVs will get scaled and packed, but not merged.");

		private static readonly GUIContent ScaleInLightmapContent               = new GUIContent("Scale In Lightmap", "Scale how the meshes generated by the model use space in a generated lightmap.");
#if UNITY_2017_2_OR_NEWER
		private static readonly GUIContent StitchLightmapSeamsContent			= new GUIContent("Stitch seams", "When enabled, seames in baked lightmap will get smoothed. Requires progressive lightmapper");
#endif
		private static readonly GUIContent ShowGeneratedMeshesContent			= new GUIContent("Show Meshes", "Select to show the generated Meshes in the hierarchy");
#if UNITY_2017_3_OR_NEWER
		private static readonly GUIContent MeshColliderCookingContent			= new GUIContent("Collider Cooking", "Determines how optimized the mesh collider will be");
		private static readonly GUIContent CookForFasterSimulationContent		= new GUIContent("Optimize", "Toggle on for faster simulation or off for faster cooking time.");
		private static readonly GUIContent EnableMeshCleaningContent			= new GUIContent("Clean Mesh", "Toggle on to clean the mesh.");
		private static readonly GUIContent WeldColocatedVerticesContent			= new GUIContent("Weld Vertices", "Toggle the removal of equal vertices.");
#endif
		
		private static readonly GUIContent AngleErrorContent					= new GUIContent("Angle Error", "Maximum allowed angle distortion (0..1)");
		private static readonly GUIContent AreaErrorContent						= new GUIContent("Area Error", "Maximum allowed area distortion (0..1)");
		private static readonly GUIContent HardAngleContent						= new GUIContent("Hard Angle", "This angle (in degrees) or greater between triangles will cause seam to be created");
		private static readonly GUIContent PackMarginContent					= new GUIContent("Pack Margin", "How much uv-islands will be padded");

//		private static readonly GUIContent VertexChannelColorContent			= new GUIContent("Color channel");
		private static readonly GUIContent VertexChannelTangentContent			= new GUIContent("Tangent channel");
		private static readonly GUIContent VertexChannelNormalContent			= new GUIContent("Normal channel");
		private static readonly GUIContent VertexChannelUV1Content				= new GUIContent("UV1 channel");

		
		private static readonly GUIContent EnableLightmapsForAllContent			= new GUIContent("Enable lightmaps for all models");
		private static readonly GUIContent EnableLightmapsContent				= new GUIContent("Enable lightmaps");
		private static readonly GUIContent DisableLightmapsContent				= new GUIContent("Disable lightmaps");
		
		private static readonly GUIContent ResetContent							= new GUIContent("Reset to default");
		
		private static readonly GUIContent IgnoreNormalsContent					= new GUIContent("Ignore Normals","When enabled, prevents the UV charts from being split during the precompute process for Realtime Global Illumination lighting.");
		private static readonly GUIContent AutoUVMaxDistanceContent				= new GUIContent("Max Distance","Specifies the maximum worldspace distance to be used for UV chart simplification. If charts are within this distance they will be simplified for optimization purposes.");
		private static readonly GUIContent AutoUVMaxAngleContent				= new GUIContent("Max Angle","Specifies the maximum angle in degrees between faces sharing a UV edge. If the angle between the faces is below this value, the UV charts will be simplified.");

		private static readonly GUIContent MinimumChartSizeContent				= new GUIContent("Min Chart Size", "Specifies the minimum texel size used for a UV chart. If stitching is required, a value of 4 will create a chart of 4x4 texels to store lighting and directionality. If stitching is not required, a value of 2 will reduce the texel density and provide better lighting build times and run time performance.");

		private static readonly GUIContent StitchCracksContent					= new GUIContent("Stitch Cracks", "Some imprecise CSG operations may leave thin gaps and holes, this feature fills in those holes. Increases the amount of triangles.");

		public static int[] MinimumChartSizeValues = { 2, 4 };
		public static GUIContent[] MinimumChartSizeStrings =
		{
			new GUIContent("2 (Minimum)"),
			new GUIContent("4 (Stitchable)"),
		};
	}
}
 