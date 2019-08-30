using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// Token的类型
    /// </summary>
    public enum TokenType
    {
        Plus,   // +
        Minus,  // -
        Star,   // *
        Slash,  // /

        GE,     // >=
        GT,     // >
        EQ,     // ==
        LE,     // <=
        LT,     // <

        SemiColon, // ;
        LeftParen, // (
        RightParen,// )

        Assignment,// =

        If,
        Else,

        Int,

        Identifier,     //标识符

        IntLiteral,     //整型字面量
        StringLiteral   //字符串字面量
    }
}
