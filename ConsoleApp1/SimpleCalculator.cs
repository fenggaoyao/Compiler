using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class SimpleCalculator
    {
        public SimpleCalculator()
        {

        }


        /**
  * 执行脚本，并打印输出AST和求值过程。
  * @param script
  */
        public void evaluate(String script)
        {
            try
            {
                SimpleLexer lexer = new SimpleLexer();
                TokenReader tokens = lexer.Tokenize(script);

                ASTNode tree = Prog(tokens);

                DumpAST(tree, "");
                evaluate(tree, "");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        /**
     * 语法解析：根节点
     * @return
     * @throws Exception
     */
        public SimpleASTNode Prog(TokenReader tokens)
        {
            SimpleASTNode node = new SimpleASTNode(ASTNodeType.Programm, "Calculator");
            SimpleParser parse = new SimpleParser();
            SimpleASTNode child = parse.additive(tokens);

            if (child != null) {
                node.addChild(child);
            }
            return node;
        }


        /**
       * 对某个AST节点求值，并打印求值过程。
       * @param node
       * @param indent  打印输出时的缩进量，用tab控制
       * @return
       */
        private int evaluate(ASTNode node, String indent)
        {
            int result = 0;
            Console.WriteLine(indent + "Calculating: " + node.getType());
            switch (node.getType())
            {
                case ASTNodeType.Programm:
                    node.getChildren().ForEach(item => result = evaluate(item, indent + "\t"));
                    break;
                case ASTNodeType.Additive:
                    ASTNode child1 = node.getChildren()[0];
                    int value1 = evaluate(child1, indent + "\t");
                    ASTNode child2 = node.getChildren()[1];
                    int value2 = evaluate(child2, indent + "\t");
                    if (node.getText().Equals("+"))
                    {
                        result = value1 + value2;
                    }
                    else
                    {
                        result = value1 - value2;
                    }
                    break;
                case ASTNodeType.Multiplicative:
                    child1 = node.getChildren()[0];
                    value1 = evaluate(child1, indent + "\t");
                    child2 = node.getChildren()[1];
                    value2 = evaluate(child2, indent + "\t");
                    if (node.getText().Equals("*"))
                    {
                        result = value1 * value2;
                    }
                    else
                    {
                        result = value1 / value2;
                    }
                    break;
                case ASTNodeType.IntLiteral:
                    result = Convert.ToInt32(node.getText());
                    break;
                default:
                    break;
            }
            Console.WriteLine(indent + "Result: " + result);
            return result;
        }

        /**
* 打印输出AST的树状结构
* @param node
* @param indent 缩进字符，由tab组成，每一级多一个tab
*/
        public void DumpAST(ASTNode node, String indent)
        {
            Console.WriteLine(indent + node.getType() + " " + node.getText());
            node.getChildren().ForEach(item => DumpAST(item, indent + "\t"));


        }

    }
}
