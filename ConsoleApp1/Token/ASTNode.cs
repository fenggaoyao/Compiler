using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// AST的节点。
    /// 属性包括AST的类型、文本值、下级子节点和父节点
    /// </summary>
    public interface ASTNode
    {
        //父节点
        ASTNode getParent();

        //子节点
        List<ASTNode> getChildren();

        //AST类型
        ASTNodeType getType();

        //文本值
        String getText();
    }

    public enum ASTNodeType {
        Programm,           //程序入口，根节点

        IntDeclaration,     //整型变量声明
        ExpressionStmt,     //表达式语句，即表达式后面跟个分号
        AssignmentStmt,     //赋值语句

        Primary,            //基础表达式
        Multiplicative,     //乘法表达式
        Additive,           //加法表达式

        Identifier,         //标识符
        IntLiteral          //整型字面量
    }
}
