using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace surround_win
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Clear the ListBox before displaying new results
            listBox1.Items.Clear();

            // Initialize the enumerator to get audio devices
            var enumerator = new MMDeviceEnumerator();

            // Enumerate all audio output devices (render devices)
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            // Display each device in the ListBox
            foreach (var device in devices)
            {
                listBox1.Items.Add(device.FriendlyName);
            }
        }
    }
}
