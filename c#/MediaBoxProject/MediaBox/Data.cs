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

namespace MediaBox
{
    public class Data
    {
        public Models.DB GetData(Form form)
        {
            Models.DB db = new Models.DB(){ };
            try
            {
                DataTable rows = new DataTable();
                DataTable typeFields = new DataTable();
                string nameField = String.Empty;
                int nRow = 0;
                var value = (dynamic)null;
                int fieldCount = 0;
                string sql = "";
                SQLiteCommand command = null;
                SQLiteDataReader reader = null;
                DataRow fields = null;
                DataRow typeField = null;
                DataColumn column = null;
                Common common = new Common();
                Utility utility = new Utility();
                sql = "SELECT ID, WidthPanel, HeightPanel, CountElements FROM Configuration;";
                command = new SQLiteCommand(sql, SqLite.dBConnection);
                reader = command.ExecuteReader();
                reader.Read();
                int id = 0;
                int widthPanel = 0;
                int heightPanel = 0;
                int countElements = 0;
                try
                {
                    id = Convert.ToInt32(reader.GetValue(0));
                    widthPanel = Convert.ToInt32(reader.GetValue(1));
                    heightPanel = Convert.ToInt32(reader.GetValue(2));
                    countElements = Convert.ToInt32(reader.GetValue(3));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                reader.Close();
                sql = "SELECT * FROM Catalog;";
                command = new SQLiteCommand(sql, SqLite.dBConnection);
                reader = command.ExecuteReader();
                fieldCount = reader.FieldCount;
                if (countElements > 0 && reader.HasRows) common.ProgressBar(countElements, utility.GetCurrentMethod().Name,form);
                while (reader.Read())
                {
                    Application.DoEvents();
                    fields = rows.NewRow();
                    if (nRow == 0)
                    {
                        typeField = typeFields.NewRow();
                    }
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
                            column = new DataColumn(nameField);
                            column.DataType = typeof(Type);
                            typeFields.Columns.Add(column);
                        }
                        value = reader.GetValue(nField);
                        if (nRow == 0)
                        {
                            typeField[nameField] = type;
                        }
                        fields[nameField] = value;
                    }
                    rows.Rows.Add(fields);
                    if (nRow == 0)
                    {
                        typeFields.Rows.Add(typeField);
                    }
                    nRow++;
                    common.ProgressBar(countElements, utility.GetCurrentMethod().Name, form);
                }
                reader.Close();
                db = new Models.DB
                {
                    TblCatalog = new Models.TblCatalog { Data = rows, TypeFields = typeFields },
                    TblConfiguration = new Models.TblConfiguration { ID = id,  HeightPanel = heightPanel, WidthPanel = widthPanel, CountElements = countElements }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //throw;
            }
            return db;
        }

        public void PopolateCatalogInDataBase(string tableName, List<dynamic> rows, Form form)
        {
            try
            {
                Common common = new Common();
                Utility utility = new Utility();
                string sql = String.Empty;
                string values_tmp = String.Empty;
                string fields_tmp = String.Empty;
                PropertyInfo[] props = null;
                Type type = null;
                Type typeOfValue = null;
                string nameOfField = String.Empty;
                Byte[] valueData = null;
                int countRows = rows.Count;
                if (countRows > 0) common.ProgressBar(countRows, utility.GetCurrentMethod().Name, form);
                for (int y = 0; y < countRows; y++)
                {
                    Application.DoEvents();
                    values_tmp = String.Empty;
                    fields_tmp = String.Empty;

                    type = rows[y].GetType();
                    props = type.GetProperties();

                    for (int i = 1; i < props.Length; i++)
                    {
                        Application.DoEvents();
                        var value = props[i].GetValue(rows[y]);
                        if (value == null) continue;
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

                    sql = $"INSERT INTO {tableName} ({fields_tmp}) VALUES ({values_tmp}); ";

                    SQLiteCommand command = new SQLiteCommand(sql, SqLite.dBConnection);
                    command.Parameters.Add("@valueData", DbType.Binary, valueData.Length).Value = valueData;
                    command.ExecuteNonQuery();
                    common.ProgressBar(countRows, utility.GetCurrentMethod().Name, form);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //throw;
            }
        }
    }
}
