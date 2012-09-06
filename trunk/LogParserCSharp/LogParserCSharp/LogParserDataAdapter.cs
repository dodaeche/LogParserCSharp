#region Version Info
/* ========================================================================
* 【本类功能概述】
*
* 作者：shenjk 时间：2012/9/5 18:05:40
* 文件名：LogParserDataAdapter
* 版本：V1.0.1
*
* 修改者： 时间：
* 修改说明：
* ========================================================================
*/
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace System.Data.LogParserClient
{
    /// <summary>
    /// LogParserDataAdapter
    /// </summary>
    public sealed class LogParserDataAdapter : DbDataAdapter, IDbDataAdapter, IDataAdapter, ICloneable
    {
        
        public LogParserDataAdapter()
        {           
            GC.SuppressFinalize(this);
        }

        public LogParserDataAdapter(LogParserCommand selectCommand)
            : this()
        {
            this.SelectCommand = selectCommand;
        }

    }
}
