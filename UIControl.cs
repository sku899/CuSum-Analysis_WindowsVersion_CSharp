using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Data;
using System.Resources;

namespace CuSum_Analysis
{
    class UIControl
    {
        ComboBox[] cBox;
        CheckBox[] chkBox;
        static int convertFactor = 100;
        static decimal maxNumUDValue = 1m;
        static decimal minNumUDValue = 0.01m;
        static decimal numUDStep = 0.01m;
//================================================================================//
        public UIControl(ComboBox[] cbox, CheckBox[] chkbox)
        {
            cBox = new ComboBox[cbox.Length];
            for(int i = 0; i < cbox.Length; i++)
            {
                cBox[i] = cbox[i];
            }
            chkBox = new CheckBox[chkbox.Length];
            for(int i = 0; i < chkbox.Length; i++)
            {
                chkBox[i] = chkbox[i];
            }
        }
        //********************************************************************************//

        //================================================================================//
        public static void ComboFill(string role, ComboBox cBox, string fileName, int defaultSelection)
        {
            var reader = new StreamReader(File.OpenRead(fileName));
            if (cBox.Name != "cbxUserName")
            {
                cBox.Items.Add("===Select " + role + "===");
            }
            else
            {
                cBox.Items.Add(" Input User Name");
            }
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line.Trim().Length>0)
                    cBox.Items.Add(line);
            }
            cBox.Sorted = true; ;
            cBox.SelectedIndex = defaultSelection;
            reader.Close();
        }//ComboFill
         //********************************************************************************//

        //================================================================================//
        public static void getDefaultWorkingDirectory(TextBox wd,CheckBox chkudwd, bool chk)
        {
            string workingDirectory = Directory.GetCurrentDirectory()+@"\Data Files";// Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            wd.Text = workingDirectory;
            chkudwd.Checked = chk;
        }
        //********************************************************************************//

       //================================================================================//
       public static void changeWorkingDirectory(TextBox wd, CheckBox chk)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                wd.Text = folderDlg.SelectedPath;
                if (folderDlg.SelectedPath != Directory.GetCurrentDirectory())
                    chk.Checked = false;
                else
                    chk.Checked = true;
            }
        }
        //********************************************************************************//
       
        
        //================================================================================//
        public static void initialNumUDTBar(NumericUpDown[] numud, TrackBar[] tbar)
        {
            decimal[] vals = new decimal[] { 0.10m, 0.10m, 0.05m, 0.10m };
            for (int i=0; i < numud.Length;i++)
            {
                numud[i].Minimum = minNumUDValue;
                if (numud[i].Name== "numUpDownP1")
                {
                    numud[i].Minimum = 2*minNumUDValue;
                }
                numud[i].Maximum = maxNumUDValue;
                numud[i].Increment = numUDStep;
                numud[i].DecimalPlaces = 2;
                tbar[i].Minimum = (int)(minNumUDValue * convertFactor);
                tbar[i].Maximum = (int)(maxNumUDValue * convertFactor);
                //MessageBox.Show(tbar[i].Maximum.ToString());
                numud[i].Value = vals[i];
                tbar[i].Value= (int)(vals[i] * convertFactor);
            }
        }

        //********************************************************************************//

        //================================================================================//
        public static void setIPValues(object sender, NumericUpDown[] numud, TrackBar[] tbar,bool isIPFormOpened, IPForm ipfrm)
        {
            //Console.WriteLine("tbar: "+tbar[0].Value.ToString());
            string type = sender.GetType().Name;
            string name="";
            int intVal=1;
            decimal decVal=0.01m;
            int index = -1;
            int senderType = 0;
            if (type=="NumericUpDown")
            {
                name = ((NumericUpDown)sender).Name;
                decVal = ((NumericUpDown)sender).Value;
                intVal = (int)(decVal * convertFactor);
                senderType = 1;
            }
            if (type == "TrackBar")
            {
                name = ((TrackBar)sender).Name;
                intVal = ((TrackBar)sender).Value;
                decVal = (decimal)((decimal)(intVal) /(decimal)(convertFactor));
                senderType = 2;
            }
            for (int i=0; i<numud.Length;i++)
            {
                if (name==numud[i].Name)
                {
                    index = i;                                       
                    tbar[i].Value = intVal;
                }
                if (name == tbar[i].Name)
                {
                    index = i;
                    numud[i].Value = decVal;
                }
            }
            if (isIPFormOpened)
            {
                if (type == "NumericUpDown") { ipfrm.valueFromMainForm(index, (double)decVal, 1); }
                else { ipfrm.valueFromMainForm(index, (double)intVal, 2); }
            }

        }
        //********************************************************************************//

        //================================================================================//
        public static void preLoadIPValue(object sender, NumericUpDown[] numud, TrackBar[] tbar)
        {
            int preloadIndex = ((ComboBox)sender).SelectedIndex;
            decimal[] alpha = { 0.01m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m };
            decimal[] beta = { 0.01m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m };
            decimal[] p0 = { 0.01m, 0.05m, 0.2m, 0.15m, 0.05m, 0.05m, 0.2m };
            decimal[] p1 = { 0.02m, 0.1m, 0.4m, 0.3m, 0.15m, 0.15m, 0.4m };

            if (preloadIndex >= 0)
            {
                numud[0].Value = alpha[preloadIndex];
                numud[1].Value = beta[preloadIndex];
                numud[2].Value = p0[preloadIndex];
                numud[3].Value = p1[preloadIndex];
                tbar[0].Value = (int)(alpha[preloadIndex] * 100);
                tbar[1].Value = (int)(beta[preloadIndex] * 100);
                tbar[2].Value = (int)(p0[preloadIndex] * 100);
                tbar[3].Value = (int)(p1[preloadIndex] * 100);
            }
        }
        //********************************************************************************//

        //================================================================================//
        public static void writeSepecialCharToLabel(Label[] lbl)
        {

            string[] labelString = new string[] { "\u03B1", "\u03B2", "p" + "\u2080", "p" + "\u2081" };
            for (int i=0; i < labelString.Length; i++)
            {
                lbl[i].Text = labelString[i];
            }
            
        }
        //********************************************************************************//


        //================================================================================//
        public static void setGUIPosition(object sender, Form frm, IPForm frm2, FrmChart fchart)
        {
            string[] rbNames = { "rbCenter", "rbUpperRight", "rbUpperLeft", "rbLowerRight", "rbLowerLeft" };
            string optionName = ((RadioButton)sender).Name;
            int option = -1;
            for (int i = 0; i < rbNames.Length; i++)
            {
                if (optionName == rbNames[i])
                {
                    option = i;
                    break;
                }
            }

            if (option != -1)
            {
                Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
                int xpos = 0;
                int ypos = 0;
                
                switch (option)
                {
                    case 0:
                        xpos = (workingRectangle.Width - frm.Size.Width) / 2;
                        ypos = (workingRectangle.Height - frm.Size.Height) / 2;

                        break;
                    case 1://ur
                        
                        break;
                    case 2://ul
                        xpos = (workingRectangle.Width - frm.Size.Width);
                        ypos = 0;
                        break;
                    case 3://lr
                        xpos = 0;
                        ypos = (workingRectangle.Height - frm.Size.Height);
                        break;
                    case 4://ll
                        xpos = (workingRectangle.Width - frm.Size.Width);
                        ypos = (workingRectangle.Height - frm.Size.Height);
                        break;
                }
                frm.Location = new Point(xpos, ypos);
                switch (option)
                {
                    case 0:
                        xpos = frm.Location.X - frm2.Size.Width;
                        ypos = frm.Location.Y;

                        break;
                    case 1://ur
                        xpos = frm.Size.Width;// + frm2.Size.Width;
                        ypos = frm.Location.Y;

                        break;
                    case 2://ul
                        xpos = frm.Location.X - frm2.Size.Width;
                        ypos = frm.Location.Y; 
                        break;
                    case 3://lr
                        xpos = frm.Size.Width;// + frm2.Size.Width;
                        ypos = frm.Location.Y;
                        break;
                    case 4://ll
                        xpos = frm.Location.X - frm2.Size.Width;
                        ypos = frm.Location.Y;
                        break;
                }
                frm2.Location = new Point(xpos, ypos);
                if (frm.Location.X - fchart.Width < 0)
                {
                    //MessageBox.Show((MainForm.Location.X + MainForm.Width).ToString()+);
                    fchart.Location = new System.Drawing.Point(frm.Location.X + 30 + frm.Width, frm.Location.Y);
                    ;
                }
                else fchart.Location = new System.Drawing.Point(frm.Location.X - fchart.Width, frm.Location.Y + 10);
            }
        }
        //********************************************************************************//


        //================================================================================//
        public static void setIPFormPosition(IPForm frm2, CuSumAnalysisGUI mainfrm)
        {
            frm2.Location = new Point();
        }
        //********************************************************************************//

        //================================================================================//
        //public void saveToExcelFile(bool saveToOpen, string excelProgressFileName, DataGridView dgv, Label saveFileProgresslbl, ProgressBar pbar, string workingDirectory)
        public static void saveToExcelFile(bool saveToOpen, string excelProgressFileName, DataGridView dgv, string workingDirectory)//Label saveFileProgresslbl, ProgressBar pbar, )
        {
            string message;
            string caption;
            if (dgv.Rows.Count <=1)
            {
                message = "No Data. Nothing can be saved";
                caption = "No Data";
                MessageBox.Show(message, caption, MessageBoxButtons.OK);
                return;
            }
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
                excelworkBook = excel.Workbooks.Add();
                //clear worksheet in existing excel file
                if (saveToOpen)
                {
                    excelworkBook.Close(0);
                    excelworkBook = excel.Workbooks.Open(excelProgressFileName);
                    excelSheet = excelworkBook.Sheets[1];
                    excelSheet.Cells.ClearContents();
                    excelSheet.Cells.ClearFormats();
                }
                // Workk sheet  
                excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;

                excelSheet.Name = DateTime.Now.ToString("ddMMyyyy");
                //write data to worksheet
                //saveFileProgresslbl.Text = "Progress Status : Write Data to File.";
                //pbar.Value = 0;
                //pbar.Visible = true;
                //write the header
                for (int col = 0; col < dgv.Columns.Count; col++)
                {
                    excelSheet.Cells[1, col + 1] = dgv.Columns[col].HeaderText;
                }
                int rowCount = dgv.Rows.Count-1;
                for (int row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < dgv.Columns.Count; col++)
                    {
                        excelSheet.Cells[row + 2, col + 1] = dgv.Rows[row].Cells[col].Value;
                    }

                    //saveFileProgressBar.Width = (int)(Math.Ceiling(((double)saveFileBaseBar.Width * (double)(row+1) / (rowCount))));
                    //int value = (int)Math.Ceiling((pbar.Maximum * (double)(row + 1) / (rowCount)));
                    //bar value
                    //this.setBarValue(value, pbar);
                    //bar value
                    //saveFileProgresslbl.Text = "Progress Status: " + Math.Ceiling((100.0d * (double)(row + 1) / (rowCount))).ToString() + " % written";// (row-1).ToString() + " Rows are Written."; 


                }
                //excelSheet.Cells[1, 1] = "Sample test data";
                //excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();
                excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[dgv.Rows.Count , dgv.Columns.Count]];
                excelCellrange.EntireColumn.AutoFit();
                Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;



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
                MessageBox.Show(message, caption, MessageBoxButtons.OK);
            }
            //saveFileProgresslbl.Visible = false;

            //pbar.Visible = false;
            //pbar.Value = 0;
        }
        //********************************************************************************//

        //================================================================================//
        public static void setFont(CuSumAnalysisGUI frm)
        {
            foreach (Control c in frm.Controls)
            {
                
                getFont(c);
                if ((c is GroupBox))
                {
                    inBoxGroup(c);
                }
                if (c is TabControl)
                {
                    foreach (TabPage p in c.Controls)
                    {
                        foreach (Control cc in p.Controls)
                        {
                            getFont(cc);
                            if ((cc is GroupBox))
                            {
                                inBoxGroup(cc);  
                            }
                            if (cc is Panel)
                            {
                                //MessageBox.Show(cc.Name);
                                foreach (Control pp in cc.Controls)
                                {
                                    getFont(pp);
                                    if (( pp is GroupBox))
                                        inBoxGroup(pp);   
                                }
                            }
                        }
                    }
                }//is tabcontrol


            }//for each


        }
        //********************************************************************************//

        //================================================================================//
        private static void getFont(Control cc)
        {

            string fonttype = "Microsoft Sans Serif";
            int fontSize = 8;
            if (cc is GroupBox)
            {
                switch (cc.Tag)
                {
                    case "tagPlotBox" :
                        cc.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold); //new Font();
                        cc.ForeColor = Color.Purple;
                        break;
                    case "tagFileBox":
                        cc.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold); //new Font();
                        cc.ForeColor = Color.Purple;
                        break;
                    default:
                        cc.Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold); //new Font();
                        cc.ForeColor = Color.Navy;
                        break;
                }

            }
            if (cc is Label)
            {
                switch (cc.Tag)
                {
                    case "tagIP":
                        cc.Font = new Font(fonttype, fontSize + 2, FontStyle.Regular);
                        cc.ForeColor = Color.Black;
                        break;
                    case "tagOpenFile":
                        cc.Font = new Font(fonttype, fontSize , FontStyle.Bold);
                        cc.ForeColor = Color.MediumPurple;
                        break;
                    case "operation":
                        cc.Font = new Font(fonttype, 12 , FontStyle.Bold);
                        cc.ForeColor = Color.White;
                        break;

                    case "tagIPLabel":
                        cc.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
                        cc.ForeColor = Color.DarkMagenta;
                        break;
                    case "PatientInfo":
                        cc.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
                        cc.ForeColor = Color.DarkOliveGreen;
                        break;
                    case "trainingLabel":
                        cc.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
                        cc.ForeColor = Color.DarkMagenta;
                        break;
                    case "tagTitle":
                        cc.Font = new Font(fonttype, 27, FontStyle.Bold);
                        cc.ForeColor = Color.Blue;
                        break;
                    case "tagDashBoard":
                        cc.Font = new Font(fonttype, 12, FontStyle.Bold);
                        cc.ForeColor = Color.DarkGoldenrod;
                        break;
                    case "tagFileBox:":
                        cc.Font = new Font(fonttype, fontSize, FontStyle.Regular);
                        cc.ForeColor = Color.Black;
                        break;
                    default:
                        cc.Font = new Font(fonttype, fontSize, FontStyle.Regular);
                        cc.ForeColor = Color.Black;
                        break;
                }
            }
    
            if (cc is Button)
            {
                cc.Font = new Font(fonttype, fontSize, FontStyle.Regular);
                cc.ForeColor = Color.Black;
            }
            if (cc is TabControl)
            {
                cc.Font = new Font(fonttype, fontSize, FontStyle.Regular);
                cc.ForeColor = Color.Black;
            }
            if (cc is ComboBox)
            {
                cc.Font = new Font(fonttype, fontSize, FontStyle.Regular);
                cc.ForeColor = Color.Black;
            }
            if (cc is RadioButton)
            {
                cc.Font = new Font(fonttype, fontSize, FontStyle.Regular);
                cc.ForeColor = Color.Black;
                if (cc.Tag == "CEPOD")
                {
                    cc.Font = new Font(fonttype, fontSize, FontStyle.Bold);
                    cc.ForeColor = Color.Black;
                }
            }
            if (cc is TextBox)
            {
                cc.Font = new Font(fonttype, fontSize, FontStyle.Regular);
                cc.ForeColor = Color.Black;
            }
            if (cc is CheckBox)
            {
                cc.Font = new Font(fonttype, fontSize, FontStyle.Regular);
                cc.ForeColor = Color.Black;
            }
            if (cc is NumericUpDown)
            {
                cc.Font = new Font(fonttype, fontSize, FontStyle.Bold);
                cc.ForeColor = Color.Black;
            }
            if (cc is DataGridView)
            {
                cc.Font = new Font(fonttype, fontSize, FontStyle.Regular);
                cc.ForeColor = Color.Black;
            }
        }
        //********************************************************************************//

        //================================================================================//
        private static void inBoxGroup(Control c)
        {
            foreach (Control cc in c.Controls)
            {
                getFont(cc);
                if (cc is GroupBox)
                {
                    inBoxGroup(cc);
                }
                if (cc is Panel)
                    foreach (Control cpp in cc.Controls)
                        getFont(cpp);
            }
        }
        //********************************************************************************//

       //================================================================================//
       public int getSelectedTabIndex(TabControl tc)
        {
            return tc.SelectedIndex;
        }
        //********************************************************************************//

        //================================================================================//
        public static void changeSelect(DataGridViewCellMouseEventArgs e, DataGridView dgvProgressData,TextBox txtNavidationIndex)
        {
            
            if ((e.Button == MouseButtons.Right) || (e.Button == MouseButtons.Left))
            {
                //progressDataGridView.CurrentCell = progressDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dgvProgressData.ClearSelection();
                if (e.RowIndex != -1)
                {
                    if (e.ColumnIndex == -1)
                    {
                        dgvProgressData.Rows[e.RowIndex].Cells[0].Selected = true;
                    }
                    else
                    {
                        dgvProgressData.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                        //csd.selectedColumn = e.ColumnIndex;
                    }
                    if (e.RowIndex > dgvProgressData.Rows.Count - 2)
                    {
                        txtNavidationIndex.Text = "New Row";
                    }
                    else
                    {
                        txtNavidationIndex.Text = (e.RowIndex + 1).ToString() + " of " + (dgvProgressData.Rows.Count - 1).ToString();
                    }
                }
                //csd.selectedRow = e.RowIndex;
                //csd.displaySelectedRowToBufferGDV();
                //MessageBox.Show("selected Row" + e.RowIndex.ToString(), csd.dt.Rows.Count.ToString(), MessageBoxButtons.OK);
            }
        }
        //********************************************************************************//

        //================================================================================//
        public static void checkTextInput(ComboBox cbx, string fileName, string roleType)
        {
            string inputName = cbx.Text;
            if (inputName.Trim().Length > 0)
            {
                inputName = inputName.Substring(0, 1).ToUpper() + inputName.Substring(1, inputName.Length - 1).ToLower();
                bool isExisting = false;
                for (int i = 0; i < cbx.Items.Count; i++)
                {
                    if (inputName.ToUpper() == cbx.Items[i].ToString().ToUpper())
                    {
                        Console.WriteLine("Same");
                        cbx.SelectedIndex = i;
                        isExisting = true;
                        break;
                    }
                }//for
                if (isExisting == false)
                {
                    if (MessageBox.Show(roleType + " " + inputName + " is not in the list, add it?", "Add New "+ roleType, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        cbx.Items.Add(inputName);
                        cbx.SelectedIndex = cbx.Items.Count - 1;
                        using (StreamWriter w = File.AppendText(fileName))
                        {
                            w.WriteLine(inputName);
                        }
                    }//if
                    else
                    {
                        cbx.Text = "";
                    }

                }
            }
        }//checktextinput 
        //********************************************************************************//

        //================================================================================//
        public static ImageList addIcons()
        {
            ResourceManager resources = new ResourceManager(typeof(CuSumAnalysisGUI));
            ImageList iconsList = new ImageList();
            iconsList.ImageSize = new Size(60, 60);
            iconsList.TransparentColor = Color.Blue;
            iconsList.ColorDepth = ColorDepth.Depth32Bit;
            //string path = "C:\Users\test\source\repos\CuSum\CuSum Analysis\bin\Debug\";
            iconsList.Images.Add(Image.FromFile(@".\images\personal-trainer.png"));
            iconsList.Images.Add(Image.FromFile(@".\images\personal-trainer-2.jpg"));
            iconsList.Images.Add(Image.FromFile(@".\images\training-2.jpg"));
            iconsList.Images.Add(Image.FromFile(@".\images\graph-chart-2.jpg"));
            iconsList.Images.Add(Image.FromFile(@".\images\personal-trainer-selected.jpg"));
            iconsList.Images.Add(Image.FromFile(@".\images\training-selected.jpg"));
            iconsList.Images.Add(Image.FromFile(@".\images\graph-chart-selected.jpg"));
            return iconsList;
        }
        //********************************************************************************//

        //================================================================================//
        public static void setButtonIcon(Button btn,string text, string iconFile, ToolTip tTip, string toolText)
        {
            btn.BackgroundImage = Image.FromFile(iconFile);
            btn.Text = text;
            btn.BackgroundImageLayout = ImageLayout.Stretch;
            tTip.SetToolTip(btn, toolText);
        }
        //********************************************************************************//


    }//class
}
