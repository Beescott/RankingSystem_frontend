using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace RankingSystem
{
    public class RankingSystemPreviewWindow : EditorWindow
    {

        [MenuItem("RankingSystem/RankingSystemPreviewWindow")]
        private static void ShowWindow()
        {
            var window = GetWindow<RankingSystemPreviewWindow>();
            window.titleContent = new GUIContent("RankingSystemPreviewWindow");
            window.Show();
        }

        public RankingSystemController rankingSystemController;

        private void OnGUI()
        {
            SerializedObject obj = new SerializedObject(this);

            obj.Update();
            EditorGUILayout.PropertyField(obj.FindProperty("rankingSystemController"));

            if (rankingSystemController == null)
            {
                EditorGUILayout.HelpBox("A ranking system controller must be selected", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                DrawProperties();
                DrawPreview();
                EditorGUILayout.EndHorizontal();
            }

            obj.ApplyModifiedProperties();
        }

        private void DrawProperties()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(GUILayout.Width(200));
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Primary color:");
            rankingSystemController.primaryColor = EditorGUILayout.ColorField(rankingSystemController.primaryColor, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Secondary color:");
            rankingSystemController.secondaryColor = EditorGUILayout.ColorField(rankingSystemController.secondaryColor, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                foreach (Image image in rankingSystemController.primaryColorUsers)
                {
                    if (image != null)
                        image.color = rankingSystemController.primaryColor;
                }

                foreach (Image image in rankingSystemController.secondaryColorUsers)
                {
                    if (image != null)
                        image.color = rankingSystemController.secondaryColor;
                }
                EditorUtility.SetDirty(rankingSystemController);
            }
        }

        private void DrawPreview()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginVertical();
            GUIStyle titleGS = GUI.skin.GetStyle("Label");
            titleGS.alignment = TextAnchor.UpperCenter;
            GUILayout.Label("Ranks", titleGS);
            EditorGUILayout.EndVertical();

            titleGS.alignment = TextAnchor.UpperLeft;

            // 5 displayed players by default
            EditorGUILayout.BeginVertical();
            int numberOfPlayersToDisplay = rankingSystemController.displayAmountPlayersPerPage ? rankingSystemController.numberOfPlayersPerPage : 5;
            GUIStyle gs = new GUIStyle();

            for (int i = 0; i < numberOfPlayersToDisplay; i++)
            {
                Color colorToUse = i % 2 == 0 ? rankingSystemController.primaryColor : rankingSystemController.secondaryColor;
                gs.normal.background = MakeTex(600, 1, colorToUse);
                GUILayout.BeginHorizontal(gs, GUILayout.Height(30));
                GUILayout.Label(i.ToString());

                titleGS.alignment = TextAnchor.UpperCenter;
                GUILayout.Label("player name");

                titleGS.alignment = TextAnchor.UpperRight;
                GUILayout.Label("player score");
                titleGS.alignment = TextAnchor.UpperLeft;
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

    }

}