using System;
using System.Collections.Generic;
using LeapWoF.Interfaces;

namespace LeapWoF
{

    /// <summary>
    /// The GameManager class, handles all game logic
    /// </summary>
    public class GameManager
    {

        /// <summary>
        /// The input provider
        /// </summary>
        private IInputProvider inputProvider;

        /// <summary>
        /// The output provider
        /// </summary>
        private IOutputProvider outputProvider;

        private string TemporaryPuzzle;
        public List<string> charGuessList = new List<string>();

        public GameState GameState { get; private set; }

        public GameManager() : this(new ConsoleInputProvider(), new ConsoleOutputProvider())
        {

        }

        public GameManager(IInputProvider inputProvider, IOutputProvider outputProvider)
        {
            if (inputProvider == null)
                throw new ArgumentNullException(nameof(inputProvider));
            if (outputProvider == null)
                throw new ArgumentNullException(nameof(outputProvider));

            this.inputProvider = inputProvider;
            this.outputProvider = outputProvider;

            GameState = GameState.WaitingToStart;
        }

        /// <summary>
        /// Manage game according to game state
        /// </summary>
        public void StartGame()
        {
            InitGame();

            while (true)
            {

                PerformSingleTurn();

                if (GameState == GameState.RoundOver)
                {
                    StartNewRound();
                    continue;
                }

                if (GameState == GameState.GameOver)
                {
                    outputProvider.WriteLine("Game over");
                    break;
                }
            }
        }
        public void StartNewRound()
        {
            TemporaryPuzzle = "Hello world";

            // update the game state
            GameState = GameState.RoundStarted;
        }

        public void PerformSingleTurn()
        {
            outputProvider.Clear();
            DrawPuzzle();
            outputProvider.WriteLine("Type 1 to spin, 2 to solve");
            GameState = GameState.WaitingForUserInput;

            var action = inputProvider.Read();

            switch (action)
            {
                case "1":
                    Spin();
                    break;
                case "2":
                    Solve();
                    break;
            }

        }

        /// <summary>
        /// Draw the puzzle
        /// </summary>
        private void DrawPuzzle()
        {
            outputProvider.WriteLine("The puzzle is:");
            outputProvider.WriteLine();
            string displayString = "";

            for (int i = 0; i < TemporaryPuzzle.Length; i++)
            {
                if (charGuessList.Contains(TemporaryPuzzle[i].ToString().ToLower()))
                {
                    displayString += TemporaryPuzzle[i];
                }
                else if (string.IsNullOrWhiteSpace(TemporaryPuzzle[i].ToString()))
                {
                    displayString += " ";
                }
                else
                {
                    displayString += "-";
                }
            }
            outputProvider.WriteLine(displayString);
            outputProvider.WriteLine();
        }

        /// <summary>
        /// Spin the wheel and do the appropriate action
        /// </summary>
        public void Spin()
        {
            outputProvider.WriteLine("Spinning the wheel...");
            //TODO - Implement wheel + possible wheel spin outcomes
            GuessLetter();
        }

        public void Solve()
        {
            outputProvider.Write("Please enter your solution:");
            var guess = inputProvider.Read();
            if (guess.ToLower().Equals(TemporaryPuzzle.ToLower()))
            {
                outputProvider
                    .WriteLine("Correct! You win!");
                System.Threading.Thread.Sleep(5000);
                GameState = GameState.GameOver;
            }else
            {
                outputProvider.WriteLine("Incorrect!");
                System.Threading.Thread.Sleep(5000);
            }
            
        }

        public void GuessLetter()
        {
            outputProvider.Write("Please guess a letter: ");
            var guess = inputProvider.Read();
            if (guess.Length > 1)
            {
                outputProvider.WriteLine("Please guess only one letter at a time.");
                System.Threading.Thread.Sleep(5000);
            }
            else if (!TemporaryPuzzle.ToLower().Contains(guess.ToLower()))
            {
                outputProvider.WriteLine("Incorrect. Try again!");
                System.Threading.Thread.Sleep(5000);
            }
            else
            {
                charGuessList.Add(guess.ToLower());
            }
        }

        /// <summary>
        /// Optional logic to accept configuration options
        /// </summary>
        public void InitGame()
        {

            outputProvider.WriteLine("Welcome to Wheel of Fortune!");
            StartNewRound();
        }
    }
}
