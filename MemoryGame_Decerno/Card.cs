using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MemoryGame_Decerno
{
    public class Card
    {
        private Color color;

        public Card(Color color)
        {
            this.color = color;
        }

        public Color GetColor()
        {
            return this.color;
        }


    }
}
