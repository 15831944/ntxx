﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EntityFrameworkTryBLL;
using EntityFrameworkTryBLL.XuanxingManager;

namespace Annon.Xuanxing
{

    public partial class QuotedPriceSpecialInformation : Form
    {
        private double sum=0;
        private string propertyName { set; get; }
        private string valueCode { set; get; }
        public QuotedPriceSpecialInformation()
        {
            InitializeComponent();
            dataGridView_Load(null, null);
        }
        public int PropertyId
        { get; set; }
        public int ValueCodeId
        {
            get;
            set;
        }

        public void setPrice(int propertyId, int valueCodeId)
        {
            PropertyId = propertyId;
            ValueCodeId = valueCodeId;
        }
        public QuotedPriceSpecialInformation(string propertyName,string valueCode)
        {
            InitializeComponent();
            dataGridView_Load(null, null);
            this.propertyName = propertyName;
            this.valueCode = valueCode;
        }

        public QuotedPriceSpecialInformation(int OrderID, string ModelPropertyName, string ProCode,string valueDescription)
        {
            InitializeComponent();
            this.orderId = OrderID;
            this.modelPropertyName = ModelPropertyName;
            this.proCode = ProCode;
            this.valueDescription = valueDescription;
        }
        private int orderId;
        private string modelPropertyName;
        private string proCode;
        private string valueDescription;


        private void dataGridView_Load(object sender, EventArgs e)
        {
            this.dataGridView1.Columns.Add("description", "Description");

            this.dataGridView1.Columns.Add("list", "List");
            this.dataGridView1.Columns.Add("spaNo", "SPA No.");

            for (int j = 0; j < this.dataGridView1.ColumnCount; j++)
            {
                if (j == 0)
                    this.dataGridView1.Columns[j].Width = 230;
                else if (j == 1)
                {
                    this.dataGridView1.Columns[j].Width = 180;
                }
                else if (j == 2)
                {
                    this.dataGridView1.Columns[j].Width = 80;
                }

            }

            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            this.dataGridView1.ColumnHeadersHeight = this.dataGridView1.ColumnHeadersHeight * 2;

            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

            this.dataGridView1.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView1_CellPainting);

            this.dataGridView1.Paint += new PaintEventHandler(dataGridView1_Paint);

        }
        void dataGridView1_Paint(object sender, PaintEventArgs e)
        {

            string[] monthes = { "Special Infromation" };

            for (int j = 0; j < monthes.Length; )
            {

                Rectangle r1 = this.dataGridView1.GetCellDisplayRectangle(j, -1, true); //get the column header cell

                r1.X += 1;

                r1.Y += 1;

                r1.Width = (r1.Width + this.dataGridView1.GetCellDisplayRectangle(j + 1, -1, true).Width + this.dataGridView1.GetCellDisplayRectangle(j + 2, -1, true).Width) - 2;

                r1.Height = r1.Height / 2 - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor), r1);

                StringFormat format = new StringFormat();

                format.Alignment = StringAlignment.Center;

                format.LineAlignment = StringAlignment.Center;

                e.Graphics.DrawString(monthes[j / 2],

                    this.dataGridView1.ColumnHeadersDefaultCellStyle.Font,

                    new SolidBrush(this.dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor),

                    r1,

                    format);

                j += 2;

            }

        }

        void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {

                e.PaintBackground(e.CellBounds, false);



                Rectangle r2 = e.CellBounds;

                r2.Y += e.CellBounds.Height / 2;

                r2.Height = e.CellBounds.Height / 2;

                e.PaintContent(r2);

                e.Handled = true;

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //PropertyBLL.UpdatePrice(1, PropertyId, ValueCodeId, Convert.ToDecimal(textBox_Price.Text));
            //if(dataGridView1.SelectedRows.Count>0)
            foreach(DataGridViewRow dataRow in dataGridView1.Rows)
            {
                string partNo = (string)dataRow.Cells[0].Value;
                string partDescription = (string)dataRow.Cells[1].Value;
                string listPrice = (string)dataRow.Cells[2].Value;
                AccessoryBLL.insertIntoAccessoryOrder(orderId, partNo, partDescription, 1, Convert.ToInt32(listPrice));
            }
            CatalogBLL.saveOrder(1, orderId, modelPropertyName, proCode, Convert.ToDecimal(textBox_Price.Text));
            MessageBox.Show("价格更新成功！");
            //CatalogBLL.saveOrder(1, orderId, modelPropertyName, proCode, Convert.ToDecimal(textBox_Price.Text),valueDescription);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void dataGridView1Binding()
        {
            dataGridView1.DataSource = AccessoryBLL.getAccessories("Themostats");
        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2&&e.RowIndex>=0)
            {
                for (int i = 0; i <= e.RowIndex; i++)
                {
                    double price = dataGridView1.Rows[i].Cells[2].Value == null ? 0 : Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    sum += price;
                }
                textBox_Price.Text = "" + sum;
                sum = 0;
            }
        }
    }
}
