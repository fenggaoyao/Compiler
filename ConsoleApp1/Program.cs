using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleCalculator calculator = new SimpleCalculator();
            //测试表达式
            string script = "2+3*5";
            Console.WriteLine("\n计算: " + script + "，看上去一切正常。");
            calculator.evaluate(script);

            //测试语法错误
            script = "(1+2)*8";
            Console.WriteLine("\n: " + script );
            calculator.evaluate(script);

            script = "2+3+4";
            Console.WriteLine("\n计算: " + script + "，结合性出现错误。");
            calculator.evaluate(script);



        }
        /// <summary>
        /// 词法解析
        /// </summary>
        void TestSimpleLexer()
        {
            SimpleLexer lexer = new SimpleLexer();

            String script = "int age = 45;";
            Console.WriteLine("parse: " + script);
            SimpleTokenReader tokenReader = lexer.Tokenize(script);
            SimpleLexer.Dump(tokenReader);

            //测试inta的解析
            script = "inta age == 45;";
            Console.WriteLine("\nparse :" + script);
            tokenReader = lexer.Tokenize(script);
            SimpleLexer.Dump(tokenReader);

            //测试in的解析
            script = "in age = 45;";
            Console.WriteLine("\nparse :" + script);
            tokenReader = lexer.Tokenize(script);
            SimpleLexer.Dump(tokenReader);

            //测试>=的解析
            script = "age >= 45;";
            Console.WriteLine("\nparse :" + script);
            tokenReader = lexer.Tokenize(script);
            SimpleLexer.Dump(tokenReader);

            //测试>的解析
            script = "age > 45;";
            Console.WriteLine("\nparse :" + script);
            tokenReader = lexer.Tokenize(script);
            SimpleLexer.Dump(tokenReader);
        }

        void Parser()
        {
            SimpleParser parser = new SimpleParser();
            String script = null;
            ASTNode tree = null;
            try
            {
                script = "2;int age =3*(45+2); age= 20*2+2; age+10*2;";
                Console.WriteLine("解析：" + script);
                tree = parser.parse(script);
                parser.DumpAST(tree, "");
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

            //测试异常语法
            try
            {
                script = "2+3+;";
                Console.WriteLine("解析：" + script);
                tree = parser.parse(script);
                parser.DumpAST(tree, "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //测试异常语法
            try
            {
                script = "2+3*;";
                Console.WriteLine("解析：" + script);
                tree = parser.parse(script);
                parser.DumpAST(tree, "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
