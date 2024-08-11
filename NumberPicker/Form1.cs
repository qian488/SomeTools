using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumberPicker
{
    public partial class Form1 : Form
    {
        private Random random = new Random();
        private System.Windows.Forms.Timer timer;
        private int minValue;
        private int maxValue;
        private bool isRolling = false;

        public Form1()
        {
            InitializeComponent();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 50; // ���ֹ������ٶ�
            timer.Tick += Timer_Tick;
        }

        private void startStopButton_Click(object sender, EventArgs e)
        {
            if (!isRolling)
            {
                // ��ʼ����
                if (int.TryParse(minValueTextBox.Text, out minValue) &&
                    int.TryParse(maxValueTextBox.Text, out maxValue) &&
                    minValue <= maxValue)
                {
                    isRolling = true;
                    startStopButton.Text = "Stop";
                    timer.Start();
                }
                else
                {
                    MessageBox.Show("��������Ч�ķ�Χ��");
                }
            }
            else
            {
                // ֹͣ��������ʾ���
                isRolling = false;
                startStopButton.Text = "Start";
                timer.Stop();
                int result = int.Parse(rollingNumberLabel.Text);
                ShowResult(result);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int randomNumber = random.Next(minValue, maxValue + 1);
            rollingNumberLabel.Text = randomNumber.ToString();
        }

        private async void ShowResult(int result)
        {
            rollingNumberLabel.Text = "";
            string resultText = result.ToString();
            foreach (char digit in resultText)
            {
                rollingNumberLabel.Text += digit;
                await Task.Delay(500); // ÿ��������ʾ���
            }
        }

    }
}