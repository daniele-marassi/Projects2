using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Data;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using Additional;

namespace Additional
{
    public class SqLite
    {
        public static SQLiteConnection dBConnection;
        public void OpenDB(string filePathDB)
        {
            try
            {
                dBConnection = new SQLiteConnection($"Data Source={filePathDB}; Version=3; UseUTF16Encoding=True;");
                dBConnection.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void CloseDB()
        {
            try
            {
                dBConnection.Close();
            }
            catch (Exception)
            { 
                throw;
            }
        }
        public void CreateDB(string filePathDB)
        {
            try
            {
                SQLiteConnection.CreateFile(filePathDB);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void ExecuteCommand(string sql)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, dBConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable ExecuteSelect(string sql, dynamic fieldCount = null)
        {

            try
            {
                DataTable rows = new DataTable();
                string nameField = String.Empty;
                DataRow fields = null;
                DataColumn column = null;
                SQLiteDataReader reader = null;
                SQLiteCommand command = null;
                var value = (dynamic)null;
                command = new SQLiteCommand(sql, dBConnection);
                reader = command.ExecuteReader();
                if (fieldCount == null) { fieldCount = reader.FieldCount; }
                while (reader.Read())
                {
                    Application.DoEvents();
                    fields = rows.NewRow();
                    for (int nField = 0; nField < fieldCount; nField++)
                    {
                        Application.DoEvents();
                        nameField = reader.GetName(nField).ToString();
                        Type type = reader.GetFieldType(nField);
                        if (!rows.Columns.Contains(nameField))
                        {
                            column = new DataColumn(nameField);
                            column.DataType = type;
                            rows.Columns.Add(column);
                        }
                        value = reader.GetValue(nField);
                        fields[nameField] = value;
                    }
                    rows.Rows.Add(fields);
                }
                reader.Close();
                return rows;
            }
            catch (Exception)
            {
                throw;
            }   
        }

        public string CreateInsertSql(string tableName, List<dynamic> rows, bool withID, Form form)
        {
            try
            {
                Utility utility = new Utility();
                string sql = String.Empty;
                string values_tmp = String.Empty;
                string fields_tmp = String.Empty;
                PropertyInfo[] props = null;
                Type type = null;
                Type typeOfValue = null;
                Byte[] valueData = null;
                string nameOfField = String.Empty;
                int countRows = rows.Count;
                for (int y = 0; y < countRows; y++)
                {
                    Application.DoEvents();
                    values_tmp = String.Empty;
                    fields_tmp = String.Empty;

                    type = rows[y].GetType();
                    props = type.GetProperties();

                    for (int i = 0; i < props.Length; i++)
                    {
                        Application.DoEvents();
                        var value = props[i].GetValue(rows[y]);
                        nameOfField = props[i].Name;
                        typeOfValue = value.GetType();

                        if (typeOfValue == typeof(DateTime))
                        {
                            value = value.ToString("yyyy-MM-dd hh:mm:ss");
                        }

                        if (typeOfValue == typeof(DateTimeOffset))
                        {
                            value = value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK");
                        }

                        //if (withID == false && nameOfField.ToLower() == "id") { continue; }
                        if (withID == false && i == 0) { continue; }

                        if (fields_tmp != String.Empty) { fields_tmp += ", "; }
                        fields_tmp += $"[{nameOfField}]";

                        if (values_tmp != String.Empty) { values_tmp += ", "; }

                        if (typeOfValue == typeof(string) || typeOfValue == typeof(DateTime) || typeOfValue == typeof(DateTimeOffset))
                        { values_tmp += $"'{value.ToString()}'"; }
                        else if (typeOfValue != typeof(Byte[]))
                        {
                            values_tmp += $"{value.ToString().Replace(',', '.')}";
                        }

                        if (typeOfValue == typeof(Byte[]))
                        {
                            valueData = value;
                            values_tmp += "@valueData";
                        }
                    }
                    sql += $"INSERT INTO {tableName} ({fields_tmp}) VALUES ({values_tmp}); ";
                }
                return sql;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string CreateUpdateSql(string tableName, List<dynamic> rows, Form form, string whereCondition = null)
        {
            try
            {
                Utility utility = new Utility();
                string sql = String.Empty;
                string tmp = String.Empty;
                PropertyInfo[] props = null;
                Type type = null;
                Type typeOfValue = null;
                Byte[] valueData = null;
                string nameOfField = String.Empty;
                string tmpWhereCondition = String.Empty;
                int ID = 0;
                int countRows = rows.Count;
                for (int y = 0; y < countRows; y++)
                {
                    Application.DoEvents();
                    tmp = String.Empty;
                    type = rows[y].GetType();
                    props = type.GetProperties();

                    for (int i = 0; i < props.Length; i++)
                    {
                        Application.DoEvents();
                        var value = props[i].GetValue(rows[y]);
                        nameOfField = props[i].Name;
                        typeOfValue = value.GetType();

                        if (typeOfValue == typeof(DateTime))
                        {
                            value = value.ToString("yyyy-MM-dd hh:mm:ss");
                        }

                        if (typeOfValue == typeof(DateTimeOffset))
                        {
                            value = value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK");
                        }

                        //if (nameOfField.ToLower() == "id") {ID = value; continue; }
                        if (i == 0) { ID = value; continue; }

                        if (tmp != String.Empty) { tmp += ", "; }
                        if (typeOfValue == typeof(string) || typeOfValue == typeof(DateTime) || typeOfValue == typeof(DateTimeOffset))
                        { tmp += $"[{nameOfField}] = '{value.ToString()}'"; }
                        else if (typeOfValue != typeof(Byte[])) { tmp += $"[{nameOfField}] = {value.ToString().Replace(',', '.')}"; }

                        if (typeOfValue == typeof(Byte[]))
                        {
                            valueData = value;
                            tmp += $"[{nameOfField}] = '{"@valueData"}'";
                        }

                    }
                    if (whereCondition == null) { tmpWhereCondition = $"ID = {ID}"; }
                    else { tmpWhereCondition = whereCondition; }
                    sql += $"UPDATE {tableName} SET {tmp} WHERE {tmpWhereCondition}; ";
                }
                return sql;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
