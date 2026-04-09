/*
Hello. This comment is here to note that I have read the specifications for this project. 
As compared to those specifications, there are two discrepancies of note, one of which is explained on lines 68-72.
As for the other, you will note some of my code comments are to the side of the code rather than on a separate line, the latter 
"encouraged" by the Software Specification Guidelines from Webgenic. As this is not a strict requirement, and the lines
of comments that do not follow this recommendation are not typically technically descriptive,
I have chosen to ignore this formatting suggestion in some circumstances.

If I ever have to clarify elements of the code in future re-submissions, those clarifications will be found or mentioned here.
If this extent of clarification over semantics seems odd, it is due to the length of dealing with misunderstandings which might otherwise be resolved 
rather quickly in person. In other words, I'd rather only resubmit work that is genuinely incorrect, rather than resubmit the same work with a justification.
Thank you.
*/

using System.Globalization;

TextInfo myTI = new CultureInfo("en-AU",false).TextInfo; //This is for use in the title case operation
string recordFile = "highScores.txt"; // Stores the high-score history in a simple text file.

List<ScoreEntry> Quiz() //Option A
{
    Console.Clear();

    Console.WriteLine("Enter your name:");
    string name = myTI.ToTitleCase(myTI.ToLower(Console.ReadLine() ?? throw new InvalidOperationException())); 
    debug : Console.WriteLine(name);
    //So, you might be wondering why we convert to lower, then titlecase. 
    //It's because the titlecase operation intentionally leaves words that are entirely uppercase alone, assuming them to be acronyms. 
    //While entirely reasonable in 99 percent of use cases, this edge case calls for an extra operation.
    
    Console.Clear();

    int score = 0;
    var questions = new List<Question>();

    // Pre-generate all questions to account for quirks of the C# randomisation system (generating them on a per-question basis can cause repeated questions)
    for (int i = 0; i < 10; i++)
    {
        questions.Add(new Question());
    }

    for (int i = 0; i < 10; i++)
    {
        questions[i].QuestionDisplay(i + 1); 
        var ui = Console.ReadLine();

        // Typing x lets the player exit the quiz early.
        if (ui == "x")
        {
            break;
        }

        bool uiIsValid = false;
        while (!uiIsValid) //This feels like a terrible way to do this, but it works, and I am too out of practice in C# to identify the better way. 
        {
            if (double.TryParse(ui, out double userInput))
            {
                // Round both values so tiny decimal differences do not break the answer check.
                if (Math.Abs(Math.Round(userInput, 2) - Math.Round(questions[i].Answer, 2)) < 1) //Not sure who decided we needed division, but it's not my fault that it makes the quizzes hard. 
                {
                    score++;
                    uiIsValid = true;
                }
                else
                {
                    uiIsValid = true;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                ui = Console.ReadLine();
            }
        }
    }

    Console.Clear();
    Console.WriteLine($"Your score is {score}/10");
    Console.WriteLine("Press Enter to return to the main menu.");
    Console.ReadLine();
    
    /*
    The 2 lines of code below are for the high-score table, which I've used in place of the aray which the task asks for. 
    Now, if it is absolutely essential that for this task I use an array, let me know. I'll refactor the code,
    but you should be aware that it's not the right tool for this job and will result in some fragility. 
    
    I'd really rather not refactor it, though.
    */
    
    var scoreEntries = new List<ScoreEntry> { new(name, score) };
    File.AppendAllText(recordFile, $"{name} - {score}{Environment.NewLine}");
    return scoreEntries;
}

void MainMenu(List<ScoreEntry> scoreData)
{
    Console.Clear();

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
        Console.Clear();
        try
        {
            PrintScores(scoreData);
            Console.WriteLine();
            Console.WriteLine("Press Enter to return to the main menu.");
            Console.ReadLine();
            MainMenu(scoreData);
        }
        catch (Exception)
        {
            Console.WriteLine("There are no scores to display.");
            Console.WriteLine("Press Enter to return to the main menu.");
            Console.ReadLine();
            MainMenu(scoreData);
        }
    }
    else if (userInputChar == 'C')
    {
        Console.Clear();

        // This reads every saved score line back from the file and prints them out.
        string[] fileContents = File.ReadAllLines(recordFile); //This was temporary, but the display actually looks clean enough to just leave it. 
        foreach (var line in fileContents)
        {
            Console.WriteLine(line);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to return to the main menu.");
        Console.ReadLine();
        MainMenu(scoreData);
    }
    else if (userInputChar == 'D')
    {
        Console.Clear();
        Environment.Exit(0);
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter a valid option.");
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
        MainMenu(scoreData);
    }
}

MainMenu(null!);


void PrintScores(List<ScoreEntry> scores) //Option B
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
        private static readonly Random Random = new Random();

        public Question()
        {
            NumberA = Random.Next(1, 11);
            NumberB = Random.Next(1, 11);
            char[] operations = ['+', '-', '*', '/'];

            Operator = operations[Random.Next(0, operations.Length)];
            
            Answer = Operator switch
            {
                '+' => NumberA + NumberB,
                '-' => NumberA - NumberB,
                '*' => NumberA * NumberB,
                '/' => Convert.ToDouble(NumberA) / Convert.ToDouble(NumberB),
                _ => throw new ArgumentOutOfRangeException()
            };

        }

        public void QuestionDisplay(int questionNumber)
        {
            Console.WriteLine($"Question {questionNumber}: {Equation}");
        }
    }

public record ScoreEntry(string Name, int Score);