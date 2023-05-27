using System;
using System.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 3 || args.Length % 2 == 0 || args.Distinct().Count() != args.Length)
        {
            Console.WriteLine("Invalid input. Please provide an odd number of non-repeating strings as moves.");
            Console.WriteLine("Example: dotnet run rock paper scissors lizard Spock");
            return;
        }

        string[] moves = args;

        string hmacKey = GenerateHMACKey();
        string computerMove = GetRandomMove(moves);

        Console.WriteLine("HMAC: " + CalculateHMAC(hmacKey, computerMove));

        Console.WriteLine("Available moves:");
        for (int i = 0; i < moves.Length; i++)
        {
            Console.WriteLine((i + 1) + " - " + moves[i]);
        }

        Console.WriteLine("0 - exit");
        Console.WriteLine("? - help");

        Console.Write("Enter your move: ");
        string input = Console.ReadLine();

        if (input == "0")
        {
            Console.WriteLine("Exiting the game.");
            return;
        }

        if (input == "?")
        {
            Console.WriteLine("Help: The moves are represented by numbers. Enter the number corresponding to your move.");
            Console.WriteLine("For example, if you want to play 'rock', enter 1.");
            Console.WriteLine("Enter '0' to exit the game.");
            return;
        }

        if (!int.TryParse(input, out int playerMoveIndex) || playerMoveIndex < 1 || playerMoveIndex > moves.Length)
        {
            Console.WriteLine("Invalid move. Please enter a valid move number.");
            return;
        }

        string playerMove = moves[playerMoveIndex - 1];

        Console.WriteLine("Your move: " + playerMove);
        Console.WriteLine("Computer move: " + computerMove);

        int half = moves.Length / 2;
        int playerMoveIndexInCircle = Array.IndexOf(moves, playerMove);
        int computerMoveIndexInCircle = Array.IndexOf(moves, computerMove);

        if (playerMoveIndexInCircle == computerMoveIndexInCircle)
        {
            Console.WriteLine("It's a draw!");
        }
        else if ((playerMoveIndexInCircle >= computerMoveIndexInCircle + 1 && playerMoveIndexInCircle <= computerMoveIndexInCircle + half) ||
                 (playerMoveIndexInCircle <= computerMoveIndexInCircle - 1 && (playerMoveIndexInCircle + half) % moves.Length <= computerMoveIndexInCircle))
        {
            Console.WriteLine("You win!");
        }
        else
        {
            Console.WriteLine("You lose!");
        }

        Console.WriteLine("HMAC key: " + hmacKey);
    }

    static string GenerateHMACKey()
    {
        byte[] key = new byte[16];
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            rng.GetBytes(key);
        }
        return BitConverter.ToString(key).Replace("-", "");
    }

    static string GetRandomMove(string[] moves)
    {
        Random random = new Random();
        int index = random.Next(moves.Length);
        return moves[index];
    }

    static string CalculateHMAC(string key, string message)
    {
        byte[] keyBytes = Encoding.ASCII.GetBytes(key);
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        using (var hmac = new System.Security.Cryptography.HMACSHA256(keyBytes))
        {
            byte[] hashBytes = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }
    }
}
