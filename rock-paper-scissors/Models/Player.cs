namespace Rock_Paper_Scissors.Models
{
    public enum Move
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    public class Player
    {
        public string Name { get; set; }
        public int Wins { get; set; }
        public Move LastMove { get; set; }
    }
}
