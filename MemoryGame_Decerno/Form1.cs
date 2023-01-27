using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MemoryGame_Decerno
{
    public partial class Form1 : Form
    {
        private const string backGroundImagePath = "C:\\Users\\Namn\\Documents\\source\\repos\\MemoryGame_Decerno\\images\\unnamed.png";
        private const string logoImagePath = "C:\\Users\\Namn\\Documents\\source\\repos\\MemoryGame_Decerno\\images\\logo.png";
        private Image image;
        private Button selectedButton1;
        private Button selectedButton2;
        private Button[] memoryCardButtons;
        private Label scoreLabel;
        private List<PictureBox> scorePictureBoxes;
        private int cardRowSize;
        private int totalNrCards;

        public event EventHandler ResetGame;

        public Form1()
        {
            InitializeComponent();
        }

        protected virtual void OnResetGame(EventArgs e)
        {
            ResetGame?.Invoke(this, e);
        }

        private void OnMemoryCardButtonClicked(object sender, EventArgs eventArgs)
        {
            Button b = (Button)sender;
            b.Image = null;
            b.Enabled = false;
            if (selectedButton1 != null)
            {
                selectedButton2 = b;
                MemoryCardManager.instance.ValidateCards(int.Parse(b.Name), int.Parse(selectedButton1.Name));
                ToggleButtons(false);
            }
            else
            {
                selectedButton1 = b;
            }
        }

        private void OnMatch(object sender, EventArgs e)
        {
            UpdateScoreField();
            RemoveButtons();
            ToggleButtons(true);
        }

        private void OnMisMatch(object sender, EventArgs e)
        {
            ResetButtons();
            ToggleButtons(true);
        }

        private void ToggleButtons(bool toggle)
        {
            foreach(Button b in memoryCardButtons)
            {
                b.Enabled = toggle;
            }
        }

        private void OnGameFinished(object sender, EventArgs e)
        {
            string message = "Do you want to play again?";
            string caption = "Congratulations! You finished the game.";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);

            if (result == DialogResult.No)
            {
                this.Close();
            }
            else
            {
                GameManager.instance.ResetGame();
                ResetForm();
            }
        }

        private void UpdateScoreField()
        {
            scoreLabel.Text = $"Current Score: {GameManager.instance.score+1}";
            scorePictureBoxes[GameManager.instance.score].BackColor = selectedButton1.BackColor;
        }

        private void ResetButtons()
        {
            selectedButton1.Enabled = true;
            selectedButton1.Image = image;
            selectedButton1 = null;
            selectedButton2.Enabled = true;
            selectedButton2.Image = image;
            selectedButton2 = null;
        }

        private void RemoveButtons()
        {
            selectedButton1.Hide();
            selectedButton2.Hide();
            selectedButton1 = null;
            selectedButton2 = null;
        }

        private void ResetForm()
        {
            foreach (Button b in memoryCardButtons)
            {
                Controls.Remove(b);
            }
            CreateButtons();
            foreach (PictureBox pictureBox in scorePictureBoxes)
            {
                pictureBox.BackColor = Color.Gray;
            }
            scoreLabel.Text = "Current Score: 0";
            selectedButton1 = null;
            selectedButton2 = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            MemoryCardManager.instance.Match += OnMatch;
            MemoryCardManager.instance.MisMatch += OnMisMatch;
            GameManager.instance.GameFinished += OnGameFinished;

            totalNrCards = GameManager.instance.finalScore * 2;
            cardRowSize = (int)Math.Sqrt(totalNrCards);

            CreateButtons();
            CreateLogo();
            CreateScoreArea();
        }

        private void CreateLogo()
        {
            PictureBox logoImage = new PictureBox();
            logoImage.Image = Image.FromFile(logoImagePath);
            logoImage.Width = 150;
            logoImage.Height = 60;
            logoImage.Location = new Point(100, 0);
            Controls.Add(logoImage);
        }

        private void CreateScoreArea()
        {
            scoreLabel = new Label();
            scoreLabel.Text = $"Current Score: 0";
            scoreLabel.Width = 200;
            scoreLabel.Height = 100;
            scoreLabel.Font = new Font("Arial", 16);
            scoreLabel.Location = new Point(0, 400);
            scorePictureBoxes = new List<PictureBox>();
            for (int i = 0; i < GameManager.instance.finalScore; i++)
            {
                PictureBox scoreBox = new PictureBox();
                scoreBox.BackColor = Color.Gray;
                scoreBox.Width = 40;
                scoreBox.Height = 50;
                scoreBox.Location = new Point((scoreBox.Width + 2) * i, 430);
                scorePictureBoxes.Add(scoreBox);
                Controls.Add(scoreBox);
            }
            Controls.Add(scoreLabel);
        }


        private void CreateButtons()
        {
            image = Image.FromFile(backGroundImagePath);
            int x = 0;
            int y = 0;
            memoryCardButtons = new Button[totalNrCards];
            for (int i = 0; i < totalNrCards; i++)
            {
                Button b = new Button();
                b.Width = 60;
                b.Height = 70;
                b.Image = image;
                b.BackColor = MemoryCardManager.instance.GetCard(i).GetColor();
                if (i % cardRowSize == 0)
                {
                    y += b.Height;
                    x = 0;
                }
                x += b.Width;
                b.Location = new Point(x, y);
                b.Name = i.ToString();
                b.Click += (sender, EventArgs) => OnMemoryCardButtonClicked(sender, EventArgs);
                memoryCardButtons[i] = b;
                Controls.Add(b);

            }
        }
    }
}
