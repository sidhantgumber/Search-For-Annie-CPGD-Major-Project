using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Callbacks;

public class DialogueEditor : EditorWindow
{
    
    Dialogue selectedDialogue = null;
    [NonSerialized]   // an extra new node was being initialized everytime we created a new node, to avoid it in sab parameters ko non serializable banaana padha
    GUIStyle nodeStyle;
    [NonSerialized]  
    GUIStyle playerNodeStyle;
    [NonSerialized]
    DialogueNode draggingNode = null;
    [NonSerialized]
    Vector2 draggingOffset;
    [NonSerialized]
    DialogueNode creatingNode = null;
    [NonSerialized]
    DialogueNode deletingNode = null;
    [NonSerialized]
    DialogueNode linkingParentNode = null;
    Vector2 scrollPosition;
    [NonSerialized]
    bool draggingCanvas = false;
    [NonSerialized]
    Vector2 draggingCanvasOffset;

    const float canvasSize = 4000;
    const float backgroundSize = 50;

    // string customText = "Sidhant";

    [MenuItem("Window/Dialogue Editor")]
    public static void ShowEditorWindow()
    {
       // Debug.Log("Editor window visible");
        GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");  // boolean here is to make it a utility window, utility window cannot be docked, hence false because we want to use it multiple times and not once
    }

    //used onopenassetattribute to open the dialogue editor everytime we double click the dialogue editor
    [OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        // used this to make sure yeh neeche vaala code (dialogue editor khulna) tabhi execute ho jab dialogue vaale scriptable object pe click karein
        Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;   // idhar casting kari to filter out other type of objects   
        if (dialogue != null)
        {
            ShowEditorWindow();
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        
        Selection.selectionChanged += OnSelectionChanged;    // default unity event of the Selection class


        // yeh sab set kiya to make the node look like a node
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
        nodeStyle.normal.textColor = Color.white;
        nodeStyle.padding = new RectOffset(20, 20, 20, 20);
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        playerNodeStyle = new GUIStyle();
        playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        playerNodeStyle.normal.textColor = Color.white;
        playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
        playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
    }

    // made this method to show the items in the editor window according to the selected dialogue
    private void OnSelectionChanged()
    {
       // Debug.Log("Selection Changed");
        Dialogue newDialogue = Selection.activeObject as Dialogue;    // selection.activeobject selected object return karta hai par hame sirf dialogue chahiye to usse type cast kar diya
        if (newDialogue != null)
        {
            selectedDialogue = newDialogue;
            Repaint();    // used repaint to update the editor window on clicking another object
        }
    }

    private void OnGUI()
    {
        if (selectedDialogue == null)
        {
            EditorGUILayout.LabelField("No Dialogue Selected.");
        }
        else
        {
            ProcessEvents();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
           // Debug.Log(scrollPosition);
           // GUILayoutUtility.GetRect(4000, 4000);     // specified the size of the editor window  GetRect reserves space in the auto layout
            Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
            Texture2D backgroundTex = Resources.Load("background") as Texture2D;   // texture ko resources folder se load karaaya as uska kaam sirf editor mei hai to build file mei background texture ki zaroorat nahi hai
            Rect texCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);  // the last two parameters are to tile the background texture exactly to fit it inside 
            GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);  // used this function instead of draw texture to tile the texture instead of scaling it


            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
           
                DrawConnections(node);
            }
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawNode(node);
                
            }

            EditorGUILayout.EndScrollView();

            //used two different loops taaki curves aur nodes overlap na karein

            if (creatingNode != null)
            {
               
                selectedDialogue.CreateNode(creatingNode);
                creatingNode = null;

            }
            if(deletingNode != null)
            {
                
                selectedDialogue.DeleteNode(deletingNode);
                deletingNode = null;
            }
        }
    }


    // method to drag nodes
    private void ProcessEvents()
    {
        if (Event.current.type == EventType.MouseDown && draggingNode == null)
        {
            draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
            if (draggingNode != null)
            {
                draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;  // used this offset to make the node not snap at the exact mouse position
                Selection.activeObject = draggingNode;
            }
            else
            {
                draggingCanvas = true;
                draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                Selection.activeObject = selectedDialogue;
            }
        }

        // daba ke kheenchne ke liye
        else if (Event.current.type == EventType.MouseDrag && draggingNode != null)   //A UnityGUI event.   Events correspond to user input(key presses, mouse actions), or are UnityGUI layout or rendering events :Source Documentation
        {
            Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
            draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
            GUI.changed = true;
        }
        else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
        {
            scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
            GUI.changed = true;
        }
        else if (Event.current.type == EventType.MouseUp && draggingCanvas)
        {
            draggingCanvas = false;
        }
        //chhodhne ke liye
        else if (Event.current.type == EventType.MouseUp && draggingNode != null)
        {
            draggingNode = null;
        }
    }


    // method to draw bezier curves from parent node to child nodes
    private void DrawConnections(DialogueNode node)
    {
        Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y); // start position ke liye node ka right centre
        foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
        {
            Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);  // end point ke liye child node ka left most centre
            Vector3 controlPointOffset = endPosition - startPosition;  // offset use kiya curve ko tangent dene ke liye, end - start to give it's length relative to the distance between nodes
            controlPointOffset.y = 0;
            controlPointOffset.x *= 0.8f;  // thoda sa curve reduce kiya taaki dekhne mei better lage
            Handles.DrawBezier(
                startPosition, endPosition,
                startPosition + controlPointOffset,
                endPosition - controlPointOffset,    // curve ke control point mei offset pass kiya taaki actually curve bane
                Color.white, null, 4f);
        }
    }
    private DialogueNode GetNodeAtPoint(Vector2 point)
    {
        DialogueNode foundNode = null;    // used this to get all nodes under the mouse pointer
        foreach (DialogueNode node in selectedDialogue.GetAllNodes())
        {
            if (node.GetRect().Contains(point))  // returns true if the point is in the rect
            {
                foundNode = node;
            }
        }
        return foundNode;
    }
    private void DrawNode(DialogueNode node)
    {

        GUIStyle style = nodeStyle;
        if(node.IsPlayerSpeaking())
        {
            style = playerNodeStyle;
        }
        GUILayout.BeginArea(node.GetRect(), style);
        node.SetText(EditorGUILayout.TextField(node.GetText()));
    

        
        foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
        {
            EditorGUILayout.LabelField(childNode.GetText());
        }

        GUILayout.BeginHorizontal();
        // this will have to be called after the iteration over the nodes gets finished in OnGUI()
        if ( GUILayout.Button("+"))
        {
            creatingNode = node;
        }
        DrawLinkButtons(node);
        if (GUILayout.Button("-"))
        {
            deletingNode = node;
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }


    //will allow us to add and remove links to various nodes
    private void DrawLinkButtons(DialogueNode node)
    {
        if (linkingParentNode == null)
        {
            if (GUILayout.Button("link"))
            {
                linkingParentNode = node;
            }
        }
        else if (linkingParentNode == node)
        {
            if (GUILayout.Button("cancel"))
            {
                linkingParentNode = null;
            }
        }
        else if (linkingParentNode.GetChildren().Contains(node.name))
        {
            if (GUILayout.Button("unlink"))
            {
               
                linkingParentNode.RemoveChild(node.name);
                linkingParentNode = null;
            }
        }
        else
        {
            if (GUILayout.Button("child"))
            {
             
                linkingParentNode.AddChild(node.name);
                linkingParentNode = null;
            }
        }
    }

}
