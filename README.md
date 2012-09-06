LogParserCSharp
===============

How to use?
    public class LogParserHelper
    {
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="query">查询语句</param>
        /// <returns></returns>
        public static object ExecuteScalar(string query)
        {
            using (LogParserConnection conn = new LogParserConnection())
            {
                conn.Open();
                using (LogParserCommand cmd = new LogParserCommand(query, conn))
                {
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 返回DataReader对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(string query)
        {
            using (LogParserConnection conn = new LogParserConnection())
            {
                conn.Open();
                using (LogParserCommand cmd = new LogParserCommand(query, conn))
                {
                    return cmd.ExecuteReader();
                }
            }
        }
        /// <summary>
        /// 返回DataSet数据集
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string query)
        {
            using (LogParserConnection conn = new LogParserConnection())
            {
                conn.Open();
                using (LogParserCommand cmd = new LogParserCommand(query, conn))
                {
                    using (LogParserDataAdapter ad = new LogParserDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        ad.Fill(ds);
                        if (ds.Tables.Count > 0)
                            return ds;
                        else
                            return null;
                    }
                }
            }
        }
    }


[TestMethod]
public void TestMethod_DataReader()
        {
            //COUNT(DISTINCT c-ip)

            using (IDataReader dr = LogParserHelper.ExecuteReader(@"select top 10 c-ip, count(c-ip) as ip_count from F:\iislog\u_ex120606.log GROUP BY c-ip order by ip_count desc"))
            {
                while (dr.Read())
                {
                    Console.WriteLine(dr["c-ip"] + "\t" + dr["ip_count"]);
                }

            }
        }