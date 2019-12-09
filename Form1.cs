using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DealerOnV1
{
    public partial class Form1 : Form
    {
        ObservableCollection<Item> items = new ObservableCollection<Item>();
        decimal salesTax = 0;
        decimal total = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Item item = new Item();
            //Validation
            if (txtItemDescritption.Text == string.Empty)
            {
                MessageBox.Show("Item Description is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                decimal price = Convert.ToDecimal(txtPrice.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Item Price must be a number in format ###.##!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Init Item object
            item.ItemDescription = txtItemDescritption.Text;
            item.ItemCategory = cbCategory.SelectedItem.ToString();
            item.ItemPrice = Convert.ToDecimal(txtPrice.Text);
            //Check if Item is imported
            if (radNo.Checked) { item.IsImported = false; }
            else { item.IsImported = true; }
           
            //Check if Item is tax exemt 
            if (item.ItemCategory.Equals("Other")) { item.ItemSalesTax = item.ItemPrice * .10M; }
            else { item.ItemSalesTax = 0; }
        
            //Calc tax
            if (item.IsImported) { item.ItemImportTax = item.ItemPrice * .05M; }
            else { item.ItemImportTax = 0; }
            item.ItemTotal = item.ItemSalesTax + item.ItemImportTax + item.ItemPrice;
            salesTax += item.ItemSalesTax + item.ItemImportTax;
            lblSalesTax.Text = salesTax.ToString("C");
            items.Add(item);

            total += item.ItemTotal;         
            lblTotal.Text = total.ToString("C");
            lbCurrentItems.Items.Add($"{item.ItemDescription} {item.ItemTotal.ToString("C")}");


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //init items 
            cbCategory.SelectedIndex = 0;
            radNo.Checked = true;
            lblSalesTax.Text = salesTax.ToString("C");
            lblTotal.Text = total.ToString("C");
            lbFinal.Enabled = false;
            btnNew.Enabled = false;
            
           
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //remove item from list, update totals and validate
            try
            {
                total -= items[lbCurrentItems.SelectedIndex].ItemTotal;
                salesTax -= items[lbCurrentItems.SelectedIndex].ItemSalesTax + items[lbCurrentItems.SelectedIndex].ItemImportTax;
                lblTotal.Text = total.ToString("C");
                lblSalesTax.Text = salesTax.ToString("C");
                items.RemoveAt(lbCurrentItems.SelectedIndex);
                lbCurrentItems.Items.RemoveAt(lbCurrentItems.SelectedIndex);

            }
            catch (Exception)
            {
                MessageBox.Show("Must Select an item to remove it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
          
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //print final receipt 
            lbFinal.Enabled = true;
            btnNew.Enabled = true;
            ObservableCollection<Item> dupItems = new ObservableCollection<Item>();
            foreach(Item loopItem in items)
            {
                //count how many times item is in the list
                int count = items.Count((x => x.IsImported == loopItem.IsImported && x.ItemCategory == loopItem.ItemCategory && x.ItemDescription == loopItem.ItemDescription && x.ItemPrice == loopItem.ItemPrice));

                //if item is a duplicate calc totals
                if (count > 1)
                {
                    if(dupItems.Count(x => x.IsImported == loopItem.IsImported && x.ItemCategory == loopItem.ItemCategory && x.ItemDescription == loopItem.ItemDescription && x.ItemPrice == loopItem.ItemPrice) == 0)
                    {
                        lbFinal.Items.Add($"{loopItem.ItemDescription} {(loopItem.ItemTotal * count).ToString("C")} ({count} @ {(loopItem.ItemTotal).ToString("C")})");
                        dupItems.Add(loopItem);
                    }
                }
                else
                {
                    lbFinal.Items.Add($"{loopItem.ItemDescription} {loopItem.ItemTotal.ToString("C")}");
                }
                
            }
            //print totals
            lbFinal.Items.Add($"Sales Tax: {salesTax.ToString("C")}");
            lbFinal.Items.Add($"Total: {total.ToString("C")}");


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //set everthing back to starting values & validation if no data
            cbCategory.SelectedIndex = 0;
            radNo.Checked = true;
            total = 0;
            salesTax = 0;
            lblSalesTax.Text = total.ToString("C");
            lblTotal.Text = salesTax.ToString("C");

            lbFinal.Enabled = false;
            btnNew.Enabled = false;
            txtItemDescritption.Text = string.Empty;
            txtPrice.Text = string.Empty;
            try
            {
                lbCurrentItems.Items.Clear();
                items.Clear();
                lbFinal.Items.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
