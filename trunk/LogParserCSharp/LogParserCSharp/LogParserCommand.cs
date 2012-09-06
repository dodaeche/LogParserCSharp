#region Version Info
/* ========================================================================
* 【本类功能概述】
*
* 作者：shenjk 时间：2012/9/5 14:20:17
* 文件名：LogParserCommand
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
using LogQuery = MSUtil.LogQueryClass;
using EventLogInputFormat = MSUtil.COMIISW3CInputContextClass;
using LogRecordSet = MSUtil.ILogRecordset;

namespace System.Data.LogParserClient
{
    public sealed class LogParserCommand : DbCommand, ICloneable
    {
        private string cmdText;
        private LogParserConnection connection;

        public LogParserCommand()
        {
        }
        public LogParserCommand(string cmdText, LogParserConnection connection)
        {
            this.cmdText = cmdText;
            this.connection = connection;
        }

        public override void Cancel()
        {
            throw new NotSupportedException("Cancel");
        }

        public override string CommandText
        {
            get
            {
                return this.cmdText;
            }
            set
            {
                this.cmdText = value;
            }
        }

        public override int CommandTimeout
        {
            get;
            set;
        }

        public override System.Data.CommandType CommandType
        {
            get;
            set;
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotSupportedException();
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = (LogParserConnection)value;
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { throw new NotSupportedException(); }
        }

        protected override DbTransaction DbTransaction
        {
            get;
            set;
        }

        public override bool DesignTimeVisible
        {
            get;
            set;
        }

        protected override DbDataReader ExecuteDbDataReader(System.Data.CommandBehavior behavior)
        {
            checkCmd();
            try
            {
                LogRecordSet oRecordSet = this.connection.Query.Execute(this.cmdText, this.connection.LogFormat);            
                return new LogParserDataReader(oRecordSet);
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw exc;
            }
        }

        public override int ExecuteNonQuery()
        {
            throw new NotSupportedException("ExecuteNonQuery");
        }

        public override object ExecuteScalar()
        {
            checkCmd();
            try
            {
                LogRecordSet oRecordSet = this.connection.Query.Execute(this.cmdText, this.connection.LogFormat);
                dynamic ret = oRecordSet.getRecord().getValue(0);
                oRecordSet.close();
                return ret;

            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw exc;
            }
        }

        public override void Prepare()
        {
        }

        public override System.Data.UpdateRowSource UpdatedRowSource
        {
            get;
            set;
        }

        /// <summary>
        /// 检查语法合法性
        /// </summary>
        private void checkCmd()
        {
            if (string.IsNullOrEmpty(cmdText))
                throw new ArgumentNullException("cmdText");
        }

        public object Clone()
        {
            return new LogParserCommand(cmdText, connection);
        }
    }
}
