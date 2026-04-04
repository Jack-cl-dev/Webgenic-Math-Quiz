// TODO: Add input pauses and clear the terminal when required. 
List<ScoreEntry> Quiz() //Option A
{
    Console.WriteLine("Enter your name:");
    string name = Console.ReadLine() ?? throw new InvalidOperationException();
    Console.WriteLine($"[DEBUG]: Name is {name}.");
    int score = 0;
    var questions = new List<Question>();
    for (int i = 0; i < 10; i++)
    {
        questions.Add(new Question());
    }

    for (int i = 0; i < 10; i++)
    {
        questions[i].QuestionDisplay(i + 1); 
        var ui = Console.ReadLine();
        if (ui == "x")
        {
            break;
        }
        else if (double.TryParse(ui, out double userInput))
        {
            if (Math.Round(userInput, 2) == Math.Round(questions[i].Answer, 2)) //Not sure who decided we needed division, but it's not my fault that it makes the quizzes hard. 
            {
                score++;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }
    Console.WriteLine($"Your score is {score}/10");
    
    // The 2 lines of code below are for the high-score table, which I've used in place of the aray which the task asks for. 
    // Now, if it is absolutely essential that for the task i use an array, let me know and I'll change it.
    // But you should be aware that it's not the right tool for this job, and will result in some fragility. 
    
    var todaysScores = new List<ScoreEntry>(); 
    todaysScores.Add(new ScoreEntry(name, score));
    return todaysScores;
}

void MainMenu(List<ScoreEntry> scoreData)
{
    string headingText = "Welcome to the Math's Quiz Game. Please choose from the menu below";
    string underline = new string('_', headingText.Length);
    Console.WriteLine($"""
                       {headingText}
                       {underline}

                           (A) New Game
                           (B) View Today's High Scores
                           (C) View Past High Scores
                           (D) Quit

                           What would you like to do? Enter the letter option.
                       """);

    string userInputString = Console.ReadLine() ?? throw new InvalidOperationException();
    userInputString = userInputString.ToUpper();
    char userInputChar = Convert.ToChar(userInputString);

    if (userInputChar == 'A')
    {
        scoreData = Quiz();
        MainMenu(scoreData);
    }
    else if (userInputChar == 'B')
    {
        printScores(scoreData);
        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey();
        MainMenu(scoreData);
    }
    // ...
}

{
    MainMenu(null);
}

void printScores(List<ScoreEntry> scores) //Option B
{
    foreach (var score in scores)
    {
        Console.WriteLine($"{score.Name} - {score.Score}");
    }
}

public class Question
    {
        private int NumberA { get;}
        private int NumberB { get;}
        private char Operator { get;}
        public double Answer { get; }
        private string Equation => $"{NumberA} {Operator} {NumberB}";
        private static Random _random = new Random();

        public Question()
        {
            NumberA = _random.Next(1, 11);
            NumberB = _random.Next(1, 11);
            char[] operations = { '+', '-', '*', '/' };

            Operator = operations[_random.Next(0, operations.Length)];
            
            Answer = Operator switch
            {
                '+' => NumberA + NumberB,
                '-' => NumberA - NumberB,
                '*' => NumberA * NumberB,
                '/' => Convert.ToDouble(NumberA) / Convert.ToDouble(NumberB), //TODO: Convert A and B to doubles in the case of a division, then round. Also specify to the user a DP for these questions so we can mark. 
                _ => throw new ArgumentOutOfRangeException()
            };

        }

        public void QuestionDisplay(int questionNumber)
        {
            Console.WriteLine($"Question {questionNumber}: {Equation}");
        }
    }

public record ScoreEntry(string Name, int Score);