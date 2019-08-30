using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// 一个简单的Token流。是把一个Token列表进行了封装。
    /// </summary>
    public class SimpleTokenReader : TokenReader
    {
        List<Token> tokens = null;
        int pos = 0;

        public SimpleTokenReader(List<Token> tokens)
        {
            this.tokens = tokens;
        }


        public int getPosition()
        {
            return pos;
        }

        public Token peek()
        {
            if (pos < tokens.Count)
                return tokens[pos];
            return null;
        }

        public Token read()
        {
            if (pos < tokens.Count)
            {
                return tokens[pos++];
            }
            return null;

        }

        public void setPosition(int position)
        {
            if (position >= 0 && position < tokens.Count)
            {
                pos = position;
            }
        }

        public void unread()
        {
            if (pos > 0)
            {
                pos--;
            }
        }
    }
}
