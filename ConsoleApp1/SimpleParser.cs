using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// 语法解析
    /// </summary>
    public class SimpleParser
    {     


        /**
    * 解析脚本
    * @param script
    * @return
    * @throws Exception
    */
        public ASTNode parse(String script)
        {
            SimpleLexer lexer = new SimpleLexer();
            TokenReader tokens = lexer.Tokenize(script);
            ASTNode rootNode = Prog(tokens);
            return rootNode;
        }

        /**
         * AST的根节点，解析的入口。
         * @return
         * @throws Exception
         */
        private SimpleASTNode Prog(TokenReader tokens)
        {
            SimpleASTNode node = new SimpleASTNode(ASTNodeType.Programm, "pwc");

            while (tokens.peek() != null)
            {
                SimpleASTNode child = intDeclare(tokens);

                if (child == null)
                {
                    child = expressionStatement(tokens);
                }

                if (child == null)
                {
                    child = assignmentStatement(tokens);
                }

                if (child != null)
                {
                    node.addChild(child);
                }
                else
                {
                    throw new Exception("unknown statement");
                }
            }

            return node;
        }

        /**
         * 表达式语句，即表达式后面跟个分号。
         * @return
         * @throws Exception
         */
        private SimpleASTNode expressionStatement(TokenReader tokens)
        {
            int pos = tokens.getPosition();
            SimpleASTNode node = additive(tokens);
            if (node != null)
            {
                Token token = tokens.peek();
                if (token != null && token.getType() == TokenType.SemiColon)
                {
                    tokens.read();
                }
                else
                {
                    node = null;
                    tokens.setPosition(pos); // 回溯
                }
            }
            return node;  //直接返回子节点，简化了AST。
        }

        /**
         * 赋值语句，如age = 10*2;
         * @return
         * @throws Exception
         */
        private SimpleASTNode assignmentStatement(TokenReader tokens)
        {
            SimpleASTNode node = null;
            Token token = tokens.peek();    //预读，看看下面是不是标识符
            if (token != null && token.getType() == TokenType.Identifier)
            {
                token = tokens.read();      //读入标识符
                node = new SimpleASTNode(ASTNodeType.AssignmentStmt, token.getText());
                token = tokens.peek();      //预读，看看下面是不是等号
                if (token != null && token.getType() == TokenType.Assignment)
                {
                    tokens.read();          //取出等号
                    SimpleASTNode child = additive(tokens);
                    if (child == null)
                    {    //出错，等号右面没有一个合法的表达式
                        throw new Exception("invalide assignment statement, expecting an expression");
                    }
                    else
                    {
                        node.addChild(child);   //添加子节点
                        token = tokens.peek();  //预读，看看后面是不是分号
                        if (token != null && token.getType() == TokenType.SemiColon)
                        {
                            tokens.read();      //消耗掉这个分号

                        }
                        else
                        {                //报错，缺少分号
                            throw new Exception("invalid statement, expecting semicolon");
                        }
                    }
                }
                else
                {
                    tokens.unread();            //回溯，吐出之前消化掉的标识符
                    node = null;
                }
            }
            return node;
        }



        /**
        * 整型变量声明，如：
        * int a;
        * int b = 2*3;
        *
        * @return
        * @throws Exception
*/
        private SimpleASTNode intDeclare(TokenReader tokens)
        {
            SimpleASTNode node = null;
            Token token = tokens.peek();
            if (token != null && token.getType() == TokenType.Int)
            {
                token = tokens.read();
                if (tokens.peek().getType() == TokenType.Identifier)
                {
                    token = tokens.read();
                    node = new SimpleASTNode(ASTNodeType.IntDeclaration, token.getText());
                    token = tokens.peek();
                    if (token != null && token.getType() == TokenType.Assignment)
                    {
                        tokens.read();  //取出等号
                        SimpleASTNode child = additive(tokens);
                        if (child == null)
                        {
                            throw new Exception("invalide variable initialization, expecting an expression");
                        }
                        else
                        {
                            node.addChild(child);
                        }
                    }
                }
                else
                {
                    throw new Exception("variable name expected");
                }

                if (node != null)
                {
                    token = tokens.peek();
                    if (token != null && token.getType() == TokenType.SemiColon)
                    {
                        tokens.read();
                    }
                    else
                    {
                        throw new Exception("invalid statement, expecting semicolon");
                    }
                }
            }
            return node;
        }

        /**
     * 基础表达式
     * @return
     * @throws Exception
     */
        private SimpleASTNode primary(TokenReader tokens)
        {
            SimpleASTNode node = null;
            Token token = tokens.peek();
            if (token != null)
            {
                if (token.getType() == TokenType.IntLiteral) //int
                {
                    token = tokens.read();
                    node = new SimpleASTNode(ASTNodeType.IntLiteral, token.getText());
                }
                else if (token.getType() == TokenType.Identifier)//变量
                {
                    token = tokens.read();
                    node = new SimpleASTNode(ASTNodeType.Identifier, token.getText());
                }
                else if (token.getType() == TokenType.LeftParen) // (
                {
                    tokens.read();
                    node = additive(tokens);
                    if (node != null)
                    {
                        token = tokens.peek();
                        if (token != null && token.getType() == TokenType.RightParen)
                        {
                            tokens.read(); //消耗右括号
                        }
                        else
                        {
                            throw new Exception("expecting right parenthesis");
                        }
                    }
                    else
                    {
                        throw new Exception("expecting an additive expression inside parenthesis");
                    }
                }
            }
            return node;  //这个方法也做了AST的简化，就是不用构造一个primary节点，直接返回子节点。因为它只有一个子节点。
        }

        /**
     * 加法表达式
     * @return
     * @throws Exception
     * 
 * 实现一个计算器，但计算的结合性是有问题的。因为它使用了下面的语法规则：
 *
 * additive -> multiplicative | multiplicative + additive
 * multiplicative -> primary | primary * multiplicative    //感谢@Void_seT，原来写成+号了，写错了。
 *
 * 递归项在右边，会自然的对应右结合。我们真正需要的是左结合。
 */
    
        public SimpleASTNode additive(TokenReader tokens)
        {
            SimpleASTNode child1 = multiplicative(tokens);  //应用add规则
            SimpleASTNode node = child1;
            if (child1 != null)
            {
                while (true)
                {                              //循环应用add'规则
                    Token token = tokens.peek();
                    if (token != null && (token.getType() == TokenType.Plus || token.getType() == TokenType.Minus))
                    {
                        token = tokens.read();              //读出加号
                        SimpleASTNode child2 = multiplicative(tokens);  //计算下级节点
                        if (child2 != null)
                        {
                            node = new SimpleASTNode(ASTNodeType.Additive, token.getText());
                            node.addChild(child1);              //注意，新节点在顶层，保证正确的结合性
                            node.addChild(child2);
                            child1 = node;
                        }
                        else
                        {
                            throw new Exception("invalid additive expression, expecting the right part.");
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return node;
        }

        /**
    * 乘法表达式
    * @return
    * @throws Exception
    * M => int | int star M
    */
        private SimpleASTNode multiplicative(TokenReader tokens)
        {
            SimpleASTNode child1 = primary(tokens); //消耗一个token
            SimpleASTNode node = child1;

            while (true)  //循环调用 multiplicative规则
            {
                Token token = tokens.peek();
                if (token != null && (token.getType() == TokenType.Star || token.getType() == TokenType.Slash))
                {
                    token = tokens.read(); //消耗 * 或 /
                    SimpleASTNode child2 = primary(tokens);
                    if (child2 != null)
                    {
                        node = new SimpleASTNode(ASTNodeType.Multiplicative, token.getText());
                        node.addChild(child1);
                        node.addChild(child2);
                        child1 = node;
                    }
                    else
                    {
                        throw new Exception("invalid multiplicative expression, expecting the right part.");
                    }
                }
                else
                {
                    break;
                }
            }

            return node;
        }

        /**
 * 打印输出AST的树状结构
 * @param node
 * @param indent 缩进字符，由tab组成，每一级多一个tab
 */
       public  void DumpAST(ASTNode node, String indent)
        {
            Console.WriteLine(indent + node.getType() + " " + node.getText());
            node.getChildren().ForEach(item => DumpAST(item, indent + "\t"));


        }
    }
}
