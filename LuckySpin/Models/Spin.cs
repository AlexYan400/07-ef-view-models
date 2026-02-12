using System;
using System.Linq;
namespace LuckySpin.Models
{
    public class Spin
    {
        //Instance Variables
        Random random = new Random();
        private int[] numbers; //a spin array;

        //Constructor
        public Spin()
        {
            numbers = new int[] { random.Next(10), random.Next(10), random.Next(10) };
        }

        //Model Properties
        public int Id { get; set; }
        public decimal RunningBalance { get; set; }   
        public int[] Numbers //the spin numbers are set in the constructor
        { 
            //TODO: Uncomment the line below to Change the Model - adding the set accessor makes the DbContext take note of this as a column.
            //set { numbers = value; } 
            get { return numbers; }
        } 

        //Navigation properties
        public int GameId { get; set; } //Foreign Key to the Game who made this Spin
        public Game Game { get; set; } //Navigation property to the Game that contains this Spin
     
        //Spin Method   
        public bool isWinning(Player player) //true if Player's Luck is one of the numbers
        {
            return (player == null) ?  false : numbers.Contains(player.Luck);
        }
    }

}
