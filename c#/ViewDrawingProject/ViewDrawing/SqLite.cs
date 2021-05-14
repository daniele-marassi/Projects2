using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewDrawing
{
    class SqLite
    {
        public static SQLiteConnection eViewDBConnection;
        public void OpenDB(string filePathDB)
        {
            try
            {
                eViewDBConnection = new SQLiteConnection($"Data Source={filePathDB}; Version=3; UseUTF16Encoding=True;");
                eViewDBConnection.Open();
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
                eViewDBConnection.Close();
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
                SQLiteCommand command = new SQLiteCommand(sql, eViewDBConnection);
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
                command = new SQLiteCommand(sql, eViewDBConnection);
                reader = command.ExecuteReader();
                if (fieldCount == null) { fieldCount = reader.FieldCount; }
                while (reader.Read())
                {
                    fields = rows.NewRow();
                    for (int nField = 0; nField < fieldCount; nField++)
                    {
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

        public string CreateInsertSql(string tableName, List<dynamic> rows, bool withID)
        {
            try
            {

                string sql = String.Empty;
                string values_tmp = String.Empty;
                string fields_tmp = String.Empty;
                PropertyInfo[] props = null;
                Type type = null;
                Type typeOfValue = null;
                string nameOfField = String.Empty;


                for (int y = 0; y < rows.Count; y++)
                {
                    values_tmp = String.Empty;
                    fields_tmp = String.Empty;

                    type = rows[y].GetType();
                    props = type.GetProperties();

                    for (int i = 0; i < props.Length; i++)
                    {
                        var value = props[i].GetValue(rows[y]);
                        nameOfField = props[i].Name;
                        typeOfValue = value.GetType();

                        if (typeOfValue == typeof(DateTime))
                        {
                            value = value.ToString("yyyy-MM-dd hh:mm:ss");
                        }

                        //if (withID == false && nameOfField.ToLower() == "id") { continue; }
                        if (withID == false && i == 0) { continue; }
                        if (fields_tmp != String.Empty) { fields_tmp += ", "; }
                        fields_tmp += $"[{nameOfField}]";

                        if (values_tmp != String.Empty) { values_tmp += ", "; }
                        if (typeOfValue == typeof(string) || typeOfValue == typeof(DateTime))
                        { values_tmp += $"'{value.ToString()}'"; }
                        else { values_tmp += $"{value.ToString()}"; }
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

        public string CreateUpdateSql(string tableName, List<dynamic> rows, string whereCondition = null)
        {
            try
            {
                string sql = String.Empty;
                string tmp = String.Empty;
                PropertyInfo[] props = null;
                Type type = null;
                Type typeOfValue = null;
                string nameOfField = String.Empty;
                string tmpWhereCondition = String.Empty;
                int ID = 0;

                for (int y = 0; y < rows.Count; y++)
                {
                    tmp = String.Empty;
                    type = rows[y].GetType();
                    props = type.GetProperties();

                    for (int i = 0; i < props.Length; i++)
                    {
                        var value = props[i].GetValue(rows[y]);
                        nameOfField = props[i].Name;
                        typeOfValue = value.GetType();

                        //if (nameOfField.ToLower() == "id") {ID = value; continue; }
                        if (i == 0) { ID = value; continue; }

                        if (tmp != String.Empty) { tmp += ", "; }
                        if (typeOfValue == typeof(string) || typeOfValue == typeof(DateTime))
                        { tmp += $"[{nameOfField}] = '{value.ToString()}'"; }
                        else { tmp += $"[{nameOfField}] = {value.ToString()}"; }
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
