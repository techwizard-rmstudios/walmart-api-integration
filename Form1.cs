
/// *************************************************************
/// *             Coded by Ekaterina Bozhko                     *
/// *             bozhkokateryna12@gmail.com                    *
/// *************************************************************

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WalmartAPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public WalmartAuth WalmartAPI = new WalmartAuth("7946bdd0-0fdb-495b-b0bd-e1cf46734d73", "GdAPU36SuzOXdzPAtYMIXG-vnYltUW2wihEXREfLUvyHwoQ6XBcdphYuQ8A_RdjKsevu4Z8tNefoVcQTONcuRg");
        public InventoryManager InventoryManager = new InventoryManager();

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;

                textBox4.Text = selectedFilePath;
                richTextBox1.Text = File.ReadAllText(selectedFilePath);
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string accessToken = await WalmartAPI.GetTokenAsync();
            Feed feed = await InventoryManager.PostInventory(accessToken, textBox4.Text.ToString());

            try
            {
                richTextBox1.Text = JsonConvert.SerializeObject(feed);
            }
            catch
            {

            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string accessToken = await WalmartAPI.GetTokenAsync();
            Inventory inventory = await InventoryManager.GetInventory(accessToken, textBox1.Text);

            try
            {
                textBox2.Text = inventory.quantity.unit;
                textBox3.Text = inventory.quantity.amount.ToString();
            }
            catch
            {

            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string accessToken = await WalmartAPI.GetTokenAsync();
            Inventory inventory = await InventoryManager.PutInventory(accessToken, textBox1.Text, textBox2.Text, int.Parse(textBox3.Text));

            try
            {
                textBox2.Text = inventory.quantity.unit;
                textBox3.Text = inventory.quantity.amount.ToString();
            }
            catch
            {

            }
        }
    }
}
