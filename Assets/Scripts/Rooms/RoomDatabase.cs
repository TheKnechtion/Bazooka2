//It's just data? There's no using? That's kinda nutty.


//this contains the room database linked list
using System;
using System.Collections.Generic;

public class RoomDatabase
{
    //reference the head node of the linked list.
    //public so that it can be used as an entry
    //point to the list for the game manager.

    //public DoublyNode headNode; //Is a room
    //public List<DoublyNode> roomList;





    public List<Room> Room_Database { get; set; }



    public RoomDatabase()
    {
        Room_Database = new List<Room>();


        Room r000 = new Room();
        r000.sceneName = "Scenes/TestingArea/Test_BatteryPuzzle";
        r000.objetiveType = RoomType.ReturnPower;
        Room_Database.Add(r000);

    }


    /*
    public RoomDatabase()
    { 
        roomList = new List<DoublyNode>();
    }
  v   */


    /*
    //creates the room database
    public void CreateLinkedList()
    {
        //creates the node that holds data from the first room
        headNode = new DoublyNode();
        headNode.previousNode = null;


        //creates the node that holds data from the second room
        DoublyNode roomTwo = new DoublyNode();
        headNode.nextNode = roomTwo;
        roomTwo.previousNode = headNode;

        //creates the node that holds data from the final room
        DoublyNode roomThree = new DoublyNode();
        roomTwo.nextNode = roomThree;
        roomThree.previousNode = roomTwo;

        DoublyNode roomFour = new DoublyNode();
        roomThree.nextNode = roomFour;
        roomFour.previousNode = roomThree;
        roomFour.nextNode = null;
    */

        /*
        //We add these to a list of DoublyNodes to iterate through it 
        //for data checking
        roomList.Add(headNode);
        roomList.Add(middleNode);
        roomList.Add(tailNode);
        */
    



}


//toying around with a doubly linked list created via a loop
//created to help understand how a linked list works, might be
//helpful some day, but didn't seem necessary for this current
//application.
/*
for (int i = 0; i < 5;)
{
    private DoublyNode previousNode;
    private DoublyNode nextNode;

    //private DoublyNode currentNode_1 = null;
    //private DoublyNode currentNode_2 = null;

    if (currentNode_1.previousNode == null)
    {
        DoublyNode currentNode_1 = new DoublyNode(i++);
        headNode.nextNode = currentNode_1;
        currentNode_1.previousNode = headNode;
    }
    else
    {
        DoublyNode currentNode_2 = new DoublyNode(i++);
        currentNode_2.previousNode = currentNode_1;
        currentNode_1.nextNode = currentNode_2;
        currentNode_1 = currentNode_2;
    }
}
*/