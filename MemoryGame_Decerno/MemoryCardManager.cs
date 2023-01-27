using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryGame_Decerno
{
    public class MemoryCardManager
    {
        public static MemoryCardManager instance;
        public event EventHandler Match;
        public event EventHandler MisMatch;

        private Dictionary<int, Card> cardDict;

        public MemoryCardManager()
        {
            instance = this;
        }

        protected virtual void OnMatch(EventArgs e)
        {
            Match?.Invoke(this, e);
        }

        protected virtual void OnMisMatch(EventArgs e)
        {
            MisMatch?.Invoke(this, e);
        }

        public void CreateCards(int nCards, List<Color> colors)
        {
            int[] indices = new int[nCards];
            for (int i = 0; i < nCards; i++)
            {
                indices[i] = i;
            }

            cardDict = new Dictionary<int, Card>();
            for (int i = 0; i < nCards; i++)
            {
                Card card = new Card(colors[i]);
                cardDict[indices[i]] = card;
            }
        }

        public List<Color> CreateColors(int nColors)
        {
            List<Color> colorList = new List<Color>();
            Random r = new Random();
            for (int i = 0; i < nColors; i++)
            {
                Color c = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                colorList.Add(c);
                colorList.Add(c);
            }

            colorList = colorList.OrderBy(c => r.Next()).ToList();
            return colorList;
        }

        public async void ValidateCards(Card card1, Card card2)
        {
            bool match = CompareCards(card1, card2);
            await Task.Delay(2000);
            if (match)
            {
                Match?.Invoke(this, EventArgs.Empty);
                GameManager.instance.AddScore();
            }
            else
            {
                MisMatch?.Invoke(this, EventArgs.Empty);
            }
        }
        public void ValidateCards(int card1Index, int card2Index)
        {
            Card card1 = cardDict[card1Index];
            Card card2 = cardDict[card2Index];
            ValidateCards(card1, card2);
        }

        public bool CompareCards(Card card1, Card card2)
        {
            return card1.GetColor().Equals(card2.GetColor());
        }

        public Card GetCard(int index)
        {
            return cardDict[index];
        }



    }
}
