using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.SQLite;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SelectPdf;
using System.Drawing;
using System.Windows.Forms;

namespace GetAttestati
{
    public class WorkFlow
    {
        private static Logger classLogger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Execute Elaboration
        /// </summary>
        public void Execute()
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    var configurations = GetConfig();
                    var data = new DataTable();
                    var attestati = new List<Attestato>() { };
                    var localDbSourcePath = $"{System.Environment.CurrentDirectory}\\Data\\LocalDbSource.sqlite";
                    var localDbOutputPath = $"{System.Environment.CurrentDirectory}\\Data\\LocalDbOutput.sqlite";
                    var sql = String.Empty;
                    var offset = 0;
                    var startRowPosition = 0;

                    if (!configurations.LocalDb)
                    {
                        sql += "SELECT * FROM (";
                        sql += "SELECT ROW_NUMBER() OVER (ORDER BY data_esame,id_allievo,codfisc,id_classe,filename) AS RowNum, * FROM dbo.ESTRAZIONI_ATTESTATI ";

                        ////////////////////// solo per test
                        //sql += "WHERE id_allievo IN(  0  ,1  ,100  ,1000  ,10001  ,1001  ,10011  ,10014  ,10016  ,10017  ,10018  ,10019  ,1002  ,10020  ,10021  ,10023  ,10029  ,1003  ,10030  ,10032  ,10037  ,10404  ,11422  ,11968  ,12343  ,12767  ,16613  ,22256  ,2246  ,2252  ,2265  ,23010  ,23012  ,23015  ,23080  ,23379  ,23483  ,23493  ,23541  ,23548  ,23620  ,23623  ,23651  ,23712  ,23713  ,23720  ,23735  ,23759  ,23907  ,24088  ,24581  ,24686  ,24795  ,24838  ,25120  ,25175  ,27950  ,29339  ,29346  ,2953  ,29547  ,29615  ,2985  ,2994  ,30469  ,30953  ,30956  ,3139  ,3140  ,33385  ,34097  ,35483  ,35504  ,35518  ,35764  ,37223  ,4032  ,43082  ,43432  ,43824  ,46958  ,47546  ,47688  ,47911  ,47913  ,48018  ,48567  ,48941  ,49003  ,49117  ,49163  ,49177  ,49187  ,49218  ,49294  ,49301  ,49308  ,49312  ,49344  ,49346  ,49347  ,49352  ,49354  ,49358  ,49359  ,49361  ,49363  ,49367  ,49370  ,49371  ,49373  ,49374  ,49375  ,49376  ,49377  ,49381  ,49382  ,49385  ,49386  ,49387  ,49388  ,49390  ,49391  ,49392  ,49393  ,49395  ,49396  ,49397  ,49399  ,49401  ,49402  ,49403  ,49404  ,49405  ,49406  ,49407  ,49409  ,49411  ,49412  ,49413  ,49414  ,49415  ,49416  ,49417  ,49419  ,49420  ,49421  ,49423  ,49425  ,49426  ,49427  ,49428  ,49430  ,49431  ,49432  ,49434  ,49435  ,49436  ,49437  ,49438  ,49439  ,49440  ,49442  ,49443  ,49444  ,49445  ,49446  ,49449  ,49450  ,49451  ,49452  ,49453  ,49454  ,49457  ,49458  ,49459  ,49460  ,49464  ,49465  ,49467  ,49468  ,49471  ,49472  ,49474  ,49475  ,49476  ,49480  ,49481  ,49482  ,49484  ,49485  ,49486  ,49487  ,49488  ,49489  ,49491  ,49502  ,49503  ,49505  ,49506  ,49506  ,49508  ,49509  ,49511  ,49512  ,49513  ,49516  ,49518  ,49523  ,49524  ,49527  ,49528  ,49534  ,5216  ,553  ,7722  ,7931  ,9724) ";
                        startRowPosition = configurations.StartRowPosition;
                        if (startRowPosition == 0) startRowPosition = 1;

                        sql += $") a WHERE a.RowNum >= {startRowPosition} AND a.RowNum <= {configurations.QuantityRows + (startRowPosition - 1)} ";

                        sql += "ORDER BY data_esame,id_allievo,codfisc,id_classe,filename ASC; ";

                    }
                    else
                    {

                        sql += "SELECT * FROM ESTRAZIONI_ATTESTATI ";

                        //////////// solo per test
                        //sql += "WHERE id_allievo=1000 and id_classe=6828 ";
                        //sql += "WHERE id_allievo IN(  0  ,1  ,100  ,1000  ,10001  ,1001  ,10011  ,10014  ,10016  ,10017  ,10018  ,10019  ,1002  ,10020  ,10021  ,10023  ,10029  ,1003  ,10030  ,10032  ,10037  ,10404  ,11422  ,11968  ,12343  ,12767  ,16613  ,22256  ,2246  ,2252  ,2265  ,23010  ,23012  ,23015  ,23080  ,23379  ,23483  ,23493  ,23541  ,23548  ,23620  ,23623  ,23651  ,23712  ,23713  ,23720  ,23735  ,23759  ,23907  ,24088  ,24581  ,24686  ,24795  ,24838  ,25120  ,25175  ,27950  ,29339  ,29346  ,2953  ,29547  ,29615  ,2985  ,2994  ,30469  ,30953  ,30956  ,3139  ,3140  ,33385  ,34097  ,35483  ,35504  ,35518  ,35764  ,37223  ,4032  ,43082  ,43432  ,43824  ,46958  ,47546  ,47688  ,47911  ,47913  ,48018  ,48567  ,48941  ,49003  ,49117  ,49163  ,49177  ,49187  ,49218  ,49294  ,49301  ,49308  ,49312  ,49344  ,49346  ,49347  ,49352  ,49354  ,49358  ,49359  ,49361  ,49363  ,49367  ,49370  ,49371  ,49373  ,49374  ,49375  ,49376  ,49377  ,49381  ,49382  ,49385  ,49386  ,49387  ,49388  ,49390  ,49391  ,49392  ,49393  ,49395  ,49396  ,49397  ,49399  ,49401  ,49402  ,49403  ,49404  ,49405  ,49406  ,49407  ,49409  ,49411  ,49412  ,49413  ,49414  ,49415  ,49416  ,49417  ,49419  ,49420  ,49421  ,49423  ,49425  ,49426  ,49427  ,49428  ,49430  ,49431  ,49432  ,49434  ,49435  ,49436  ,49437  ,49438  ,49439  ,49440  ,49442  ,49443  ,49444  ,49445  ,49446  ,49449  ,49450  ,49451  ,49452  ,49453  ,49454  ,49457  ,49458  ,49459  ,49460  ,49464  ,49465  ,49467  ,49468  ,49471  ,49472  ,49474  ,49475  ,49476  ,49480  ,49481  ,49482  ,49484  ,49485  ,49486  ,49487  ,49488  ,49489  ,49491  ,49502  ,49503  ,49505  ,49506  ,49506  ,49508  ,49509  ,49511  ,49512  ,49513  ,49516  ,49518  ,49523  ,49524  ,49527  ,49528  ,49534  ,5216  ,553  ,7722  ,7931  ,9724) ";

                        sql += "ORDER BY data_esame,id_allievo,codfisc,id_classe,filename ASC ";

                        offset = configurations.StartRowPosition - 1;
                        if (offset < 0) offset = 0;

                        sql += $"LIMIT {configurations.QuantityRows} OFFSET {offset}; ";
                    }

                    if (!configurations.LocalDb || (!File.Exists(localDbSourcePath) && configurations.LocalDb))
                        data = GetData(false, configurations.ConnectionStringSepDb, null, sql);

                    if (!File.Exists(localDbSourcePath) && configurations.LocalDb)
                        CreateLocalDbSource(data, localDbSourcePath);

                    if (configurations.LocalDb && data.Rows.Count == 0)
                        data = GetData(true, null, localDbSourcePath, sql);

                    attestati = GetAttestatiAndConvertToPdf(data, configurations.SepPath, configurations.SepDataPath);

                    CreateLocalDbOutput(data, localDbOutputPath, attestati);

                    SaveConfig("StartRowPosition", (configurations.StartRowPosition + configurations.QuantityRows).ToString());

                    logger.Warn("Elaboration ENDED");
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }
                finally
                {
                    Console.ReadLine();
                }
            }
        }

        private List<Attestato> GetAttestatiAndConvertToPdf(DataTable data, string sepPath, string sepDataPath)
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                List<Attestato> result = new List<Attestato>() { };
                try
                {
                    var pathFileHtm = String.Empty;
                    var htm = String.Empty;
                    var start = 0;
                    var end = 0;
                    var acualtPathLinks = String.Empty;
                    SelectPdf.PdfDocument pdf = null;
                    MemoryStream msOutput = null;
                    byte[] blob = null;
                    var newPathLinks = sepPath;
                    var countElaborated = 0;
                    var converter = new HtmlToPdf();

                    foreach (DataRow row in data.Rows)
                    {
                        pathFileHtm = Path.Combine(sepDataPath, "datafile_allievo", row["id_allievo"].ToString(), row["filename"].ToString());
 
                        htm = String.Empty;
                        if (File.Exists(pathFileHtm))
                        {
                            try
                            {
                                htm = File.ReadAllText(pathFileHtm, Encoding.Default);
                                //htm = "<p style=" + (char)34 + "font-family:sans-serif; font-size:11px; margin-bottom:64px;" + (char)34 + "> Allianz S.p.A. Attestato E-learning </p>" + htm;
                                htm = htm.Replace("<head>", "<head><meta http-equiv=" + (char)34 + "content-type" + (char)34 + " content=" + (char)34 + "text/html; charset=windows-1252" + (char)34 + ">");
                                htm = htm.Replace("<HEAD>", "<head><meta http-equiv=" + (char)34 + "content-type" + (char)34 + " content=" + (char)34 + "text/html; charset=windows-1252" + (char)34 + ">");

                                if (newPathLinks.Substring(newPathLinks.Length - 1, 1) != @"\") newPathLinks += @"\";
                                newPathLinks = newPathLinks.Replace(@"\", "/");

                                start = htm.IndexOf(Convert.ToChar(34) + "http");

                                if (start != -1)
                                {
                                    end = htm.IndexOf("cgi-bin/", start);
                                    acualtPathLinks = htm.Substring(start, end - start);
                                    htm = htm.Replace(acualtPathLinks, Convert.ToChar(34) + newPathLinks);
                                    htm = htm.Replace(@"//", @"/");
                                }

                                if (start == -1)
                                    logger.Warn($"Not Found Links Path into file {pathFileHtm}");

                                converter = new HtmlToPdf();

                                converter.Options.PdfPageSize = PdfPageSize.Custom;
                                converter.Options.WebPageWidth = 640;
                                converter.Options.WebPageHeight = 1038;
                                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                                converter.Options.MarginTop = 55;
                                converter.Options.MarginBottom = 0;
                                converter.Options.MarginLeft = 55;
                                converter.Options.MarginRight = 55;

                                pdf = converter.ConvertHtmlString(htm);

                                msOutput = new MemoryStream();
                                pdf.Save(msOutput);
                                blob = msOutput.ToArray();

                                pdf.Close();

                                result.Add(new Attestato { Id_Allievo = Convert.ToInt32(row["id_allievo"].ToString()), FileName = row["filename"].ToString(), Pdf = blob });

                                countElaborated++;

                                logger.Info($"{countElaborated} elaborated on {data.Rows.Count}");
                            }
                            catch (Exception ex)
                            {
                                logger.Error($"file: {row["filename"].ToString()} - " + ex.ToString());
                            }
                        }
                        else { logger.Error($"Not Found Attestato - file: {pathFileHtm}"); }
                    }  
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
                return result;
            }
        }

        private void CreateLocalDbOutput(DataTable data, string localDbSourcePath, List<Attestato> attestati)
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    var sqLite = new SqLite();
                    Byte[] valueData = null;
                    DateTime data_esame = DateTime.MinValue;
                    var data_esame_str = String.Empty;
                    var sql = String.Empty;
                    var path = $"{System.Environment.CurrentDirectory}\\Data\\LocalDbOutput.sqlite";
                    var countInserted = 0;

                    DeleteAndCreatesLocalDb(path);

                    foreach (DataRow row in data.Rows)
                    {
                        try
                        {
                            data_esame = Convert.ToDateTime(row["data_esame"]);
                            data_esame_str = String.Format("{0:yyyy-MM-dd HH:mm:ss}", data_esame);

                            sql = $"INSERT INTO ESTRAZIONI_ATTESTATI (Gruppo, id_allievo ,codfisc, cognome ,nome ,nickname ,id_classe ,NomeClasse ,data_esame,filename, pdf) ";
                            sql += $"VALUES ('{row["Gruppo"].ToString().Replace("'", "''")}',{row["id_allievo"].ToString()}, '{row["codfisc"].ToString().Replace("'", "''")}', '{row["cognome"].ToString().Replace("'", "''")}', '{row["nome"].ToString().Replace("'", "''")}', '{row["nickname"].ToString().Replace("'", "''")}', {row["id_classe"].ToString()}, '{row["NomeClasse"].ToString().Replace("'", "''")}', '{data_esame_str}', '{row["filename"].ToString().Replace(".htm", ".pdf").Replace("'", "''")}', @valueData); ";

                            valueData = attestati.Where(_ => _.FileName == row["filename"].ToString() && _.Id_Allievo == Convert.ToInt32(row["id_allievo"].ToString())).FirstOrDefault()?.Pdf;
                            if (valueData == null) valueData = new Byte[0];

                            sqLite.OpenDB(path);
                            using (SQLiteCommand command = new SQLiteCommand(sql, SqLite.dBConnection))
                            {
                                command.Parameters.Add("@valueData", DbType.Binary, valueData.Length).Value = valueData;
                                command.ExecuteNonQuery();
                            }
                            sqLite.CloseDB();

                            countInserted++;

                            logger.Info($"{countInserted} inserted on {data.Rows.Count}");
                        }
                        catch (Exception ex)
                        {
                            logger.Error($"file: {row["filename"].ToString()} - " + ex.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void DeleteAndCreatesLocalDb( string pathLocalDb)
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    var sqLite = new SqLite();
                    var sql = String.Empty;

                    if (File.Exists(pathLocalDb))
                    {
                        logger.Info($"LocalDb already exists: {pathLocalDb}");
                    }
                    else
                    {
                        sqLite.CreateDB(pathLocalDb);
                        sqLite.OpenDB(pathLocalDb);
                        sql = System.IO.File.ReadAllText($"{System.Environment.CurrentDirectory}\\Data\\default_DB_NOT_EDIT_NOT_REMOVE.sql");
                        sqLite.ExecuteCommand(sql);
                        sqLite.CloseDB();
                        logger.Info($"LocalDb Created: {pathLocalDb}");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void CreateLocalDbSource(DataTable data, string localDbSourcePath)
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    var sqLite = new SqLite();
                    var data_esame = DateTime.MinValue;
                    var data_esame_str = String.Empty;
                    var sql = String.Empty;
                    var path = $"{System.Environment.CurrentDirectory}\\Data\\LocalDbSource.sqlite";
                    var countInserted= 0;

                    DeleteAndCreatesLocalDb(path);
                    
                    foreach (DataRow row in data.Rows)
                    {
                        try
                        {
                            data_esame = Convert.ToDateTime(row["data_esame"]);
                            data_esame_str = String.Format("{0:yyyy-MM-dd HH:mm:ss}", data_esame);
                            sql = $"INSERT INTO ESTRAZIONI_ATTESTATI (Gruppo, id_allievo ,codfisc, cognome ,nome ,nickname ,id_classe ,NomeClasse ,data_esame,filename, pdf) ";
                            sql += $"VALUES ('{row["Gruppo"].ToString().Replace("'", "''")}', {row["id_allievo"].ToString()}, '{row["codfisc"].ToString().Replace("'", "''")}', '{row["cognome"].ToString().Replace("'", "''")}', '{row["nome"].ToString().Replace("'", "''")}', '{row["nickname"].ToString().Replace("'", "''")}', {row["id_classe"].ToString()}, '{row["NomeClasse"].ToString().Replace("'", "''")}', '{data_esame_str}', '{row["filename"].ToString().Replace("'", "''")}', NULL); ";

                            sqLite.OpenDB(path);
                            sqLite.ExecuteCommand(sql);
                            sqLite.CloseDB();

                            countInserted++;
                            logger.Info($"{countInserted} inserted on {data.Rows.Count}");
                        }
                        catch (Exception ex)
                        {
                            logger.Error($"file: {row["filename"].ToString()} - " + ex.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private DataTable GetData(bool localDb, string connectionStringRemoteDb, string localDbSourcePath, string sql)
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                DataTable result = new DataTable();
                try
                {
                    if (!localDb)
                    {
                        logger.Info("Get Data from Remote Db");

                        var nameField = String.Empty;
                        DataRow fields = null;
                        DataColumn column = null;
                        var builder = new SqlConnectionStringBuilder();
                        Type type = null;
                        var fieldCount = 0;

                        builder.ConnectionString = connectionStringRemoteDb;

                        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                        {
                            connection.Open();

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.CommandTimeout = 0;
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    fieldCount = reader.FieldCount;
                                    while (reader.Read())
                                    {
                                        fields = result.NewRow();
                                        for (int nField = 0; nField < fieldCount; nField++)
                                        {
                                            nameField = reader.GetName(nField).ToString();
                                            type = reader.GetFieldType(nField);
                                            if (!result.Columns.Contains(nameField))
                                            {
                                                column = new DataColumn(nameField);
                                                column.DataType = type;
                                                result.Columns.Add(column);
                                            }
                                            fields[nameField] = reader.GetValue(nField);
                                        }
                                        result.Rows.Add(fields);
                                    }
                                }
                            }
                        }
                    }

                    if (localDb)
                    {
                        logger.Info("Get Data from Local Db");

                        SqLite sqLite = new SqLite();

                        sqLite.OpenDB(localDbSourcePath);
                        result = sqLite.ExecuteSelect(sql);
                        sqLite.CloseDB();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    logger.Info($"{result.Rows.Count} rows");
                }

                return result;
            }
        }

        private Configurations GetConfig()
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                var result = new Configurations() { };
                try
                {
                    var sepPath = ConfigurationManager.AppSettings["SepPath"].ToString();
                    var sepDataPath = ConfigurationManager.AppSettings["SepDataPath"].ToString();
                    var connectionStringSepDb = ConfigurationManager.AppSettings["ConnectionStringSepDb"].ToString();
                    var localDb = Convert.ToBoolean(ConfigurationManager.AppSettings["LocalDb"].ToString());
                    var startRowPosition = Convert.ToInt32(ConfigurationManager.AppSettings["StartRowPosition"].ToString());
                    var quantityRows = Convert.ToInt32(ConfigurationManager.AppSettings["QuantityRows"].ToString());

                    result.SepPath = sepPath;
                    result.ConnectionStringSepDb = connectionStringSepDb;
                    result.LocalDb = localDb;
                    result.SepDataPath = sepDataPath;
                    result.StartRowPosition = startRowPosition;
                    result.QuantityRows = quantityRows;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }
        }

        private void SaveConfig(string key, string value)
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    config.AppSettings.Settings[key].Value = value;
                    config.Save(ConfigurationSaveMode.Modified);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}