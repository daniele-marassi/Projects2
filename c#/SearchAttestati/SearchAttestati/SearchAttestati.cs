using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SearchAttestati
{
    public partial class SearchAttestati : Form
    {
        private static Logger classLogger = LogManager.GetCurrentClassLogger();
        private static WaitFrm waitFrm = new WaitFrm();
        private static string message = String.Empty;
        private static bool waitStrated = false;
        private static MessagesFrm messagesFrm = null;

        public SearchAttestati()
        {
            InitializeComponent();
        }

        private DataTable InitToGetData(string value, string fullPathLocalDb)
        {
            DataTable result = new DataTable();
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    var configurations = GetConfig();
                    var sql = String.Empty;
                    var isNumeric = int.TryParse(value, out int n);
                    var isDate = false;
                    var date = DateTime.MinValue;
                    var dateString = date.ToString("yyyy-MM-dd");

                    if (value.Split('/').Count() == 3 || value.Split('-').Count() == 3) isDate = DateTime.TryParse(value, out date);

                    if (!configurations.LocalDb)
                        sql += "SELECT Gruppo, id_allievo AS 'Id Allievo', codfisc AS 'Codice Fiscale', cognome + ' ' + nome AS 'Utente', nickname AS 'Username', id_classe AS 'Id Classe', nomeClasse AS 'Nome Classe', data_esame AS 'Data Esame', filename AS 'Filename', Id FROM ESTRAZIONI_ATTESTATI ";
                    else
                        sql += "SELECT Gruppo, id_allievo AS `Id Allievo`, codfisc AS `Codice Fiscale`, cognome || ' ' || nome AS `Utente`, nickname AS `Username`, id_classe AS `Id Classe`, nomeClasse AS `Nome Classe`, data_esame AS `Data Esame`, filename AS `Filename`, Id FROM ESTRAZIONI_ATTESTATI ";

                    if (value != null && value != String.Empty)
                    {
                        sql += "WHERE";
                        sql += " ";

                        if (!isNumeric && !isDate)
                        {
                            sql += $"Gruppo LIKE '%{value.Trim().Replace(" ", "%")}%'";
                            sql += " OR ";
                            sql += $"codfisc = '{value.Trim()}'";
                            sql += " OR ";
                            sql += $"Utente LIKE '%{value.Trim().Replace(" ", "%")}%'";
                            sql += " OR ";
                            sql += $"nickname = '{value.Trim()}'";
                            sql += " OR ";
                            sql += $"NomeClasse LIKE '%{value.Trim().Replace(" ", "%")}%'";
                            sql += " OR ";
                            sql += $"data_esame LIKE '%{value.Trim().Replace(" ", "%").Replace(@"\", "-")}%'";
                            sql += " OR ";
                            sql += $"filename LIKE '%{value.Trim().Replace(" ", "%")}%'";
                            sql += " ";
                        }
                        else if (isNumeric && !isDate)
                        {
                            sql += $"id_allievo = {value.Trim()}";
                            sql += " OR ";
                            sql += $"id_classe = {value.Trim()}";
                            sql += " ";
                        }
                        else if (!isNumeric && isDate)
                        {
                            sql += $"data_esame LIKE '%{dateString}%'";
                            sql += " ";
                        }
                    }
                    sql += "ORDER BY Gruppo, Utente, codfisc, id_allievo, NomeClasse; ";

                    result = GetData(configurations.LocalDb, configurations.ConnectionStringRemoteDb, fullPathLocalDb, sql);

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }
        }

        private string SetFullPathLocalDb()
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                var fullPathLocalDb = String.Empty;
                try
                {
                    var configurations = GetConfig();
                    fullPathLocalDb = configurations.FullPathLocalDb;
                    if (!System.IO.File.Exists(fullPathLocalDb) && configurations.LocalDb)
                    {
                        openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
                        openFileDialog.FileName = "LocalDbOutput.sqlite";
                        PositionWaitTmr.Enabled = false;
                        openFileDialog.Title = "Select local db sqlite";

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                            fullPathLocalDb = openFileDialog.FileName;

                        PositionWaitTmr.Enabled = true;
                        SaveConfig("FullPathLocalDbDefaultIsEmpty", fullPathLocalDb);

                        if (!System.IO.File.Exists(fullPathLocalDb) && configurations.LocalDb) throw new Exception("LocalDb not found!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return fullPathLocalDb;
            }
        }

        private string SetPathToSave()
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                var pathtoSave = String.Empty;
                try
                {
                    var configurations = GetConfig();
                    pathtoSave = configurations.PathToSave;
                    if (!System.IO.Directory.Exists(pathtoSave))
                    {
                        folderBrowserDialog.SelectedPath = System.IO.Path.GetTempPath();
                        PositionWaitTmr.Enabled = false;
                        folderBrowserDialog.Description = "Select the folder where the Attestati will be saved";

                        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                            pathtoSave = folderBrowserDialog.SelectedPath;

                        PositionWaitTmr.Enabled = true;
                        SaveConfig("PathToSave", pathtoSave);

                        if (!System.IO.Directory.Exists(pathtoSave)) throw new Exception("Path not valid!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return pathtoSave;
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
                                        Application.DoEvents();
                                        fields = result.NewRow();
                                        for (int nField = 0; nField < fieldCount; nField++)
                                        {
                                            Application.DoEvents();
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

        private Configurations GetConfig()
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                var result = new Configurations() { };
                try
                {
                    ConfigurationManager.RefreshSection("appSettings");
                    var fullPathLocalDb = ConfigurationManager.AppSettings["FullPathLocalDbDefaultIsEmpty"].ToString();
                    var connectionStringRemoteDb = ConfigurationManager.AppSettings["ConnectionStringRemoteDb"].ToString();
                    var pathToSave = ConfigurationManager.AppSettings["PathToSave"].ToString();
                    var localDb = Convert.ToBoolean(ConfigurationManager.AppSettings["LocalDb"].ToString());

                    result.FullPathLocalDb = fullPathLocalDb;
                    result.ConnectionStringRemoteDb = connectionStringRemoteDb;
                    result.LocalDb = localDb;
                    result.PathToSave = pathToSave;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            message = String.Empty;

            StartWait();
            Search();
            EndWait();
        }

        private void StartWait()
        {
            waitStrated = true;
            SearchBtn.Enabled = false;
            SelDeselBtn.Enabled = false;
            SaveBtn.Enabled = false;
            SearchTxt.Enabled = false;
            dataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            waitFrm.Show();
            waitFrm.Visible = false;
            PositionWaitTmr.Enabled = true;
            Application.DoEvents();
        }

        private void EndWait()
        {
            PositionWaitTmr.Enabled = false;
            waitFrm.Visible = false;
            waitFrm.Hide();
            SearchBtn.Enabled = true;
            SelDeselBtn.Enabled = true;
            SaveBtn.Enabled = true;
            SearchTxt.Enabled = true;
            dataGrid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            if (message != String.Empty)
            {
                messagesFrm = new MessagesFrm();
                messagesFrm.Show();
                messagesFrm.Visible = false;
                messagesFrm.ShowMessages(message);
                messagesFrm.Visible = true;

                message = String.Empty;
            }
            waitStrated = false;
        }

        private void Search()
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    var response = new DialogResult();
                    var sBind = new BindingSource();
                    var data = new DataTable();

                    if (SearchTxt.Text == String.Empty)
                        response = MessageBox.Show("Il campo di ricerca è vuoto, in questo modo verranno esposti tutti i dati. Procedere?", MethodInfo.GetCurrentMethod().Name, MessageBoxButtons.YesNo);

                    if (response == DialogResult.No)
                    {
                        return;
                    }

                    dataGrid.SelectAll();
                    dataGrid.ClearSelection();
                    SaveProgressLbl.Text = "";
                    FoundedAttestatiLbl.Text = "0 Attestati Founded";
                    SelDeselBtn.Text = "Select all";

                    data = InitToGetData(SearchTxt.Text, SetFullPathLocalDb());

                    sBind.DataSource = data;
                    dataGrid.DataSource = sBind;
                    dataGrid.AllowUserToAddRows = false;

                    FoundedAttestatiLbl.Text = $"{dataGrid.RowCount} Attestati Founded";

                    foreach (DataGridViewColumn column in dataGrid.Columns)
                    {
                        Application.DoEvents();
                        if (column.Name == "Selected" || column.Name == "OpenPdf")
                            column.ReadOnly = false;
                        else
                            column.ReadOnly = true;

                        if (column.Name == "Id") column.Visible = false;
                    }

                    for (int i = 0; i < dataGrid.RowCount; i++)
                    {
                        Application.DoEvents();
                        dataGrid[1, i].ToolTipText = "Click to Open Pdf";
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            message = String.Empty;

            StartWait();
            SaveAllSelected();
            EndWait(); 
        }

        private void SaveAllSelected()
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    var response = new DialogResult();
                    var countSelected = 0;
                    var countErrors = 0;
                    var countSaved = 0;
                    var configurations = GetConfig();
                    var filename = String.Empty;
                    var gruppo = String.Empty;
                    var id_allievo = String.Empty;
                    var id_classe = String.Empty;
                    var id = String.Empty;
                    var data = new DataTable();
                    byte[] blob = null;
                    var pathToSave = SetPathToSave();
                    var errors = String.Empty;
                    var fullPathLog = String.Empty;

                    countSelected = CountItemSelected();
                    SaveProgressLbl.Text = $"0 Saved on {countSelected} Selected";

                    if (SearchTxt.Text == String.Empty)
                        response = MessageBox.Show($"Sono stati selezionati {countSelected} Attestati. Procedere?", MethodInfo.GetCurrentMethod().Name, MessageBoxButtons.YesNo);

                    if (response == DialogResult.No)
                    {
                        return;
                    }

                    for (int i = 0; i < dataGrid.RowCount; i++)
                    {
                        Application.DoEvents();
                        try
                        {
                            if (dataGrid[0, i].Value?.ToString().ToLower() == "true")
                            {
                                filename = dataGrid.Rows[i].Cells["Filename"].Value.ToString();
                                id_allievo = dataGrid.Rows[i].Cells["Id Allievo"].Value.ToString();
                                gruppo = dataGrid.Rows[i].Cells["Gruppo"].Value.ToString();
                                id_classe = dataGrid.Rows[i].Cells["Id Classe"].Value.ToString();
                                id = dataGrid.Rows[i].Cells["Id"].Value.ToString();

                                data = GetPdf(id);
                                blob = data.Rows[0]["pdf"] as Byte[];

                                if (blob.Length == 0)
                                    throw new Exception("Attestato not exists");

                                SaveFileFromBlob(blob, Path.Combine(pathToSave, "Attestati", id_allievo), id + "_" + gruppo.Replace(" ", "") + "_" + id_classe + "" + filename.Replace(".htm", ".pdf"));

                                countSaved++;
                                SaveProgressLbl.Text = $"{countSaved} Saved on {countSelected} Selected";

                                logger.Info($"Saved file: {filename} - " + SaveProgressLbl.Text);
                            }
                        }
                        catch (Exception ex)
                        {
                            countErrors++;
                            logger.Error($" filename: {filename} - " + ex.ToString());
                            errors += $" filename: {filename} - " + ex.Message + Environment.NewLine;
                        }

                    }

                    fullPathLog = GetLogFileName("mainLog");

                    message = errors == String.Empty ? SaveProgressLbl.Text : SaveProgressLbl.Text + Environment.NewLine + Environment.NewLine + $"With {countErrors} errors:" + Environment.NewLine + "You can check the logs here: " + fullPathLog + Environment.NewLine + errors;

                    if (Directory.Exists(Path.Combine(pathToSave, "Attestati")) && countSaved > 0)
                        System.Diagnostics.Process.Start(Path.Combine(pathToSave, "Attestati"));
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private string GetLogFileName(string targetName)
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                string fileName = null;
                try
                {
                    FileTarget fileTarget = null;
                    var logEventInfo = new LogEventInfo();
                    WrapperTargetBase wrapperTarget;
                    Target target;

                    if (LogManager.Configuration != null && LogManager.Configuration.ConfiguredNamedTargets.Count != 0)
                    {
                        target = LogManager.Configuration.FindTargetByName(targetName);
                        if (target == null)
                        {
                            throw new Exception("Could not find target named: " + targetName);
                        }

                        wrapperTarget = target as WrapperTargetBase;

                        if (wrapperTarget == null)
                        {
                            fileTarget = target as FileTarget;
                        }
                        else
                        {
                            fileTarget = wrapperTarget.WrappedTarget as FileTarget;
                        }

                        if (fileTarget == null)
                        {
                            throw new Exception("Could not get a FileTarget from " + target.GetType());
                        }

                        logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
                        fileName = fileTarget.FileName.Render(logEventInfo);
                        fileName = fileName.Replace(@"\\",@"\");
                    }
                    else
                    {
                        throw new Exception("LogManager contains no Configuration or there are no named targets");
                    }

                    if (!File.Exists(fileName))
                    {
                        throw new Exception("File " + fileName + " does not exist");
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                return fileName;
            }
        }

        private DataTable GetPdf(string id)
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                var result = new DataTable();
                try
                {
                    var sql = String.Empty;
                    var configurations = GetConfig();

                    sql += "SELECT pdf FROM ESTRAZIONI_ATTESTATI ";
                    sql += $"WHERE Id = {id}";

                    result = GetData(configurations.LocalDb, configurations.ConnectionStringRemoteDb, configurations.FullPathLocalDb, sql);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            }
        }

        private void SelDeselBtn_Click(object sender, EventArgs e)
        {
            message = String.Empty;

            StartWait();
            SelectDeselect();
            dataGrid.EndEdit();
            var countSelected = CountItemSelected();
            SaveProgressLbl.Text = $"0 Saved on {countSelected} Selected";
            EndWait();
        }

        private void SelectDeselect()
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    for (int i = 0; i < dataGrid.RowCount; i++)
                    {
                        if (SelDeselBtn.Text == "Select all")
                            dataGrid[0, i].Value = true;
                        else
                            dataGrid[0, i].Value = false;
                    }

                    if (SelDeselBtn.Text == "Select all")
                        SelDeselBtn.Text = "Deselect all";
                    else
                        SelDeselBtn.Text = "Select all";

                    dataGrid.Update();
                    dataGrid.Refresh();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (waitStrated) return;

            var id_allievo = "";
            var gruppo = "";
            var id_classe = "";
            var fileName = "";
            var id = "";

            try
            {
                if (e.ColumnIndex == dataGrid.Columns["OpenPdf"].Index && e.RowIndex >= 0)
                {

                    StartWait();
                    id_allievo = dataGrid[dataGrid.Columns["Id Allievo"].Index, e.RowIndex].Value.ToString();
                    gruppo = dataGrid[dataGrid.Columns["Gruppo"].Index, e.RowIndex].Value.ToString();
                    id_classe = dataGrid[dataGrid.Columns["Id Classe"].Index, e.RowIndex].Value.ToString();
                    fileName = dataGrid[dataGrid.Columns["Filename"].Index, e.RowIndex].Value.ToString();
                    id = dataGrid[dataGrid.Columns["Id"].Index, e.RowIndex].Value.ToString();

                    OpenPdfOnClick(id_allievo, gruppo, id_classe , fileName, id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($" filename: {fileName} - " + ex.Message);
            }
            finally
            {
                EndWait();
            }
        }

        private void OpenPdfOnClick(string id_allievo, string gruppo, string id_classe, string fileName,string id)
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    var data = GetPdf(id);
                    var blob = data.Rows[0]["pdf"] as Byte[];
                    var path = Path.Combine(System.IO.Path.GetTempPath(), "Attestati", id_allievo);
                    var newFileName = id + "_" + gruppo + "_" + id_classe + "" + fileName.Replace(".htm", ".pdf");
                    var fullPath = String.Empty;

                    if (blob.Length==0)
                        throw new Exception("Attestato not exists");

                    fullPath  = SaveFileFromBlob(blob, path, newFileName);
                    OpenPdfFile(fullPath);
                }
                catch (Exception ex)
                {
                    logger.Error($" filename: {fileName} - " + ex.ToString());
                    throw ex;
                }
            }
        }

        private string SaveFileFromBlob(byte[] blob, string pathToSave, string fileNameWithExtension)
        {
            var fullPath = String.Empty;
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    fileNameWithExtension = RemoveInvalidFileNameChars(fileNameWithExtension);

                    if (!Directory.Exists(pathToSave))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(pathToSave);
                    }

                    fullPath = Path.Combine(pathToSave, fileNameWithExtension);

                    if (File.Exists(fullPath))
                    {
                        fullPath = Path.Combine(pathToSave, "COPY_" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + fileNameWithExtension);
                        logger.Info($"The file [{Path.Combine(pathToSave, fileNameWithExtension)}] already exists, fileName renamed");
                    }
                    using (StreamWriter sWriter = new StreamWriter(fullPath))
                    {
                        var writer = new BinaryWriter(sWriter.BaseStream);
                        writer.Write(blob);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return fullPath;
            }
        }

        private static string RemoveInvalidFileNameChars(string filename)
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    foreach (char invalidchar in System.IO.Path.GetInvalidFileNameChars())
                    {
                        filename = filename.Replace(invalidchar, '_');
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return filename;
            }
        }

        private void OpenPdfFile(string fullPath)
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                try
                {
                    System.Diagnostics.Process.Start(fullPath);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void dataGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }
            var dataGridView = (sender as DataGridView);

            if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
                dataGrid.Cursor = Cursors.Hand;
            else
                dataGrid.Cursor = Cursors.Default;
        }

        private void SearchAttestati_Load(object sender, EventArgs e)
        {
            SaveBtn.Cursor = Cursors.Hand;
            SelDeselBtn.Cursor = Cursors.Hand;
            SearchBtn.Cursor = Cursors.Hand;
            SaveProgressLbl.Text = "";
            FoundedAttestatiLbl.Text = "0 Attestati Founded";
        }

        private int CountItemSelected()
        {
            using (var logger = new NLogScope(classLogger, MethodInfo.GetCurrentMethod()))
            {
                var countSelected = 0;
                try
                {
                    for (int i = 0; i < dataGrid.RowCount; i++)
                    {
                        if (dataGrid[0, i].Value?.ToString().ToLower() == "true")
                        {
                            countSelected++;
                        }
                    }
                }
                catch (Exception)
                {

                }

                return countSelected;
            }
        }

        private void dataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var countSelected = CountItemSelected();
            SaveProgressLbl.Text = $"0 Saved on {countSelected} Selected";
        }

        private void dataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            dataGrid.EndEdit();
        }

        private void SearchTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13) Search();
        }

        private void PositionWaitTmr_Tick(object sender, EventArgs e)
        {
            waitFrm.Top = this.Top + this.Height - waitFrm.Height - 10;
            waitFrm.Left = this.Left + 10;
            waitFrm.Visible = true;
            if (Form.ActiveForm == this)
            {
                waitFrm.TopMost = true;
                waitFrm.TopMost = false;
            }
        }
    }
}