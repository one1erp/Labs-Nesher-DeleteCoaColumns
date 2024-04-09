using System;
using System.Drawing;
using System.Windows.Forms;
using Common;
using DAL;
using Telerik.WinControls.UI;

namespace DeleteCoaColumns
{
    public partial class FieldsForm : Telerik.WinControls.UI.RadForm
    {
        private DAL.Client client;
        private const int constY = 15;
        private long lid;
        private IDataLayer dal;


        public FieldsForm(long lid)
        {
            // TODO: Complete member initialization
            this.lid = lid;
            dal = new DataLayer();
            dal.Connect();


            COA_Report coa = dal.GetCoaReportById(lid);
            client = coa.Client;




            if (client != null && client.DefaultCOA_column != null)
            {
                InitializeComponent();
                var fields = client.DefaultCOA_column.Split(';');
                foreach (var field in fields)
                {
                    if (!string.IsNullOrEmpty(field))
                    {
                        AddToGrid(field.Split('@')[0], field.Split('@')[1]);
                    }
                }
            }
            else
            {
                dal.Close();
                MessageBox.Show("לא נמצאו שדות ששיכים לתעודה.");
                this.Close();
                
                
                //this.Dispose();
            }
        }

        private const string nameColumn = "ColName";
        private const string HiddenColumn = "ColHidden";
        private const string RemoveColumn = "ColRemove";
        private void AddToGrid(string fieldName, string headerName)
        {
            GridViewRowInfo gridViewRowInfo = radGridView1.Rows.AddNew();
            gridViewRowInfo.Cells[RemoveColumn].Value = true;
            gridViewRowInfo.Cells[nameColumn].Value = headerName;
            gridViewRowInfo.Cells[HiddenColumn].Value = fieldName;


        }

        void cb_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {



        }

        public bool HasChanges = false;
        private void RadButton1Click1(object sender, EventArgs e)
        {


            foreach (var row in radGridView1.Rows)
            {
                if ((bool)row.Cells[RemoveColumn].Value == false)
                {
                    RemoveFromCoa(row.Cells[HiddenColumn].Value.ToString(), row.Cells[nameColumn].Value.ToString());
                }
            }
            dal.SaveChanges();
            dal.Close();

            string msg = string.Format("הורדו מהתעודה {0} שדות.", NumberOfRemoved);
            if (HasChanges)            
            MessageBox.Show(msg);
            this.Close();
        }

        private int NumberOfRemoved = 0;
        private void RemoveFromCoa(string fn, string hn)
        {
            HasChanges = true;
            var str = fn + "@" + hn + ";";

            if (client.DefaultCOA_column.Contains(str))
            {
                var newStr = client.DefaultCOA_column.Replace(str, "");
                client.DefaultCOA_column = newStr;
                NumberOfRemoved++;
            }

        }



        private void radButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radPanel1_Resize(object sender, EventArgs e)
        {
            radPanel1.Location = new Point(Width / 2 - radPanel1.Width / 2, radPanel1.Location.Y);
            label1.Location = new Point(Width / 2 - radPanel1.Width / 2, radPanel1.Location.Y);
        }


    }
}
