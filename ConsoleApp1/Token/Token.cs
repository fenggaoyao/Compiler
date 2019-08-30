using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// 一个简单的Token。
    /// 只有类型和文本值两个属性。
    /// </summary>        
    public interface Token
    {

        /**
         * Token的类型
         * @return
         */
        TokenType getType();

        /**
         * Token的文本值
         * @return
         */
        String getText();

    }
}
