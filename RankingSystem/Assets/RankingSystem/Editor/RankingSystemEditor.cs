using UnityEngine;
using UnityEditor;
using RankingSystem;
using SocketIO;
using UnityEngine.UI;

[CustomEditor(typeof(RankingSystemController))]
public class RankingSystemEditor : Editor
{

    private SerializedProperty _url;
    private SerializedProperty _font;
    private SerializedProperty _primaryUsersColor;
    private SerializedProperty _primaryColor;
    private SerializedProperty _secondaryUsersColor;
    private SerializedProperty _secondaryColor;
    private SerializedProperty _requestAllPlayers;
    private SerializedProperty _wantedNumberOfPlayers;
    private SerializedProperty _floatPrecision;
    private SerializedProperty _systemStyle;
    private SerializedProperty _targetCanvas;
    private SerializedProperty _rankingSprites;
    private SerializedProperty  _displayAmountPlayersPerPage;
    private SerializedProperty _numberOfPlayersPerPage;

    private bool _firstStart;
    private Color _textColor;

    private void OnEnable()
    {
        _url = serializedObject.FindProperty("websocketServerURL");
        _font = serializedObject.FindProperty("fontToUse");
        _primaryUsersColor = serializedObject.FindProperty("primaryColorUsers");
        _primaryColor = serializedObject.FindProperty("primaryColor");
        _secondaryUsersColor = serializedObject.FindProperty("secondaryColorUsers");
        _secondaryColor = serializedObject.FindProperty("secondaryColor");
        _requestAllPlayers = serializedObject.FindProperty("requestAllPlayers");
        _wantedNumberOfPlayers = serializedObject.FindProperty("wantedNumberOfPlayers");
        _floatPrecision = serializedObject.FindProperty("floatPrecision");
        _systemStyle = serializedObject.FindProperty("systemStyle");
        _targetCanvas = serializedObject.FindProperty("targetCanvas");
        _rankingSprites = serializedObject.FindProperty("rankingSprites");
        _displayAmountPlayersPerPage = serializedObject.FindProperty("displayAmountPlayersPerPage");
        _numberOfPlayersPerPage = serializedObject.FindProperty("numberOfPlayersPerPage");

        _firstStart = true;
        _textColor = !EditorGUIUtility.isProSkin ? new Color32(14, 14, 14, 255) : new Color32(194, 194, 194, 255);
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        RankingSystemController systemController = (RankingSystemController)target;
        if (_firstStart && systemController.targetCanvas != null)
        {
            systemController.ChangeFont();
            _firstStart = false;
        }

        systemController.GetComponent<NetworkController>().hideFlags = HideFlags.HideInInspector;
        systemController.GetComponent<SocketIOComponent>().hideFlags = HideFlags.HideInInspector;

        EditorGUILayout.HelpBox("Properties in red are only updated by fetching a new list", MessageType.Info);

        EditorGUILayout.LabelField("Server");
        DrawUILine(Color.white, 1, 5);
        EditorGUILayout.PropertyField(_url, new GUIContent("Websocket URL"));
        EditorGUILayout.HelpBox("Last event status : " + systemController.lastEventStatus, MessageType.None);

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Ranking system display");
        DrawUILine(Color.white, 1, 5);
        EditorGUILayout.PropertyField(_targetCanvas, new GUIContent("Targeted parent", "The transform which contains all the ranking system element (use entire canvas if you want font consistency)"));

        if (systemController.targetCanvas == null)
        {
            EditorGUILayout.HelpBox("A target canvas must be assigned", MessageType.Warning);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        EditorStyles.label.normal.textColor = Color.red;
        EditorGUILayout.PropertyField(_requestAllPlayers, new GUIContent("Request all players", "True to request all players from database, false if you only wants the X first"));
        if (!systemController.requestAllPlayers)
        {
            EditorGUILayout.PropertyField(_wantedNumberOfPlayers, new GUIContent("Number of wanted players", "If the specified int is less or equel to 0, or if it is higher than the database's current list of player, then returns the entire array"));
        }
        EditorStyles.label.normal.textColor = _textColor;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_displayAmountPlayersPerPage, new GUIContent("Display players in pages", "True to only display a specified amount of players per page"));
        if (systemController.displayAmountPlayersPerPage)
        {
               EditorGUILayout.PropertyField(_numberOfPlayersPerPage, new GUIContent("Number of players per page"));
        }
        if( EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            systemController.OnUpdatedList();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_floatPrecision, new GUIContent("Float precision", "Number of digit after the comma"));
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            systemController.OnFloatPrecisionChange();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_systemStyle, new GUIContent("Increasing or decreasing score"));
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            systemController.OnRankingStyleChange();
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Ranking system style");
        DrawUILine(Color.white, 1, 5);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_font, new GUIContent("Font", "Font that will be used throughout the whole targeted parent."));
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            systemController.ChangeFont();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_primaryUsersColor, new GUIContent("Primary color users"));
        EditorGUILayout.PropertyField(_primaryColor, new GUIContent(""), GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_secondaryUsersColor, new GUIContent("Secondary color users"));
        EditorGUILayout.PropertyField(_secondaryColor, new GUIContent(""), GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();

        // If the colors have changed then update all colors
        if (EditorGUI.EndChangeCheck())
        {
            foreach (Image image in systemController.primaryColorUsers)
            {
                if (image != null)
                    image.color = systemController.primaryColor;
            }

            foreach (Image image in systemController.secondaryColorUsers)
            {
                if (image != null)
                    image.color = systemController.secondaryColor;
            }
        }

        EditorGUILayout.PropertyField(_rankingSprites, new GUIContent("Ranking sprites", "(optional) Sprites to display instead of numbers. Put them from first to last order"));
        serializedObject.ApplyModifiedProperties();
    }

    private static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}