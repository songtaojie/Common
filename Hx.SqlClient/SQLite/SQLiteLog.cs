using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hx.SqlClient.SQLite
{
    /// <summary>
    /// 日志类(内部使用)
    /// </summary>
    internal class SQLiteLog
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strAction">标题</param>
        /// <param name="strText">内容</param>
        /// <param name="as_sql">语句</param>
        /// <param name="as_param">参数</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static void WriteLine(string strAction, string strText, string as_sql, params string[] as_param)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileFullPath = path + "sqlitedberror.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Action :" + strAction + "\r\n");
            str.Append("Time   :  " + DateTime.Now.ToString("HH:mm:ss.fff") + "\r\n");
            str.Append("Message:" + strText + "\r\n");
            str.Append("Sql:" + as_sql + "\r\n");
            if (as_param != null)
            {
                if (as_param.Length > 0)
                {
                    string ls_param = string.Empty;
                    foreach (string _param in as_param)
                    {
                        ls_param += _param + " ";
                    }
                    str.Append("Param:" + ls_param + "\r\n");
                }
            }
            str.Append("-----------------------" + "\r\n");
            StreamWriter sw = default(StreamWriter);
            if (!File.Exists(fileFullPath))
                sw = File.CreateText(fileFullPath);
            else
                sw = File.AppendText(fileFullPath);
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strAction">标题</param>
        /// <param name="strText">内容</param>
        /// <param name="as_sql">文件夹</param>
        /// <param name="as_params">参数</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static void WriteLine(string strAction, string strText, string as_sql, SQLiteParameter[] as_params)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileFullPath = path + "sqlitedberror.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Action :" + strAction);
            str.Append("Time   :" + DateTime.Now.ToString("HH:mm:ss.fff") + "\r\n");
            str.Append("Message:" + strText + "\r\n");
            str.Append("Sql:" + as_sql + "\r\n");
            if (as_params != null)
            {
                if (as_params.Length > 0)
                {
                    string ls_param = string.Empty;
                    for (int i = 0; i < as_params.Length; i++)
                    {
                        SQLiteParameter _param = as_params[i];
                        ls_param += _param.ParameterName + "=" + _param.Value + " ";
                    }
                    str.Append("Param:" + ls_param + "\r\n");
                }
            }
            str.Append("-----------------------" + "\r\n");
            StreamWriter sw = default(StreamWriter);
            if (!File.Exists(fileFullPath))
                sw = File.CreateText(fileFullPath);
            else
                sw = File.AppendText(fileFullPath);
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strAction">标题</param>
        /// <param name="strText">内容</param>
        /// <param name="as_Path">路径</param>
        /// <param name="FolderName">文件夹</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static void Write(string strAction, string strText, string as_Path, string FolderName = "Log")
        {
            string path = as_Path + "\\" + FolderName + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileFullPath = path + "sqlitedberror.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Action :" + strAction);
            str.Append("Time   :" + DateTime.Now.ToString("HH:mm:ss.fff") + "\r\n");
            str.Append("Message:" + strText + "\r\n");
            str.Append("-----------------------" + "\r\n");
            StreamWriter sw = default(StreamWriter);
            if (!File.Exists(fileFullPath))
                sw = File.CreateText(fileFullPath);
            else
                sw = File.AppendText(fileFullPath);
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strAction">标题</param>
        /// <param name="strText">内容</param>
        /// <param name="FolderName">文件夹</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static void Write(string strAction, string strText, string FolderName = "Log")
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\" + FolderName + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileFullPath = path + "sqlitedberror.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Action :" + strAction);
            str.Append("Time   :" + DateTime.Now.ToString("HH:mm:ss.fff") + "\r\n");
            str.Append("Message:" + strText + "\r\n");
            str.Append("-----------------------" + "\r\n");
            StreamWriter sw = default(StreamWriter);
            if (!File.Exists(fileFullPath))
                sw = File.CreateText(fileFullPath);
            else
                sw = File.AppendText(fileFullPath);
            sw.WriteLine(str.ToString());
            sw.Close();
        }
    }
}
