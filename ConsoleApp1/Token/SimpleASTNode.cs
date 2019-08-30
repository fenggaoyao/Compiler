using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ConsoleApp1
{
   public class SimpleASTNode: ASTNode
    {
        SimpleASTNode parent = null;
        List<ASTNode> children = new List<ASTNode>();   
        ASTNodeType nodeType ;
        String text = null;

        public SimpleASTNode(ASTNodeType nodeType, String text)
        {
            this.nodeType = nodeType;
            this.text = text;
        }

        public ASTNode getParent()
        {
            return parent;
        }
        
        public List<ASTNode> getChildren()
        {
            return children;
        }

        
        public ASTNodeType getType()
        {
            return nodeType;
        }

        
        public String getText()
        {
            return text;
        }

        public void addChild(SimpleASTNode child)
        {
            children.Add(child);
            child.parent = this;
        }

    }
}
