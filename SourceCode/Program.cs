/*
 * Author: Ina Carter
 * Programming Challenge
 * BTree: finding common parent with ASCII Display
 * The main program takes a file with a series of comma or space delimited numbers and pushes each onto a queue
 * This queue is then used to create a binary tree using level order tree traversal type algorithm. 
 * The closest common parent of these two nodes is found in that binary tree. 
 * The binary tree is then represented in the console using ascii characters and colors to denote the special nodes.
 * Btree Class and Node Class
 * 5/25/2016
 */

using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTraversal
{
    class Program
    {
        static void Main(string[] args)
        {
            bool loop = true; //while loop is on going
            
            //create a random queue
            Queue nodeQueue = new Queue();
            for(int i=0; i<17; i++)
            {
                nodeQueue.Enqueue(i);
            }

            BTree tree = new BTree();
            Queue temp = new Queue(); 
            tree.constructTree(tree, nodeQueue.Count+1, temp, nodeQueue);

            do{
                Console.Write("Type in an integer key for Node 1 and hit enter. (Pick between 1-15 for a readable graphical representation).\n");

                Node n01 = new Node();
                n01.key = Convert.ToInt32(Console.ReadLine());

                Console.Write("Type in an integer key for Node 2 and hit enter. (Pick between 1-15 for a readable graphical representation).\n");
                Node n02 = new Node();
                n02.key = Convert.ToInt32(Console.ReadLine());

                ParentFinder(tree, n01, n02);
                Console.WriteLine();
                Console.Write("If you wish to try another set of nodes then type 'yes'.");
                Console.WriteLine();
                    if(Console.ReadLine()=="yes")
                        loop = true;
                    else
                        loop=false;
                    Console.Clear();
            }while (loop); //wait for key to be pressed before closing console           
        }

        /// <summary>
        /// ParentFinder finds the closest common parent to two nodes passed in.
        /// It does so by comparing their keys (as binary numbers) from left to right
        /// until one doesn't match. 
        /// Example: 1011 and 100 share 10 in common, therefore 10 is the key of the parent. 
        /// Arguments needed are Btree B, and the two Nodes n1&n2. 
        /// A graphical representation of the binary tree will be displayed to a console.
        /// </summary>
        /// <param name="B"></param>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        static void ParentFinder(BTree B, Node n1, Node n2)
        {
            //convert to binary through the use of the convert.ToString
            var s1 = Convert.ToString(n1.key, 2);            
            var s2 = Convert.ToString(n2.key, 2);

            //figure out how many spaces you need to shift to make sure both s1 and s2 are of equal length
            int shift = s1.Length - s2.Length;
            //to ensure shift is positive
            if (shift < 0)
                shift = shift * -1;
            
            //convert back to integers
            int i1 = Convert.ToInt32(s1);
            int i2 = Convert.ToInt32(s2);
           
            //if there is a shift necessary
            if (shift > 0)
            {
                //bitshift the larger number to the right
                if(i1 < i2)
                    i2 = i2 / (int)System.Math.Pow(10, shift);
                else
                    i1 = i1 / (int)System.Math.Pow(10, shift);                
            }
            
            //while the two numbers are not the same, keep bitshifting them to the right
            //once they match you found the common ancestor they both share
            //Example: 1011 and 100 share 10 in common, therefore 10 is the key of the parent. 
            while (i1 != i2)
            {
                i1 = i1 / 10;
                i2 = i2 / 10;
            }

            //convert that number to a string
            string pString = Convert.ToString(i1, 10);
            //convert that string back to an integer as a base 10
            int parentKey = Convert.ToInt32(pString, 2);

            Queue emptyQueue= new Queue();
            Queue treeQueue = new Queue();

            //create a queue
            treeQueue = BFEnqueue(B.root, emptyQueue, treeQueue);
            //display the queue lighting up the correct nodes
            display(treeQueue, n1, n2, parentKey);
        }

        /// <summary>
        /// BFEnqueue makes a queue by going through the tree in Breadth First order
        /// and creating a queue. 
        /// Takes arguments Node node which is the root node of the tree.
        /// Queue temp, which is an empty queue to start. 
        /// Queue treeQ which will hold the queue version of the tree being created.  
        /// </summary>
        /// <param name="node"></param>
        /// <param name="temp"></param>
        /// <param name="treeQ"></param>
        /// <returns="treeQ"></returns>
        public static Queue BFEnqueue(Node node, Queue temp, Queue treeQ)
        {            
            if (treeQ.Count == 0)
                treeQ.Enqueue(node);//enqueue root into temp
           
            if(node.Left != null)
                temp.Enqueue(node.Left);
            if(node.Right != null) 
                temp.Enqueue(node.Right);
          
            treeQ.Enqueue(temp.Peek()); //add the next leading item in the queue
            
            Node next = ((Node)temp.Dequeue());

            if (temp.Count != 0)
                BFEnqueue(next, temp, treeQ);

            return treeQ;
        }//end BFEnqueue function

        /// <summary>
        /// Display prints the binary tree and colors the specified nodes passed in.
        /// It takes arguments Queue tree which it uses to print the tree, 
        /// Node node1, and Node node2 which specify which nodes were searched for,
        /// and int parentKey holding the int key value of the parent.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="parentKey"></param>
        public static void display(Queue tree, Node node1, Node node2, int parentKey)
        {
            int level = 0; //starting level
            int maxLevel = (int)System.Math.Log(tree.Count, 2); //last level
            int width = Console.WindowWidth/2; //starting width divide screen in two to center the first key
            int top = Console.WindowHeight/(maxLevel+1); //starting height
            //while there are still elements in the queue
            while(tree.Count != 0)
            {
                int items = (int)System.Math.Pow(2, level); //max number of possible items on this level
                
                //until there are no more items to print, or the tree queue is empty, continue printing line
                for (int itemNum = 1; itemNum<=items && tree.Count>0; itemNum++)
                {                 
                        if (((Node)tree.Peek()).key == parentKey) //if parent key paint yellow
                            System.Console.ForegroundColor = ConsoleColor.Yellow;
                        else if (((Node)tree.Peek()).key == node1.key) //if rightmost node paint red
                            System.Console.ForegroundColor = ConsoleColor.Red;
                        else if (((Node)tree.Peek()).key == node2.key) //if leftmost node paint green
                            System.Console.ForegroundColor = ConsoleColor.Green;
                        else
                            System.Console.ForegroundColor = ConsoleColor.White; //else paint white
                   
                     int x_alignment = (itemNum - 1) + itemNum; //current itemNum plus the previous itemNum
                     Console.SetCursorPosition(width * (x_alignment), (top + (level*2)));//set the cursor
                     
                     Console.Write(( (Node)tree.Dequeue()).key); //write the next node

                     System.Console.ForegroundColor = ConsoleColor.Gray;//change color to gray
                     if((itemNum-1)%2 ==0 && level>0) //if itemNum is an even number and not the first number then draw line from it
                         for (int i = width*3 - width*1 -2; i >= 0; i--) //i = distance between children
                             Console.Write("-");                    
                }//end for loop

                width = width/2; //continually divide the width in half
                level++;//next level
            }//end while loop
        }//end display function
       
    }//end Program
}//end namespace
