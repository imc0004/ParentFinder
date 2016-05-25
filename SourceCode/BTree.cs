/*
 * Author: Ina Carter
 * Programming Challenge
 * BTree: finding common parent with ASCII Display
 * Btree Class and Node Class
 * This class just defines the node structure as well as the Binary Tree Structure
 * There are also functions for inserting nodes into the tree, and constructing the Binary tree
 * 5/25/2016
 */

using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTraversal
{

    /// <summary>
    /// NODE CLASS : Defines the structure of a node. 
    /// Node has two node (Left and Right node) attributes designating the two possible children a node has
    /// an integer attribute to hold the node's data,
    /// an integer to hold the key value,
    /// </summary>
    public class Node
    {
        public Node Left;  //left child
        public Node Right; //right child
        public int key;   //node key
        public int data;    //data in the node
        public string color; //color of node
    }

    /// <summary>
    /// BTREE CLASS : Defines the stucture of an unsorted Binary Tree
    /// It contains a Node root to hold the root of the tree. 
    /// As well as functions to enable the construction of the tree.
    /// </summary>
    public class BTree
    {
        public Node root = null; //root of the tree

        /// <summary>
        /// insertNode inserts another node into the tree.
        /// It takes an integer value which corresponds to the data value to be inserted into the node,
        /// and an integer key which corresponds to the key to identify the node by.
        /// On initialization the new node's left an right children are set to null and the data is set to the value passed in.
        /// Function returns a Node. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns"Node"></returns>
        public Node insertNode(int key, int value)
        {
            Node child = new Node();
            child.Left = null;
            child.Right = null;
            child.key = key; 
            child.data = value;
            return child;
        }

        /// <summary>
        /// constructTree constructs a binary tree in level-order
        /// It takes arguments Btree which is a binary tree, 
        /// int totalNodes specifying totaly number of nodes in the tree
        /// Queue temp which is originally an empty queue that will be used by the function,
        /// Queue nodes which holds the nodes that will go in the tree in the order in which they are to be added
        /// </summary>
        /// <param name="B"></param>
        /// <param name="totalNodes"></param>
        /// <param name="temp"></param>
        /// <param name="nodes"></param>
        public void constructTree(BTree B, int totalNodes,  Queue temp, Queue nodes)
        {
            Node current = null;//initialize temp node

            if (temp.Count == 0)
            {       
                B.root = B.insertNode(totalNodes - nodes.Count, (int)nodes.Dequeue());
                current = B.root;//first node is root of tree
                temp.Enqueue(B.root);//enqueue root into temp
            }
            
            //If there are still elements in the queue keep adding nodes
            if (temp.Count > 0 && nodes.Count > 0)
            {
                //take current off of the queue
                current = (Node)temp.Dequeue();             
                current.Left = B.insertNode(totalNodes - nodes.Count, (int)nodes.Dequeue()); //left child is next node in list
                temp.Enqueue(current.Left);  //add node to temp queue
                if (temp.Count > 0 && nodes.Count > 0)
                {
                    current.Right = B.insertNode(totalNodes - nodes.Count, (int)nodes.Dequeue()); //right child is next node in list
                    temp.Enqueue(current.Right);//add next node to temp queue
                    constructTree(B, totalNodes, temp, nodes);
                }                
            }//end outer if
        }//end construct Tree
    }//end BTree Class
}//end TreeTraversal namespace
