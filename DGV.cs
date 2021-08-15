using System;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace CuSum_Analysis
{
    class DGV
    {
        public static TabPage[] tPages;
        public static DataGridView[] dgvs;
        //================================================================================================================================//
        public static string openExcelToGetData(string wd, DataGridView dgv, System.Windows.Forms.TextBox txtNav,ProgressFrm pf, System.Drawing.Point loc, Size s)
        {
            string excelFileName = SelectFileToOpen(wd);
            int firstLine = 1;

            System.Data.DataTable dt = new System.Data.DataTable();
            if (excelFileName != "")
            {
                try
                {
                    //start Excel
                    //start progress
                    int xpos = loc.X + (int)((s.Width-pf.Size.Width) / 2);
                    int ypos = loc.Y + (int)((s.Height-pf.Size.Height) / 2);
                    pf.resetPBar();
                    pf.Show();
                    pf.Location = new System.Drawing.Point(xpos, ypos);
                    pf.updateLabel("Starting Excel Application");
                    //pf.updateProgressBar(10);

                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(excelFileName);
                    //pf.updateLabel("Opening Excel File");
                    Thread.Sleep(100);
                    Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                    Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

                    //start reding the file
                    
                    Random random = new Random();
                    int randomValue = random.Next(7, 15);
                    int currentProgressValue = randomValue;
                    pf.updateProgressBar(currentProgressValue);
                    pf.updateLabel("Reading Excel File, overall progress (" + (currentProgressValue).ToString("0.00") + "% )");
                    int rowCount = xlRange.Rows.Count;
                    int colCount = xlRange.Columns.Count;
                    Console.WriteLine("Row: {0}, Col: {1}", rowCount, colCount);
                    dt.Columns.Add("Training #");//for live data
                    for (int col = 1; col <= colCount; col++)
                    {
                        dt.Columns.Add(xlRange.Cells[firstLine, col].Value.ToString());

                    }
                    
                    int count = 0;
                    for (int row = 1 + firstLine; row <= rowCount; row++)
                    {
                        DataRow workRow = dt.NewRow();
                        try
                        {
                            workRow[0] = (row-firstLine).ToString(); //live data
                            for (int col = 1; col <= colCount; col++)
                            {
                                //workRow[col - 1] = (xlRange.Cells[row, col].value).ToString();
                                workRow[col] = (xlRange.Cells[row, col].value).ToString();
                                Console.Write(workRow[col - 1] + ", ");
                            }
                            
                            currentProgressValue = (int)Math.Ceiling((((decimal)row / (decimal)rowCount) * (100 - randomValue)))+randomValue;
                            //decimal vv = (double)(row / rowCount); //(decimal)(row / rowCount) * (100 - randomValue);
                            pf.updateLabel("Reading Excel File: " + (row- firstLine).ToString() + " of " + (rowCount-1).ToString() + " , overall progress: ( " + (currentProgressValue).ToString("0.00")+"% )");
                            
                            pf.updateProgressBar(currentProgressValue);
                            Thread.Sleep(10);
                            dt.Rows.Add(workRow);
                            count++;
                            Console.WriteLine("dgv row num: {0}, count {1}", currentProgressValue, currentProgressValue);
                        }
                        catch (Exception err)
                        {
                            //if there is error, ignore it;
                            MessageBox.Show(err.Message, "Error1", MessageBoxButtons.OK);
                        }
                    }
                    Console.WriteLine("out of loop, dt row num: {0}, count {1} and dgv row count {2}", dt.Rows.Count, count, dgv.Rows.Count);
                    dgv.DataSource = dt;
                    dgv.CurrentCell = dgv.Rows[0].Cells[0];
                    dgv.CurrentCell.Selected = true;
                    Console.WriteLine("out of loop, dt row num: {0}, count {1} and dgv row count {2}", dt.Rows.Count, count, dgv.Rows.Count);
                    txtNav.Text = "1 of " + dt.Rows.Count.ToString();
                    MessageBox.Show("Total "+ (dgv.Rows.Count-1).ToString()+ " records of data have been loaded","Open File",MessageBoxButtons.OK);
                    xlWorkbook.Close(0);
                    xlApp.Quit();
                    terminateExcel();
                    Thread.Sleep(2000);
                    pf.Hide();
                    pf.Parent = null;

                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error2", MessageBoxButtons.OK);
                }
                
            }//if filename exists
            return excelFileName;
        }//getdatatable
         //********************************************************************************************************************************//

        //================================================================================================================================//
        public static string SelectFileToOpen(string wd)
        {
            string excelFileName = "";
            OpenFileDialog fdlg = new OpenFileDialog();

            fdlg.Title = "Excel File Dialog";
            fdlg.InitialDirectory = @wd;
            fdlg.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                excelFileName = fdlg.FileName;
            }
            return excelFileName;
        }//SelectOpenFile
         //********************************************************************************************************************************//

        //================================================================================================================================//
        private static void terminateExcel()
        {
            var process = System.Diagnostics.Process.GetProcessesByName("Excel");
            foreach (var p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    try
                    {
                        p.Kill();
                    }
                    catch { }
                }
            }
            //terminate excel
        }
        //********************************************************************************************************************************//

        //================================================================================================================================//
        public static void changeSelectionOfRecord(object sender,DataGridView dgv, System.Windows.Forms.TextBox txtIndx)
        {
            string name = ((System.Windows.Forms.Button)sender).Text;
            int next = (dgv.CurrentCell.RowIndex);
            switch (name)
            {
                case ">":
                    if (next < dgv.Rows.Count - 1)
                    {
                        next = next + 1;
                    }
                    break;
                case "<":
                    if (next >0)
                    {
                        next = next - 1;
                    }
                    break;
                case ">>":
                    next = dgv.Rows.Count - 2;
                    break;
                case "<<":
                    next = 0;
                    break;
            }

            

            dgv.ClearSelection();
            dgv.CurrentCell = dgv.Rows[next].Cells[0];
            dgv.CurrentCell.Selected = true;
            if (next == dgv.Rows.Count - 1)
            {
                txtIndx.Text = "New Row";
            }
            else
            {
                txtIndx.Text = (next + 1).ToString() + " of " + (dgv.Rows.Count - 1).ToString();
            }
        }
        //********************************************************************************************************************************//

        //================================================================================================================================//
        public static string openMutlipleSheetExcelFile(string wd, TabControl tabExcel, CuSumAnalysisGUI frm, ProgressFrm pf, bool isTestMode)
        {
            string excelFileName = SelectFileToOpen(wd);
     
            if (excelFileName != "")
            {
                pf.resetPBar();
                pf.Show();
                pf.updateLabel("Starting Excel Application");
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(excelFileName);
                Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
                int numOfSheets = xlWorkbook.Worksheets.Count;
                int maxRow = 1000;
                if (isTestMode) { }
                if (isTestMode) { maxRow = 20; ; numOfSheets = 3; }
                System.Data.DataTable dt = new System.Data.DataTable();
                dgvs = new DataGridView[numOfSheets];
                pf.updateLabel("Creating Data Grid Views");
                tPages = new TabPage[numOfSheets];
                for (int i = 1; i <= numOfSheets; i++)

                {
                    tPages[i - 1] = new TabPage();
                    tPages[i - 1].Name = "Sheet" + i.ToString();
                    tPages[i - 1].Text = xlWorkbook.Sheets[i].Name;
                    tabExcel.TabPages.Add(tPages[i - 1]);
                    dgvs[i - 1] = new DataGridView();
                    tabExcel.TabPages[i - 1].Controls.Add(dgvs[i - 1]);
                    dgvs[i - 1].Name = "dataGridView_" + i.ToString();
                    dgvs[i - 1].Dock = DockStyle.Fill;
                    //dgvs[i - 1].CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(frm.dgvs_CellMouseDown);


                }
                pf.updateLabel("Reading Data File");
                string overall = ", Overall Reading Progress: 0.00%";
                for (int i = 1; i <= numOfSheets; i++)
                {
                    pf.updateLabel("Reading Data from Sheet " + i.ToString() + " of " + numOfSheets.ToString() + overall);
                    xlWorksheet = xlWorkbook.Sheets[i];
                    int[] startline = new int[] { 1, 0 };
                    while (xlWorksheet.Cells[startline[0], 1].value2 == null)
                    {
                        //MessageBox.Show("null in row");
                        startline[0]++;
                    }
                    while (xlWorksheet.Cells[startline[0], startline[1] + 1].value2 != null)
                    {
                        //MessageBox.Show("null in col");
                        startline[1]++;
                    }
                    //MessageBox.Show("StartLine: " + startline[0].ToString() + ", " + startline[1].ToString());


                    //MessageBox.Show(xlWorksheet.Name+" "+ xlWorksheet.Cells[1, 1].value.ToString()+" "+ xlWorksheet.Cells[1, 2].value.ToString());
                    dt = new System.Data.DataTable();
                    bool isEndOfData = false;
                    dt.Columns.Add("Experiment#");
                    for (int col = 1; col <= startline[1]; col++)
                    {
                        dt.Columns.Add(xlWorksheet.Cells[startline[0], col].value.ToString());
                    }
                    int row = 1 + startline[0];
                    //for (int row = startline[0]+1; row <= 100; row++)

                    while ((!isEndOfData) && (row <= maxRow))
                    {
                        DataRow workRow = dt.NewRow();
                        try
                        {
                            workRow[0] = ((row - startline[0]).ToString());
                            for (int col = 1; col <= startline[1]; col++)
                            {
                                workRow[col] = (xlWorksheet.Cells[row, col].value2).ToString();
                                Console.Write(workRow[col - 1] + ", ");
                            }

                            //Thread.Sleep(10);
                            dt.Rows.Add(workRow);
                            row++;
                            isEndOfData = (xlWorksheet.Cells[row, 1].value2 == null);
                        }//try
                        catch (Exception err)
                        {
                            //if there is error, ignore it;
                            //MessageBox.Show(err.Message, "Error1", MessageBoxButtons.OK);
                        }
                    }
                    //MessageBox.Show("TOTAL ROW IS " + row.ToString());
                    dgvs[i - 1].DataSource = dt;
                    pf.updateProgressBar((int)(((decimal)i / (decimal)(numOfSheets)) * 100m));
                    overall = ", Overall Reading Progress: " + ((int)(((decimal)i / (decimal)(numOfSheets)) * 100m)).ToString() + "%";

                }
                pf.updateLabel("Reading Data from Sheet " + numOfSheets.ToString() + " of " + numOfSheets.ToString() + overall);

                xlWorkbook.Close(0);
                xlApp.Quit();
                terminateExcel();
                Thread.Sleep(2000);
                pf.Hide();
                pf.Parent = null;
                
                //MessageBox.Show("Done");
            }
            //Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            //Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
            return excelFileName;

            //

            //
        }
        //********************************************************************************************************************************//

        //================================================================================================================================//
        public static int checkInputValue(DataGridView dgv, KeyPressEventArgs e, int ci,object sender)
        {
            //
            //
            if (dgv.CurrentCell.ColumnIndex == 0)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    //MessageBox.Show("Get here", "check input Value");
                    e.Handled = true;
                }
            }
            if(dgv.CurrentCell.ColumnIndex == 2)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            }
            if ((dgv.CurrentCell.ColumnIndex == 1))
            {
                //MessageBox.Show("ci= " + ci.ToString());
                bool isOneorZero = (e.KeyChar == '0') || (e.KeyChar == '1');
                bool iscontrolchar = char.IsControl(e.KeyChar);
                if (iscontrolchar)
                {
                    ci = 0;
                }
                else
                {
                    if (ci < 3)
                    {
                        if (!isOneorZero)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            ci++;
                        }
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            return ci;
        }
        //********************************************************************************************************************************//

        //================================================================================================================================//
        public static void setNavigationText(DataGridView dvg, System.Windows.Forms.TextBox txtb)
        {
            dvg.ClearSelection();
            dvg.CurrentCell = DGV.dgvs[0].Rows[0].Cells[0];
            DGV.dgvs[0].CurrentCell.Selected = true;
            txtb.Text = "1 of " + (DGV.dgvs[0].Rows.Count - 1).ToString();
        }
        //********************************************************************************************************************************//

        //================================================================================================================================//
        public static void saveToExcelFile(bool saveToOpen, 
            string excelProgressFileName, string workingDirectory, string[] sheetname, DataGridView[] dgvss, CuSumAnalysisGUI frm)//Label saveFileProgresslbl, ProgressBar pbar, )
        {
            string message;
            string caption;
            //if (dgv.Rows.Count <= 1)
            //{
            //    message = "No Data. Nothing can be saved";
            //    caption = "No Data";
            //    MessageBox.Show(message, caption, MessageBoxButtons.OK);
            //    return;
            //}
            //==============
            string newSavedFileName = "";
            //input new save filename
            if (!saveToOpen)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = workingDirectory;
                saveFileDialog1.Filter = "Excel|*.xlsx";
                saveFileDialog1.Title = "Save an Excel File";
                //saveFileDialog1.InitialDirectory = workingFolder;
                saveFileDialog1.FilterIndex = 1;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    newSavedFileName = saveFileDialog1.FileName;
                    //check file nane is valid
                    string filenameWoExt = Path.GetFileNameWithoutExtension(newSavedFileName);
                    bool valid = true;
                    string ch = "";
                    List<string> Pattern = new List<string> { "^", "<", ">", ";", "|", "'", "/", ",", "\\", ":", "=", "?", "\"", "*" };
                    for (int i = 0; i < Pattern.Count; i++)
                    {
                        if (filenameWoExt.Contains(Pattern[i]))
                        {
                            valid = false;
                            ch = Pattern[i];
                            break;
                        }
                    }
                    if (!valid)
                    {
                        message = "File name contain invalid character - " + ch;
                        caption = " No File Name Input";
                        MessageBox.Show(message, caption, MessageBoxButtons.OK);
                        return;
                    }
                    //check file name
                }
                else
                {
                    message = "No new file name.";
                    caption = " No File Name Input";
                    MessageBox.Show(message, caption, MessageBoxButtons.OK);
                    return;
                }

            }
            if ((newSavedFileName != "") || (saveToOpen))
            {
                //saveFileProgresslbl.Visible = true;
                //saveFileProgresslbl.Text = "Progress Status : Starting Excel Application.";
                System.Drawing.Point pt = new System.Drawing.Point(frm.Left, frm.Top);
                Size s = new Size(frm.Size.Width, frm.Size.Height);
                ProgressFrm pf = new ProgressFrm();
                pf.resetPBar();
                pf.Show();
                pf.Location = new System.Drawing.Point(pt.X + (int)(s.Width - pf.Width) / 2,
                    pt.Y + (int)s.Height / 4);
                pf.updateLabel("Starting Excel Application");
                Microsoft.Office.Interop.Excel.Application excel;
                Microsoft.Office.Interop.Excel.Workbook excelworkBook;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet;
                Microsoft.Office.Interop.Excel.Range excelCellrange;
                // Start Excel and get Application object.  
                excel = new Microsoft.Office.Interop.Excel.Application();
                // for making Excel visible  
                excel.Visible = false;
                excel.DisplayAlerts = false;
                // Creation a new Workbook  
               
                
                //clear worksheet in existing excel file
                if (saveToOpen)
                {
                    
                    excelworkBook = excel.Workbooks.Open(excelProgressFileName);
                    
                }
                else
                {
                    excelworkBook = excel.Workbooks.Add();
                }

                


                //writing to file start from here
                for (int si = 0; si < dgvss.Length; si++)
                {
                    if (saveToOpen)
                    {
                        excelSheet = excelworkBook.Sheets[sheetname[si]];
                        
                    }
                    else
                    {
                        excelSheet = 
                            excelworkBook.Sheets.Add(excelworkBook.Worksheets[excelworkBook.Sheets.Count], Type.Missing, Type.Missing,Type.Missing);
                        excelSheet.Name = "temp";
                        for (int shi= 1; shi<= excelworkBook.Sheets.Count;shi++)
                        {
                            if (sheetname[si]== excelworkBook.Sheets[shi].Name)
                            {
                                sheetname[si]= sheetname[si]+"_"+ DateTime.Now.ToString("ddMMyyyy");
                            }
                        }
                        excelSheet.Name=sheetname[si];
                        //excelSheet.Name = sheetname;// DateTime.Now.ToString("ddMMyyyy");
                    }
                    // Workk sheet  
                    excelSheet.Cells.ClearContents();
                    excelSheet.Cells.ClearFormats();
                    excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;

                    

                    //write data to worksheet
                    //saveFileProgresslbl.Text = "Progress Status : Write Data to File.";
                    //pbar.Value = 0;
                    //pbar.Visible = true;
                    //write the header
                    pf.updateLabel("Writing to Excel File");
                    DataGridView dgv = dgvss[si];
                    for (int col = 1; col < dgv.Columns.Count; col++)
                    {
                        excelSheet.Cells[1, col] = dgv.Columns[col].HeaderText;
                    }
                    int rowCount = dgv.Rows.Count - 1;
                    decimal rowPortion = 1.0m / (decimal)dgvs.Length / (decimal)(Math.Max(1, rowCount));
                    for (int row = 0; row < rowCount; row++)
                    {
                        for (int col = 1; col < dgv.Columns.Count; col++)
                        {
                            excelSheet.Cells[row + 2, col] = dgv.Rows[row].Cells[col].Value;
                        }
                        decimal progressVal = (((decimal)(si) / (decimal)(dgvss.Length) + (decimal)(row+1) * rowPortion) * 100m);
                        string overall = ", Overall Writing Progress: (" + (progressVal).ToString("0.00") + "%)";
                        pf.updateLabel("Writing Data to Sheet " + (si+1).ToString() + " of " + dgvss.Length.ToString() + overall);
                        overall = ", Overall Writing Progress: (" + progressVal.ToString("0.00") + "%)";
                        pf.updateProgressBar(Math.Min(100, (int)(Math.Ceiling(progressVal))));

                        //pf.updateProgressBar((int)(100.0d * (double)(row + 1) / (double)(rowCount)));
                        //pf.updateLabel("Writing to Excel File (" + (100.0d * (double)(row + 1) / (double)(rowCount)).ToString("0.00") + "%)");
                        //saveFileProgressBar.Width = (int)(Math.Ceiling(((double)saveFileBaseBar.Width * (double)(row+1) / (rowCount))));
                        //int value = (int)Math.Ceiling((pbar.Maximum * (double)(row + 1) / (rowCount)));
                        //bar value
                        //this.setBarValue(value, pbar);
                        //bar value
                        //saveFileProgresslbl.Text = "Progress Status: " + Math.Ceiling((100.0d * (double)(row + 1) / (rowCount))).ToString() + " % written";// (row-1).ToString() + " Rows are Written."; 


                    }
                    //excelSheet.Cells[1, 1] = "Sample test data";
                    //excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();
                    excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[dgv.Rows.Count, dgv.Columns.Count - 1]];
                    excelCellrange.EntireColumn.AutoFit();
                    Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                    border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    border.Weight = 2d;
                    //writing ends here
                }


                if (!saveToOpen)
                {
                    excelworkBook.SaveAs(newSavedFileName);
                    message = "Data Saved to " + newSavedFileName;

                }
                else
                {
                    excelworkBook.Save();
                    message = "Saved to " + excelProgressFileName;
                }
                caption = "Data Saved";
                excelworkBook.Close(0);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                //terminate excel
                var process = System.Diagnostics.Process.GetProcessesByName("Excel");
                foreach (var p in process)
                {
                    if (!string.IsNullOrEmpty(p.ProcessName))
                    {
                        try
                        {
                            p.Kill();
                        }
                        catch { }
                    }
                }
                //terminate excel
                Thread.Sleep(1250);
                pf.Hide();
                pf.Parent = null;
                MessageBox.Show(message, caption, MessageBoxButtons.OK);
            }
            //saveFileProgresslbl.Visible = false;

            //pbar.Visible = false;
            //pbar.Value = 0;
        }
        //********************************************************************************************************************************//

        //================================================================================================================================//
        public static string openExcelFileWSelection(string wd, TabControl tabExcel, CuSumAnalysisGUI frm, ProgressFrm pf, bool isTestMode,string lastOpenFileName)
        {
            string excelFileName = lastOpenFileName;
            //MessageBox.Show(excelFileName);
            if (excelFileName != "")
            {
                //position//
                System.Drawing.Point pt = new System.Drawing.Point(frm.Left, frm.Top);
                Size s = new Size(frm.Size.Width, frm.Size.Height);
                
                pf.resetPBar();
                pf.Show();
                pf.Location = new System.Drawing.Point(pt.X + (int)(s.Width-pf.Width)/2,
                    pt.Y + (int)s.Height/4);
                pf.updateLabel("Starting Excel Application");
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(excelFileName);
                Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;


                int numOfSheets = xlWorkbook.Worksheets.Count;
                string[] sheetNames;
                FrmSheetSelection fss =new FrmSheetSelection(new string[] { "" });
                
                if (numOfSheets == 0) { return lastOpenFileName; }
                
                    sheetNames = new string[numOfSheets];
                    for (int i = 0; i < numOfSheets; i++)
                        sheetNames[i] = xlWorkbook.Sheets[i + 1].Name;
                fss = new FrmSheetSelection(sheetNames);
                string str;
                str = System.IO.Path.GetFileName(excelFileName) ;
                fss.changeLabel(str);
                if (numOfSheets > 1)
                {

                    fss.StartPosition = FormStartPosition.Manual;
                    fss.Location = new System.Drawing.Point(pt.X +(s.Width-fss.Width)/2, pf.Location.Y + (s.Height-fss.Height)/2);
                    fss.ShowDialog();
                    
                }
                else
                {
                    fss.msg = new string[1] { sheetNames[0] };
                }
                
                int maxRow = 1000;
                if (isTestMode) { maxRow = 20; ; numOfSheets = 3; }
                System.Data.DataTable dt = new System.Data.DataTable();
                if (fss.msg.Length == 0)
                {
                    xlWorkbook.Close(0);
                    xlApp.Quit();
                    terminateExcel();
                    Thread.Sleep(2000);
                    pf.Hide();
                    pf.Parent = null;
                    return lastOpenFileName; }
                //dgvs = new DataGridView[numOfSheets];
                //
                for (int i = tabExcel.TabPages.Count - 1; i > 0; i--)
                {
                    tabExcel.TabPages.RemoveAt(0);
                }
                //
                numOfSheets = fss.msg.Length;
                dgvs = new DataGridView[numOfSheets];
                pf.updateLabel("Creating Data Grid Views");
                tPages = new TabPage[numOfSheets];
                int original = tabExcel.TabPages.Count;
                for (int i = original; i < numOfSheets+original; i++)
                { 
                        tPages[i- original] = new TabPage();
                        tPages[i- original].Name = "Sheet" + (i+1).ToString();
                        tPages[i- original].Text = fss.msg[i- original];
                        tabExcel.TabPages.Add(tPages[i- original]);
                
                    dgvs[i- original] = new DataGridView();
                    tabExcel.TabPages[i].Controls.Add(dgvs[i- original]);
                    dgvs[i- original].Name = "dataGridView_" + (i+1- original).ToString();
                    dgvs[i- original].Dock = DockStyle.Fill;
                    //dgvs[i - 1].CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(frm.dgvs_CellMouseDown);


                }
                
                pf.updateLabel("Reading Data File");
                string overall = ", Overall Reading Progress: 0.00%";
                for (int i = 1; i <= numOfSheets; i++)
                {
                    pf.updateLabel("Reading Data from Sheet " + i.ToString() + " of " + numOfSheets.ToString() + overall);
                    xlWorksheet = xlWorkbook.Sheets[fss.msg[i - 1]];
                    Microsoft.Office.Interop.Excel.Range rng = xlWorksheet.UsedRange;
                    //
                    
                    //MessageBox.Show(fss.msg[i - 1]);
                    dt = new System.Data.DataTable();
                    if (!(rng.Rows.Count<=1))
                    {
                        int[] startline = new int[] { 1, 0 };
                        while (xlWorksheet.Cells[startline[0], 1].value2 == null)
                        {
                            //MessageBox.Show("null in row");
                            startline[0]++;
                        }
                        //
                        xlWorksheet.Cells.ClearFormats();
                        Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
                        xlRange.Cells.ClearFormats();

                        Microsoft.Office.Interop.Excel.Range last = xlWorksheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell, Type.Missing);

                        int lastUsedRow = last.Row;

                        //lRealLastRow = _

                        //MessageBox.Show("row number: " + lastUsedRow.ToString());
                        decimal rowPortion = 1 / (decimal)(xlRange.Rows.Count - 1) / numOfSheets;


                        //MessageBox.Show("row number: " + xlRange.Rows.Count.ToString());

                        //
                        while (xlWorksheet.Cells[startline[0], startline[1] + 1].value2 != null)
                        {
                            //MessageBox.Show("null in col");
                            startline[1]++;
                        }
                        //MessageBox.Show("StartLine: " + startline[0].ToString() + ", " + startline[1].ToString());


                        //MessageBox.Show(xlWorksheet.Name+" "+ xlWorksheet.Cells[1, 1].value.ToString()+" "+ xlWorksheet.Cells[1, 2].value.ToString());
                        
                        bool isEndOfData = false;
                        dt.Columns.Add("Experiment#");
                        for (int col = 1; col <= startline[1]; col++)
                        {
                            dt.Columns.Add(xlWorksheet.Cells[startline[0], col].value.ToString());
                        }
                        int row = 1 + startline[0];
                        //for (int row = startline[0]+1; row <= 100; row++)

                        while ((!isEndOfData) && (row <= maxRow))
                        {
                            DataRow workRow = dt.NewRow();
                            try
                            {
                                workRow[0] = ((row - startline[0]).ToString());
                                for (int col = 1; col <= startline[1]; col++)
                                {
                                    workRow[col] = (xlWorksheet.Cells[row, col].value2).ToString();
                                    Console.Write(workRow[col - 1] + ", ");
                                }

                                //Thread.Sleep(10);
                                dt.Rows.Add(workRow);

                                decimal progressVal = (((decimal)(i - 1) / (decimal)(numOfSheets) + (row - startline[0]) * rowPortion) * 100m);

                                overall = ", Overall Reading Progress: (" + (progressVal).ToString("0.00") + "%)";
                                pf.updateLabel("Reading Data from Sheet " + i.ToString() + " of " + numOfSheets.ToString() + overall);
                                overall = ", Overall Reading Progress: (" + progressVal.ToString("0.00") + "%)";
                                pf.updateProgressBar(Math.Min(100, (int)(Math.Ceiling(progressVal))));
                                row++;
                                isEndOfData = (xlWorksheet.Cells[row, 1].value2 == null);

                            }//try
                            catch (Exception err)
                            {
                                //if there is error, ignore it;
                                //MessageBox.Show(err.Message, "Error1", MessageBoxButtons.OK);
                            }
                        }
                        //MessageBox.Show("TOTAL ROW IS " + row.ToString());
                        
                        //MessageBox.Show("row number: " + row.ToString());
                        //pf.updateProgressBar((int)(((decimal)i / (decimal)(numOfSheets)) * 100m));
                        //overall = ", Overall Reading Progress: (" + (((decimal)i / (decimal)(numOfSheets)) * 100m).ToString("0.00") + "%)";
                    }
                    else
                    {
                        dt.Columns.Add("Experiment#");
                        dt.Columns.Add("Success");
                        dt.Columns.Add("Score");

                    }
                    dgvs[i - 1].DataSource = dt;
                }
                if (original == 1)
                {
                    tabExcel.TabPages.RemoveAt(0);
                }
                
                pf.updateLabel("Reading Data from Sheet " + numOfSheets.ToString() + " of " + numOfSheets.ToString() + overall);
                //Thread.Sleep(8000);
                xlWorkbook.Close(0);
                xlApp.Quit();
                terminateExcel();
                Thread.Sleep(1250);
                pf.Hide();
                pf.Parent = null;
               
                
                //MessageBox.Show("Done");
            }
            //Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            //Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
            return excelFileName;

            //

            //
        }
        //********************************************************************************************************************************//

        //================================================================================================================================//
        public static void deleteSelectedRecords(DataGridView dvg, System.Windows.Forms.TextBox txtb)
        {
            if (dvg.Rows.Count <= 1)
            {
                MessageBox.Show("No Data Available", "No Data", MessageBoxButtons.OK);
                return;
            }
            if (dvg.SelectedRows.Count == 0)
            {
                dvg.Rows[dvg.CurrentCell.RowIndex].Selected = true;
            }

            if (MessageBox.Show("Are you sure to delete all selected rows", "Delete Rows", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {

                foreach (DataGridViewRow row in dvg.SelectedRows)
                {
                    //MessageBox.Show(row.Index.ToString());
                    if (row.Index != dvg.Rows.Count - 1)
                    {
                        dvg.Rows.Remove(row);
                    }
                }
                if (dvg.Rows.Count > 1)
                {
                    txtb.Text = (dvg.CurrentCell.RowIndex + 1).ToString() + " of " + (dvg.Rows.Count - 1).ToString();
                }
                else
                {
                    txtb.Text = " 0 record in Table";
                }
            }

        }
        //********************************************************************************************************************************//

        //================================================================================================================================//

        //********************************************************************************************************************************//


    }
}
