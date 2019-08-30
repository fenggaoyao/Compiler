using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// 一个Token流。由Lexer生成。Parser可以从中获取Token。
    /// </summary>
    public interface TokenReader
    {
        /**
    * 返回Token流中下一个Token，并从流中取出。 如果流已经为空，返回null;
    */
         Token read();

     
        /// <summary>
        /// 返回Token流中下一个Token，但不从流中取出。 如果流已经为空，返回null;
        /// </summary>
        Token peek();

        /**
         * Token流回退一步。恢复原来的Token。
         */
         void unread();

        /**
         * 获取Token流当前的读取位置。
         * @return
         */
         int getPosition();

        /**
         * 设置Token流当前的读取位置
         * @param position
         */
         void setPosition(int position);
    }
}
