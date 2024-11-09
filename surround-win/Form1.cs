using System;
using System.Linq;
using System.Windows.Forms;
//using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;

namespace surround_win
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Button to list audio devices and select one
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            // Create an instance of the audio controller
            var audioController = new CoreAudioController();

            // Get all the active playback devices
            var devices = audioController.GetPlaybackDevices();

            // Add each device to the listbox
            foreach (var device in devices)
            {
                // Display device name and ID in the listbox
                listBox1.Items.Add(new { Name = device.FullName, DeviceId = device.Id });
            }
        }

        // Button to set selected audio device as default
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select an audio device first.");
                return;
            }

            // Get the selected device (object containing name and device ID)
            dynamic selectedDevice = listBox1.SelectedItem;
            string selectedDeviceName = selectedDevice.Name;
            Guid deviceId = selectedDevice.DeviceId;

            // Set the default audio device
            try
            {
                SetDefaultAudioDevice(deviceId);
                MessageBox.Show($"Attempted to set audio device '{selectedDeviceName}' as the default output.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Set the default audio output device using AudioSwitcher.AudioApi
        private void SetDefaultAudioDevice(Guid deviceId)
        {
            // Create an instance of the audio controller
            var audioController = new CoreAudioController();

            // Find the device by ID
            var device = audioController.GetPlaybackDevices().FirstOrDefault(d => d.Id == deviceId);

            if (device == null)
            {
                throw new Exception("Audio device not found.");
            }

            // Set the selected device as the default output device
            audioController.DefaultPlaybackDevice = device;

            // Display a success message
            MessageBox.Show($"Device '{device.FullName}' has been set as the default output device.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
