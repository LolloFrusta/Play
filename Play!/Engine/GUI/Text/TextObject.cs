using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class TextObject
    {
        protected List<TextChar> sprites;
        protected string text;
        protected bool isActive;
        protected Font font;
        protected int hSpace;

        protected Vector2 position;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; UpdateCharStatus(value); }
        }

        public string Text
        {
            get { return text; }
            set { SetText(value); }
        }

        public TextObject(Vector2 spritePos, string textString = "", Font font = null, int horizontalSpace = 0)
        {
            position = spritePos;
            hSpace = horizontalSpace;

            if (font == null)
            {
                font = FontMgr.GetFont();
            }
            this.font = font;

            sprites = new List<TextChar>();
            if (textString != "")
            {
                SetText(textString);
            }

            IsActive = true;
        }

        private void SetText(string newText)
        {
            if (newText != text)
            {
                text = newText;
                int numChars = text.Length;
                float charX = position.X;
                float charY = position.Y;

                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];

                    if (i > sprites.Count - 1)//i is greater than last char index
                    {
                        TextChar tc = new TextChar(new Vector2(charX, charY), c, font);
                        tc.IsActive = true;
                        sprites.Add(tc);
                    }
                    else if (c != sprites[i].Character)
                    {
                        sprites[i].Character = c;
                    }

                    charX += sprites[i].HalfWidth * 2 + hSpace;
                }

                if (sprites.Count > text.Length)
                {
                    int count = sprites.Count - text.Length;
                    int from = text.Length;

                    //destroy chars
                    for (int i = from; i < sprites.Count; i++)
                    {
                        sprites[i].Destroy();
                    }

                    sprites.RemoveRange(from, count);
                }

            }
        }



        protected virtual void UpdateCharStatus(bool activeStatus)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].IsActive = activeStatus;
            }
        }
    }
}
