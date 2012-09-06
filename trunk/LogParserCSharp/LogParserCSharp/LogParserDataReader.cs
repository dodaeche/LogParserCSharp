#region Version Info
/* ========================================================================
* 【本类功能概述】
*
* 作者：shenjk 时间：2012/9/5 14:08:10
* 文件名：LogParserDataReader
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
using System.Data;
using System.Data.Common;
using LogQuery = MSUtil.LogQueryClass;
using EventLogInputFormat = MSUtil.COMIISW3CInputContextClass;
using LogRecordSet = MSUtil.ILogRecordset;

namespace System.Data.LogParserClient
{
    /// <summary>
    /// 实现DataReader
    /// </summary>
    public sealed class LogParserDataReader : DbDataReader, IDataReader, IDisposable, IDataRecord
    {
        private LogRecordSet rs;
        bool hasRows;
        bool isclosed;
        int _recordsAffected;
        private FieldData[] values;
        internal LogParserDataReader(LogRecordSet rs)
        {
            this.rs = rs;
            hasRows = !rs.atEnd();
        }

        public override void Close()
        {
            if (rs != null)
                rs.close();
            isclosed = true;
        }

        public override int Depth
        {
            get { return 0; }
        }

        public override int FieldCount
        {
            get { return rs.getColumnCount(); }
        }

        public override bool GetBoolean(int ordinal)
        {
            return Convert.ToBoolean(GetValue(ordinal));
        }

        public override byte GetByte(int ordinal)
        {
            return Convert.ToByte(GetValue(ordinal));
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotSupportedException("GetBytes");
        }

        public override char GetChar(int ordinal)
        {
            return Convert.ToChar(GetValue(ordinal));
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotSupportedException("GetChars");
        }

        public override string GetDataTypeName(int ordinal)
        {
            return "string";
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return Convert.ToDateTime(GetValue(ordinal));
        }

        public override decimal GetDecimal(int ordinal)
        {
            return Convert.ToDecimal(GetValue(ordinal));
        }

        public override double GetDouble(int ordinal)
        {
            return Convert.ToDouble(GetValue(ordinal));
        }

        public override System.Collections.IEnumerator GetEnumerator()
        {
            return new DbEnumerator(this, true);
        }

        public override Type GetFieldType(int ordinal)
        {
            return typeof(string);
        }

        public override float GetFloat(int ordinal)
        {
            return (float)Convert.ToDecimal(GetValue(ordinal));
        }

        public override Guid GetGuid(int ordinal)
        {
            return new Guid(GetString(ordinal));
        }

        public override short GetInt16(int ordinal)
        {
            return Convert.ToInt16(GetValue(ordinal));
        }

        public override int GetInt32(int ordinal)
        {
            return Convert.ToInt32(GetValue(ordinal));
        }

        public override long GetInt64(int ordinal)
        {
            return Convert.ToInt64(GetValue(ordinal));
        }

        public override string GetName(int ordinal)
        {
            return rs.getColumnName(ordinal);
        }

        public override int GetOrdinal(string name)
        {
            if (string.IsNullOrEmpty(name))
                return -1;
            for (int i = 0; i < this.FieldCount; i++)
            {
                string s_name = rs.getColumnName(i);
                if (s_name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }

        DataTable dt;


        public override DataTable GetSchemaTable()
        {
            if (dt != null)
            {
                return dt;
            }
            dt = new DataTable();
            for (int i = 0; i < this.FieldCount; i++)
            {
                string s_name = rs.getColumnName(i);
                dt.Columns.Add(s_name, typeof(string));
            }
            return dt;
        }

        public override string GetString(int ordinal)
        {
            object o = GetValue(ordinal);
            if (o != null && o != DBNull.Value)
                return o.ToString();
            return null;
        }

        public override object GetValue(int ordinal)
        {
            if (ordinal >= this.FieldCount || ordinal < 0)
                throw new IndexOutOfRangeException("ordinal");
            return values[ordinal].Data;
        }

        public override int GetValues(object[] values)
        {

            int num3;
            try
            {
                if (values == null)
                {
                    throw new ArgumentNullException("values");
                }
                int num2 = (values.Length < this.values.Length) ? values.Length : this.values.Length;

                for (int i = 0; i < num2; i++)
                {
                    values[i] = GetValue(i);
                }

                num3 = num2;
            }
            finally
            {

            }
            return num3;

        }

        public override bool HasRows
        {
            get
            {
                return hasRows;
            }
        }

        public override bool IsClosed
        {
            get
            {
                return isclosed;
            }
        }

        public override bool IsDBNull(int ordinal)
        {
            if (ordinal >= this.FieldCount || ordinal < 0)
                throw new IndexOutOfRangeException("ordinal");
            return values[ordinal].IsNull;
        }

        public override bool NextResult()
        {
            return false;
        }

        public override bool Read()
        {
            if (!hasRows)
            {
                return false;
            }
            else
            {
                if (values != null)
                    values = null;
                values = new FieldData[this.FieldCount];
            }

            bool f = !rs.atEnd();
            if (f)
            {
                for (int i = 0; i < this.FieldCount; i++)
                {
                    FieldData fd = new FieldData();
                    fd.Data = rs.getRecord().getValue(i);
                    fd.IsNull = rs.getRecord().isNull(i);
                    values[i] = fd;
                }
                _recordsAffected++;
                rs.moveNext();
                return true;
            }
            return false;
        }

        public override int RecordsAffected
        {
            get { return _recordsAffected; }
        }

        public override object this[string name]
        {
            get
            {
                int idx = GetOrdinal(name);
                if (idx > -1 && idx < this.FieldCount)
                    return GetValue(idx);
                return null;
            }
        }

        public override object this[int ordinal]
        {
            get
            {
                return GetValue(ordinal);
            }
        }

        private struct FieldData
        {
            public object Data;

            public bool IsNull;
        }
    }
}
