using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.SqlClient.SQLite
{
    /// <summary>
    /// 支持事务的SQLite类
    /// </summary>
    public class SQLiteDbca
    {
        #region 参数
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string dbconnStr = SQLiteHelper.dbConnection;
        /// <summary>
        /// 执行sql错误提示 无错误时返回string.Empty
        /// </summary>
        public string SqlErr = string.Empty;
        /// <summary>
        /// 命令对象
        /// </summary>
        internal SQLiteCommand cmd;
        /// <summary>
        /// 连接对象
        /// </summary>
        internal SQLiteConnection conn;
        /// <summary>
        /// 事务对象
        /// </summary>
        internal SQLiteTransaction tran;
        /// <summary>
        /// 执行次数
        /// </summary>
        internal int ExeNum = 0;
        #endregion
        /// <summary>
        /// 构造方法
        /// </summary>
        public SQLiteDbca()
        {
            try
            {
                conn = new SQLiteConnection(dbconnStr);
                conn.Open();
                cmd = new SQLiteCommand();
                tran = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = tran;
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SQLiteLog.WriteLine("dbca", ex.Message, "dbconnStr:" + dbconnStr, "");
            }
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="as_connStr">连接字符串</param>
        public SQLiteDbca(string as_connStr)
        {
            try
            {
                this.dbconnStr = as_connStr;
                conn = new SQLiteConnection(dbconnStr);
                conn.Open();
                cmd = new SQLiteCommand();
                tran = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = tran;
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SQLiteLog.WriteLine("dbca", ex.Message, "dbconnStr:" + dbconnStr, "");
            }
        }
        /// <summary>
        /// 返回查询字符串第一个匹配项
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns>返回对应值</returns>
        internal object ExecuteScalarObj(string sql, params string[] as_param)
        {
            SqlErr = string.Empty;
            ExeNum++;
            try
            {
                cmd.Parameters.Clear();
                SQLiteParameter[] P = SQLiteHelper.GetSQLiteParameter(as_param);
                if (P != null)
                    cmd.Parameters.AddRange(P);
                object o = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return o;
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SQLiteLog.WriteLine("dbca", SqlErr, sql, as_param);
                return null;
            }
        }
        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns>返回受影响行</returns>
        public int ExecuteNonQuery(string sql, params string[] as_param)
        {
            SqlErr = string.Empty;
            ExeNum++;
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                SQLiteParameter[] P = SQLiteHelper.GetSQLiteParameter(as_param);
                if (P != null)
                    cmd.Parameters.AddRange(P);
                int rows = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return rows;
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SQLiteLog.WriteLine("dbca", SqlErr, sql, as_param);
                return -1;
            }
        }
        /// <summary>
        /// 返回查询字符串第一个匹配项
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns>返回对应值,失败返回null</returns>
        public string ExecuteScalar(string sql, params string[] as_param)
        {
            return ExecuteScalarObj(sql, as_param) as string;
        }
        /// <summary>
        /// 返回查询结果的Int32对象
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns>对应的Int32对象,失败返回-1</returns>
        public int ExecuteScalarNum(string sql, params string[] as_param)
        {
            string ls_rc = ExecuteScalarObj(sql, as_param) as string;
            if (ls_rc == null)
            {
                return -1;
            }
            try { return Convert.ToInt32(ls_rc); }
            catch { return -1; }
        }
        /// <summary>
        /// 返回查询结果的DataTable对象
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns>对应的Int32对象,失败返回-1</returns>
        public DataTable ExecuteScalarDataTable(string sql, params string[] as_param)
        {

            SqlErr = string.Empty;
            ExeNum++;
            DataTable ldt = null;
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                SQLiteParameter[] P = SQLiteHelper.GetSQLiteParameter(as_param);
                if (P != null)
                    cmd.Parameters.AddRange(P);
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd))
                {
                    sda.Fill(ldt);
                }
                cmd.Parameters.Clear();
                return ldt;
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SQLiteLog.WriteLine("dbca", SqlErr, sql, as_param);
                return null;
            }
        }
        /// <summary>
        /// 事务提交
        /// </summary>
        public void Commit()
        {
            if (ExeNum > 0)
            {
                tran.Commit();
                ExeNum = 0;
                tran = conn.BeginTransaction();
            }
        }
        /// <summary>
        /// 事务回滚
        /// </summary>
        public void RollBack()
        {
            if (ExeNum > 0)
            {
                tran.Rollback();
                ExeNum = 0;
                tran = conn.BeginTransaction();
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn = null;
            cmd = null;
            tran = null;
        }
        /// <summary>
        /// 事务提交并关闭连接
        /// </summary>
        public void CommitAndClose()
        {
            Commit();
            Close();
        }
        /// <summary>
        /// 事务回滚并关闭连接
        /// </summary>
        public void RollBackAndClose()
        {
            RollBack();
            Close();
        }
    }
}
