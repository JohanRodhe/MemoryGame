using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MemoryGame_Decerno
{
    public class GameManager
    {
        public static GameManager instance;
        public int score;
        public readonly int finalScore;
        public readonly int mapSize;
        public event EventHandler GameFinished;

        public GameManager(int mapSize = 16)
        {
            instance = this;
            this.finalScore = mapSize / 2;
            this.mapSize = mapSize;

        }

        [STAThread]
        private static void Main()
        {
            int mapSize = 16;
            GameManager gameManager = new GameManager(mapSize: mapSize);
            MemoryCardManager cardManager = new MemoryCardManager();
            List<Color> colors = cardManager.CreateColors(mapSize / 2);
            cardManager.CreateCards(nCards: mapSize, colors: colors);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //List<Color> colorsTest = new List<Color>{ Color.Red, Color.Red };
            //cardManager.CreateCards(2, colorsTest);
            //bool match = cardManager.CompareCards(cardManager.GetCard(0), cardManager.GetCard(1));
            //assert equals match, true
        }

        protected virtual void OnGameFinished(EventArgs e)
        {
            GameFinished?.Invoke(this, e);
        }

        public void AddScore()
        {
            score++;
            if (score == finalScore)
            {
                GameFinished?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ResetGame()
        {
            score = 0;
            List<Color> colors = MemoryCardManager.instance.CreateColors(mapSize / 2);
            MemoryCardManager.instance.CreateCards(nCards: GameManager.instance.mapSize, colors: colors);
        }


    }
}
