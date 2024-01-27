using InputManager.Device;

namespace InputTesterApp
{
    public partial class Form1 : Form
    {
        private DeviceManager _manager = new();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshDevices();
        }

        private void RefreshDevices()
        {
            var devices = _manager.LoadConnectedDevices(true);

            comboBox1.DisplayMember = "Name";

            foreach (var device in devices)
            {
                comboBox1.Items.Add(device);
            }

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ListenToSelected();
        }

        private void ListenToSelected()
        {
            var selectedItem = comboBox1.SelectedItem as DeviceInstance;
            var watcher = new InputManager.Input.InputListener(selectedItem, new SharpDX.DirectInput.DirectInput());

            try
            {
                var buttonReturned = watcher.RegisterButton();

                if (!watcher.IsCancelled)
                {
                    lblResult.Text = $"{selectedItem.Name} is supported";
                    lblResult.ForeColor = Color.Green;
                    MessageBox.Show("Your device is supported!", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    lblResult.Text = $"No input was received from {selectedItem.Name}";
                    lblResult.ForeColor = Color.Red;
                    MessageBox.Show($"No inputs were received from your device.{Environment.NewLine}Contact us so we can work on supporting your device.", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception)
            {
                lblResult.Text = $"{selectedItem.Name} is not supported";
                lblResult.ForeColor = Color.Red;
                MessageBox.Show("Your device is not supported.", "Sorry");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblResult.ForeColor = Color.Black;
            lblResult.Text = "Press a button on your device...";
            this.Refresh();
            ListenToSelected();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            comboBox1.Items.Clear();
            RefreshDevices();
            MessageBox.Show("Refreshed");
        }
    }
}
