using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
public class Dialogue : ScriptableObject, ISerializationCallbackReceiver      // Interface to receive callbacks upon serialization and deserialization.
{
    [SerializeField]
    List<DialogueNode> nodes = new List<DialogueNode>();
    [SerializeField]
    Vector2 newNodeOffset = new Vector2(250, 0);

    Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();  // made this to search the dialogue node by its string ID


    private void Awake()    // will be called the first time dialogue scriptable object is selected
    {
      
        OnValidate();
    }
 #if UNITY_EDITOR
    public void CreateNode(DialogueNode parent)
    {
        DialogueNode newNode = MakeNode(parent);
        Undo.RegisterCreatedObjectUndo(newNode, "created new dialogue node");
        Undo.RecordObject(this, "Added Dialogue Node");  // jab bhi scriptable object ka data modify hoga to undo stack ko update karna padhega
        AddNode(newNode);

    }



    private void AddNode(DialogueNode newNode)
    {
        nodes.Add(newNode);
        OnValidate();
    }

    private  DialogueNode MakeNode(DialogueNode parent)
    {
        DialogueNode newNode = CreateInstance<DialogueNode>();
        newNode.name = Guid.NewGuid().ToString();

        if (parent != null)     // used this to protect when the root node is being created with no parents. Root node is also called bruce wayne node ehehehe
        {
            parent.AddChild(newNode.name);
            newNode.SetPlayerSpeaking(!parent.IsPlayerSpeaking());
            newNode.SetPosition(parent.GetRect().position + newNodeOffset);
        }

        return newNode;
    }

    public void DeleteNode(DialogueNode nodeToDelete)
    {
        Undo.RecordObject(this, "Deleted Dialogue Node");
        nodes.Remove(nodeToDelete);

        OnValidate();
        RemoveChildNodes(nodeToDelete);
        Undo.DestroyObjectImmediate(nodeToDelete);
    }


    // used this method to delete all children nodes of the deleted node
    private void RemoveChildNodes(DialogueNode nodeToDelete)
    {
        foreach (DialogueNode node in GetAllNodes())
        {
            node.RemoveChild(nodeToDelete.name);
        }
    }
#endif

    // onvalidate is called everytime a value is changed in the scriptable object
    private void OnValidate()
    {
        
        nodeLookup.Clear();
        foreach (DialogueNode node in GetAllNodes())   
        {
            nodeLookup[node.name] = node;  
        }
    }
    public IEnumerable<DialogueNode> GetAllNodes()   // used IEnumerabnle as it includes all data structures over which you can iterate using a foreach loop
    {
        return nodes;
    }
    public DialogueNode GetRootNode()
    {
        return nodes[0];
    }
    public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
    {
        foreach (string childID in parentNode.GetChildren())
        {
            if (nodeLookup.ContainsKey(childID))  // used this check to remove key not found exception, matlab child tabhi dhoondo jab child ho. Also this is probably the weirdest thing I've typed this week
            {
                yield return nodeLookup[childID]; //ienumerable use kiya tha to directly yield return kara diye child nodes instead of making a new list and storing those child nodes in that list
            }
        }
    }
    public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
    {
        foreach(DialogueNode node in GetAllChildren(currentNode))
        {
            if(!node.IsPlayerSpeaking())
            {
                yield return node;
            }
        }
    }

    public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
    {
        foreach (DialogueNode node in GetAllChildren(currentNode))
        {
            if (node.IsPlayerSpeaking())
            {
                yield return node;
            }
        }
    }



    // used this interface to save the assets to the asset database before serialising them
    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (nodes.Count == 0)
        {
            DialogueNode newNode = MakeNode(null);         
            AddNode(newNode);
        }
        if ( AssetDatabase.GetAssetPath(this) != "")
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                if(AssetDatabase.GetAssetPath(node) == "")
                {
                    AssetDatabase.AddObjectToAsset(node, this); // used this to make sure the nodes get added to the parent object and get saved 
                }
            }
        }
#endif
    }

    public void OnAfterDeserialize()
    {
       
    }
}
