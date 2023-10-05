public class DoublyNode
{
    //holds data for the next node
    public DoublyNode nextNode { get; set; }

    //holds data for the previous node
    public DoublyNode previousNode { get; set; }
    
    //holds data for if the current room has been beaten
    public bool isRoomBeaten { get; set; }

    //holds data for is the player has won the game
    public bool isWinner { get; set; }
    

    //constructor for a doublynode
    public DoublyNode()
    {

    }
}


/*

    list<string> EnemyNames

    number of defeated enemies

    list of items

    list of picked up items

*/