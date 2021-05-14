using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAttestati
{
    /// <summary>
    /// Sqlite
    /// </summary>
    public class SqLite
    {
        public static SQLiteConnection dBConnection;

        /// <summary>
        /// Open Db
        /// </summary>
        /// <param name="filePathDB"></param>
        public void OpenDB(string filePathDB)
        {
            try
            {
                dBConnection = new SQLiteConnection($"Data Source={filePathDB}; Version=3; UseUTF16Encoding=True;");
                dBConnection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Close Db
        /// </summary>
        public void CloseDB()
        {
            try
            {
                dBConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create Db
        /// </summary>
        /// <param name="filePathDB"></param>
        public void CreateDB(string filePathDB)
        {
            try
            {
                SQLiteConnection.CreateFile(filePathDB);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Execute Command
        /// </summary>
        /// <param name="sql"></param>
        public void ExecuteCommand(string sql)
        {
            try
            {
                var command = new SQLiteCommand(sql, dBConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Execute Select
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="fieldCount"></param>
        /// <returns></returns>
        public DataTable ExecuteSelect(string sql, dynamic fieldCount = null)
        {
            var rows = new DataTable();
            try
            {
                var nameField = String.Empty;
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
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rows;
        }
    }
}
