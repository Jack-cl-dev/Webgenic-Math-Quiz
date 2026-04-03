void Quiz() //Option A
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
        questions[i].QuestionDisplay(i + 1); //TODO: This is not ready for handle doubles
        var ui = Console.ReadLine();
        try // Filthy. 
        {
            char answerChar = Convert.ToChar(ui);
            if (answerChar == 'x')
            {
                break;
            }
        }
        catch
        {
            try
            {
               int answer = Convert.ToInt32(ui);
               if (answer == questions[i].Answer)
               {
                   Console.WriteLine("[DEBUG] Correct answer");
                   Console.WriteLine(questions[i].Answer);
                   score++;
               }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Press any key to try again.");
                Console.ReadLine();
                i--;
                continue;
            }
        }
    }
    Console.WriteLine($"Your score is {score} out of ten.");
    
    // The 2 lines of code below are for the high-score table, which I've used in place of the aray which the task asks for. 
    // Now, If it is absolutely essential that for the task i use an array, let me know and I'll change it.
    // But you should be aware that it's not the right tool for this job, and will result in some fragility. 
    var todaysScores = new List<ScoreEntry>(); 
    todaysScores.Add(new ScoreEntry(name, score));
}

void MainMenu()
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
    try
    {
        string userInputString = Console.ReadLine() ?? throw new InvalidOperationException();
        userInputString = userInputString.ToUpper();
        char userInputChar = Convert.ToChar(userInputString);
        if (userInputChar == 'A')
        {
            Quiz();
        }
        else if (userInputChar == 'B')
        {
            
        }
        else if (userInputChar == 'C')
        {
            
        }
        else if (userInputChar == 'D')
        {
            
        }
        else
        {
            throw new Exception();
        }
    }
    catch
    {
        Console.WriteLine("Invalid input. Press any key to try again.");
        Console.ReadLine();
        MainMenu();
    }
}
MainMenu();

public class Question
    {
        public int NumberA { get;}
        public int NumberB { get;}
        public char Operator { get;}
        public int Answer { get; set; }
        public string Equation => $"{NumberA} {Operator} {NumberB}";
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
                '/' => NumberA / NumberB, //TODO: Convert A and B to doubles in the case of a division, then round. Also specify to the user a DP for these questions so we can mark. 
                _ => throw new ArgumentOutOfRangeException()
            };

        }

        public void QuestionDisplay(int questionNumber)
        {
            Console.WriteLine($"Question {questionNumber}: {Equation}");
        }
    }

public record ScoreEntry(string Name, int Score);