using System.ComponentModel.DataAnnotations;
namespace LuckySpin.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        public string FirstName { get; set; }

        [Range(1,9, ErrorMessage = "Choose a number, 1-9")]
        public int Luck { get; set; }

        //Adds a decimal property called Balance. Assign appropriate Range and Error message
        [Range(3.0, 10.0, ErrorMessage = "bet from $3 to $10")]
        public decimal Balance { get; set; }

    }
}