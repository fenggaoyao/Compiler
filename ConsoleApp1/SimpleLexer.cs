using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
namespace ConsoleApp1
{
    public class SimpleLexer
    {
        //下面几个变量是在解析过程中用到的临时变量,如果要优化的话，可以塞到方法里隐藏起来
        private StringBuilder tokenText = null;   //临时保存token的文本      
        private SimpleToken token = null;        //当前正在解析的Token
        private List<Token> tokens = null;       //保存解析出来的Token

        /**
      * 解析字符串，形成Token。
      * 这是一个有限状态自动机，在不同的状态中迁移。
      * @param code
      * @return
      */
        public SimpleTokenReader Tokenize(String code)
        {             
            tokens = new List<Token>();
            StringReader reader = new StringReader(code);           
            tokenText = new StringBuilder();
            token = new SimpleToken();
            int ich = 0;
            char ch=(char)0;
            DfaState state = DfaState.Initial;
            try
            {
                while ((ich = reader.Read()) != -1)
                {
                    ch = (char)ich;
                    switch (state)
                    {
                        case DfaState.Initial:
                            state = initToken(ch);          //重新确定后续状态
                            break;
                        case DfaState.Id:
                            if (isAlpha(ch) || isDigit(ch))
                            {
                                tokenText.Append(ch);       //保持标识符状态
                            }
                            else
                            {
                                state = initToken(ch);      //退出标识符状态，并保存Token
                            }
                            break;
                        case DfaState.GT:
                            if (ch == '=')
                            {
                                token.type = TokenType.GE;  //转换成GE
                                state = DfaState.GE;
                                tokenText.Append(ch);
                            }
                            else
                            {
                                state = initToken(ch);      //退出GT状态，并保存Token
                            }
                            break;
                        case DfaState.GE:
                        case DfaState.Assignment:
                        case DfaState.Plus:
                        case DfaState.Minus:
                        case DfaState.Star:
                        case DfaState.Slash:
                        case DfaState.SemiColon:
                        case DfaState.LeftParen:
                        case DfaState.RightParen:
                            state = initToken(ch);          //退出当前状态，并保存Token
                            break;
                        case DfaState.IntLiteral:
                            if (isDigit(ch))
                            {
                                tokenText.Append(ch);       //继续保持在数字字面量状态
                            }
                            else
                            {
                                state = initToken(ch);      //退出当前状态，并保存Token
                            }
                            break;
                        case DfaState.Id_int1:
                            if (ch == 'n')
                            {
                                state = DfaState.Id_int2;
                                tokenText.Append(ch);
                            }
                            else if (isDigit(ch) || isAlpha(ch))
                            {
                                state = DfaState.Id;    //切换回Id状态
                                tokenText.Append(ch);
                            }
                            else
                            {
                                state = initToken(ch);
                            }
                            break;
                        case DfaState.Id_int2:
                            if (ch == 't')
                            {
                                state = DfaState.Id_int3;
                                tokenText.Append(ch);
                            }
                            else if (isDigit(ch) || isAlpha(ch))
                            {
                                state = DfaState.Id;    //切换回id状态
                                tokenText.Append(ch);
                            }
                            else
                            {
                                state = initToken(ch);
                            }
                            break;
                        case DfaState.Id_int3:
                            if (isBlank(ch))
                            {
                                token.type = TokenType.Int;
                                state = initToken(ch);
                            }
                            else
                            {
                                state = DfaState.Id;    //切换回Id状态
                                tokenText.Append(ch);
                            }
                            break;
                        default:
                            break;

                    }

                }
                // 把最后一个token送进去
                if (tokenText.Length > 0)
                {
                    initToken(ch);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return new SimpleTokenReader(tokens);
        }

        private DfaState initToken(char ch)
        {
            if (tokenText.Length > 0)
            {
                token.text = tokenText.ToString();
                tokens.Add(token);

                tokenText = new StringBuilder();
                token = new SimpleToken();
            }

            DfaState newState = DfaState.Initial;
            if (isAlpha(ch))
            {              //第一个字符是字母
                if (ch == 'i')
                {
                    newState = DfaState.Id_int1;
                }
                else
                {
                    newState = DfaState.Id; //进入Id状态
                }
                token.type = TokenType.Identifier;
                tokenText.Append(ch);
            }
            else if (isDigit(ch))
            {       //第一个字符是数字
                newState = DfaState.IntLiteral;
                token.type = TokenType.IntLiteral;
                tokenText.Append(ch);
            }
            else if (ch == '>')
            {         //第一个字符是>
                newState = DfaState.GT;
                token.type = TokenType.GT;
                tokenText.Append(ch);
            }
            else if (ch == '+')
            {
                newState = DfaState.Plus;
                token.type = TokenType.Plus;
                tokenText.Append(ch);
            }
            else if (ch == '-')
            {
                newState = DfaState.Minus;
                token.type = TokenType.Minus;
                tokenText.Append(ch);
            }
            else if (ch == '*')
            {
                newState = DfaState.Star;
                token.type = TokenType.Star;
                tokenText.Append(ch);
            }
            else if (ch == '/')
            {
                newState = DfaState.Slash;
                token.type = TokenType.Slash;
                tokenText.Append(ch);
            }
            else if (ch == ';')
            {
                newState = DfaState.SemiColon;
                token.type = TokenType.SemiColon;
                tokenText.Append(ch);
            }
            else if (ch == '(')
            {
                newState = DfaState.LeftParen;
                token.type = TokenType.LeftParen;
                tokenText.Append(ch);
            }
            else if (ch == ')')
            {
                newState = DfaState.RightParen;
                token.type = TokenType.RightParen;
                tokenText.Append(ch);
            }
            else if (ch == '=')
            {
                newState = DfaState.Assignment;
                token.type = TokenType.Assignment;
                tokenText.Append(ch);
            }
            else
            {
                newState = DfaState.Initial; // skip all unknown patterns
            }
            return newState;
        }


       
        /// <summary>
        ///  打印所有的Token
        /// </summary>
        /// <param name="tokenReader">tokenReader</param>
        public static void Dump(SimpleTokenReader tokenReader)
        {
            Console.WriteLine("text\ttype");            
            Token token = null;
            while ((token = tokenReader.read()) != null)
            {
                Console.WriteLine(token.getText() + "\t\t" + token.getType());
            }
        }


        /// <summary>
        /// 有限状态机的各种状态。
        /// </summary>
        private enum DfaState
        {
            Initial,

            If, Id_if1, Id_if2, Else, Id_else1, Id_else2, Id_else3, Id_else4, Int, Id_int1, Id_int2, Id_int3, Id, GT, GE,

            Assignment,

            Plus, Minus, Star, Slash,

            SemiColon,
            LeftParen,
            RightParen,

            IntLiteral
        }

        //是否是字母
        private Boolean isAlpha(int ch)
        {
            return ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z';
        }

        //是否是数字
        private Boolean isDigit(int ch)
        {
            return ch >= '0' && ch <= '9';
        }

        //是否是空白字符
        private Boolean isBlank(int ch)
        {
            return ch == ' ' || ch == '\t' || ch == '\n';
        }

    }

    public class SimpleToken : Token
    {
        //Token类型
        public TokenType type;

        //文本值
        public String text = null;
        public string getText()
        {
            return text;
        }

        public TokenType getType()
        {
            return type;
        }
    }


}
