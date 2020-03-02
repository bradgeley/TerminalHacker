using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour
{

    //Game configuration Data
    public const int MAX_LIVES = 3;
    string[] BombCodes = { "343 XXX XXX", "XXX 999 XXX", "XXX XXX 123" };
    string[] MainMenuText = {
        "Hello Agent.",
        "Welcome to your first day on the job.",
        "We need you to hack into the following systems and gain key information about an upcoming attack on a local gym.",
        "Good luck.",
        "",
    };
    string[] LoseScreenText = {
        "BOOOOOOOM!!!",
        "The bomb has exploded.",
        "Casualties: 42069",
    };
    string[] DifficultyNames = 
    { 
        "Callum Von Moger's IPhone", 
        "Alex's Desktop Computer", 
        "24 Hr Fitness Mainframe" 
    };
    Dictionary<int, string[]> GameDifficultyDictionary = new Dictionary<int, string[]>
    {
        [1] = new string[] { "Dog", "australia", "bodybuilder", "puppy", "tear" },
        [2] = new string[] { "Deisy", "spanish", "schwab", "finance", "money" },
        [3] = new string[] { "Biceps", "triceps", "pectoral", "posterior", "hamstrings", "quadriceps", "anterior" }
    };

    //Game State variables
    public GameState currentState;
    public int difficulty; // 1 2 or 3
    public int lives;
    public string password = "";
    public string scrambledPassword = "";
    GameState State
    {
        get { return currentState; }
        set
        {
            currentState = value;
            if (value == GameState.MainMenu)
            {
                resetGame();
                PrintMainMenuText();
            }
            if (value == GameState.Password)
            {
                RandomizePassword();
                scrambledPassword = ScrambleWord(password);
                PrintPasswordScreenText();
            }
            if (value == GameState.Win)
            {
                PrintWinScreenText();
            }
            if (value == GameState.Lose)
            {
                PrintLoseScreenText();
            }
        }
    }

    public enum GameState { MainMenu, Password, Win, Lose };

    void Start()
    {
        State = GameState.MainMenu;
    }

    void resetGame()
    {
        lives = MAX_LIVES;
        difficulty = 0;
        password = "";
        scrambledPassword = "";
    }

    void PrintMainMenuText()
    {
        Terminal.ClearScreen();
        foreach (string line in MainMenuText)
        {
            Terminal.WriteLine(line);
        }
        int count = 1;
        foreach (string entry in DifficultyNames)
        {
            string line = "Enter " + count + " for " + entry;
            Terminal.WriteLine(line);
            count++;
        }
        Terminal.WriteLine("");
    }

    void PrintPasswordScreenText()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine(DifficultyNames[difficulty-1]);
        Terminal.WriteLine("");
        Terminal.WriteLine("XX_" + scrambledPassword  + "_XX");
        Terminal.WriteLine("");
        Terminal.WriteLine("Enter password (Remaining Attempts: " + lives + ")");
    }

    void PrintWinScreenText()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("The bomb deactivation code from this device is:");
        Terminal.WriteLine("");
        Terminal.WriteLine(BombCodes[difficulty - 1]);
        Terminal.WriteLine("");
        Terminal.WriteLine("Enter 'menu' to return to main menu.");
    }

    void PrintLoseScreenText()
    {
        Terminal.ClearScreen();
        foreach (string line in LoseScreenText)
        {
            Terminal.WriteLine(line);
        }
    }

    void OnUserInput(string input)
    {
        if (input == "menu")
        {
            State = GameState.MainMenu;
        }
        else
        {
            switch (State)
            {
                case GameState.MainMenu:
                    MainMenuInput(input);
                    break;
                case GameState.Password:
                    PasswordInput(input);
                    break;
                case GameState.Win:
                    break;
                default:
                    State = GameState.MainMenu;
                    break;
            }
        }
    }

    void MainMenuInput(string input)
    {
        switch (input)
        {
            case "allyourbasearebelongtomine":
                Terminal.ClearScreen();
                Terminal.WriteLine("You received 500 gold.");
                Terminal.WriteLine("Well...not really...");
                break;
            case "343 999 123":
                Terminal.ClearScreen();
                Terminal.WriteLine("You have successfully disarmed the bomb!");
                Terminal.WriteLine("Well...not really...");
                break;
            default:
                if (IsValidDifficultySelection(input))
                {
                    State = GameState.Password;
                }
                else
                {
                    Terminal.WriteLine("Please choose a valid level.");
                }
                break;
        }
    }

    bool IsValidDifficultySelection(string input)
    {
        foreach (int key in GameDifficultyDictionary.Keys)
        {
            int.TryParse(input, out int valueAsNumber);
            if (valueAsNumber == key)
            {
                difficulty = key;
                return true;
            }

        }
        return false;
    }

    void PasswordInput(string input)
    {
        if (input == password)
        {
            State = GameState.Win;
        }
        else
        {
            lives--;
            scrambledPassword = ScrambleWord(password);
            PrintPasswordScreenText();
            if (lives == 0)
            {
                State = GameState.Lose;
            }
        }
    }

    void RandomizePassword()
    {
        foreach (KeyValuePair<int, string[]> pair in GameDifficultyDictionary)
        {
            if (pair.Key == difficulty)
            {
                int randomIndex = Random.Range(0, pair.Value.Length - 1);
                password = pair.Value[randomIndex].ToLower();
            }
        }
    }

    string ScrambleWord(string input)
    {
        string copy = input;
        string result = "";
        for (int i = 0; i < input.Length; i++) 
        {
            int randomIndex = Random.Range(0, copy.Length - 1);
            char randomChar = copy[randomIndex];
            result += randomChar;
            copy = copy.Remove(randomIndex, 1);

        }
        return result;
    }
}