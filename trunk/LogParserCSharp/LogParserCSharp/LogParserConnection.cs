#region Version Info
/* ========================================================================
* 【本类功能概述】
*
* 作者：shenjk 时间：2012/9/5 14:11:35
* 文件名：LogParserConnection
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
using IISLogInputFormat = MSUtil.COMIISW3CInputContextClass;
using LogRecordSet = MSUtil.ILogRecordset;

namespace System.Data.LogParserClient
{
    /// <summary>
    /// IIS Log Connection
    /// </summary>
    public sealed class LogParserConnection : DbConnection, ICloneable
    {
        LogQuery query;
        IISLogInputFormat connection;
        System.Data.ConnectionState state;

        internal LogQuery Query {
            get {
                if (state != System.Data.ConnectionState.Open)
                    throw new Exception("this conntion have not been opened");
                return this.query;
            }
        }

        internal IISLogInputFormat LogFormat
        {
            get
            {
                if (state != System.Data.ConnectionState.Open)
                    throw new Exception("this conntion have not been opened");
                return this.connection;
            }
        }

        public LogParserConnection()
        {

        }

        protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
        {
            throw new NotSupportedException();
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotSupportedException();
        }

        public override void Close()
        {
            state = System.Data.ConnectionState.Closed;
        }

        public override string ConnectionString
        {
            get
            ;
            set
            ;
        }

        protected override DbCommand CreateDbCommand()
        {
            throw new NotImplementedException();
        }

        public override string DataSource
        {
            get { throw new NotSupportedException(); }
        }

        public override string Database
        {
            get { throw new NotSupportedException(); }
        }

        public override void Open()
        {
            if (state != System.Data.ConnectionState.Open)
            {
                this.query = new LogQuery();
                this.connection = new IISLogInputFormat();
            }
            state = System.Data.ConnectionState.Open;
        }

        public override string ServerVersion
        {
            get { return "0.1"; }
        }

        public override System.Data.ConnectionState State
        {
            get { return state; }
        }

        public object Clone()
        {
            return new LogParserConnection();
        }

        
    }
}
