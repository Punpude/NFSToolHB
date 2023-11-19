using NFSLocaleTool;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp1
{


    public partial class Form1 : Form
    {
        private NFSLocale nfs = new NFSLocale();
        public Form1()
        {
            InitializeComponent();
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult rsl = MessageBox.Show("Are you agree?", " Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (rsl == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        string fileContent = string.Empty;
        public string filePath = string.Empty;
        public string fileNameText = string.Empty;
        private void LoadFile(string txt)
        {

            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (txt=="txt")
            {
                openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.FileName = "";
                openFileDialog1.RestoreDirectory = false;
            }
            else if (txt=="chunk")
            {
                openFileDialog1.Filter = "chunk files (*.chunk)|*.chunk|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.FileName = "";
            }
            else if (txt=="ids")
            {
                openFileDialog1.Filter = "ids files (*.ids)|*.ids|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                if (textBox3.Text!=string.Empty)
                {
                    openFileDialog1.FileName = textBox3.Text + ".ids";
                }
                else
                {
                    openFileDialog1.FileName = "";
                }
                
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    filePath = openFileDialog1.FileName;
                    //fileNameText = Path.GetFileNameWithoutExtension(openFileDialog1.InitialDirectory);


                    //var fileStream = openFileDialog1.OpenFile();

                    // read file or to send code
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error, don't open file: " + ex.Message);
                }
            }

        }

        private void compileANewHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFile("chunk");
        }


        string histogram_name = string.Empty;
        //histogram_original
        private void button2_Click(object sender, EventArgs e)
        {
            LoadFile("chunk");
            histogram_name = openFileDialog1.FileName;
            textBox2.Text = openFileDialog1.SafeFileName;
        }

        //binary
        string binary_name = string.Empty;
        private void button3_Click(object sender, EventArgs e)
        {
            LoadFile("chunk");
            binary_name = openFileDialog1.FileName;
            textBox3.Text = openFileDialog1.SafeFileName;
        }

        string text_name = string.Empty;
        //text
        private void button5_Click(object sender, EventArgs e)
        {
            LoadFile("txt");//?
            text_name = openFileDialog1.FileName;
            textBox5.Text = openFileDialog1.SafeFileName;
        }

        //mod hist
        string mod_histogram_name = string.Empty;
        private void button1_Click(object sender, EventArgs e)
        {
            LoadFile("chunk");
            mod_histogram_name = openFileDialog1.FileName;
            textBox1.Text = openFileDialog1.SafeFileName;
        }

        



        //histogram_original
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        //binary
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //text
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        //mod hist
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //mod text
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        //mod chars
        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }






        string filename_chars_save = string.Empty;
        //CreateListChars
        private void button8_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "Chars_list.txt";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            filename_chars_save = saveFileDialog1.FileName;
            //nfs.CreateListChars("Chars_list.txt");
            nfs.CreateListChars(filename_chars_save);
        }



        string filename_histogram_save = string.Empty;
        //CreateNewHistogram <inputcharstext> <inputhistogramchunk>  <outputhistogramchunkfile>
        private void button10_Click(object sender, EventArgs e)
        {
            //"new" + histogram_name + ".chunk"
            try
            {
                saveFileDialog1.FileName = "";
                saveFileDialog1.Filter = "chunk files (*.chunk)|*.chunk|All files (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                filename_histogram_save = saveFileDialog1.FileName;
                nfs.HistogramWrite(mchars_name, histogram_name, filename_histogram_save);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            //LoadFile(false);
            //nfs.HistogramWrite("Chars_list.txt", textBox2.Text, "new" + histogram_name + ".chunk");
        }




        string filename_text_save = string.Empty;
        //CreateTextfile <inputbinarychunk> <inputhistogramchunk> <outputtextfile>
        private void button7_Click(object sender, EventArgs e)
        {
            //"text_" + binary_name + ".txt"
            try
            {
                saveFileDialog1.FileName = "";
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                filename_text_save = saveFileDialog1.FileName;
                if (mod_histogram_name==string.Empty)
                {
                    nfs.Read(binary_name, histogram_name);
                }
                else
                {
                    nfs.Read(binary_name, mod_histogram_name);
                }
                
                nfs.ExtractText(filename_text_save);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            //nfs.Read(textBox3.Text, textBox2.Text);
            //nfs.ExtractText("text_"+ binary_name + ".txt");

        }
        //mod chars
        string mchars_name = string.Empty;
        private void button11_Click(object sender, EventArgs e)
        {
            LoadFile("txt");
            mchars_name = openFileDialog1.FileName;
            textBox7.Text = openFileDialog1.SafeFileName;
        }
        //ids
        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {

        }
        string ids_name = string.Empty;
        private void button4_Click(object sender, EventArgs e)
        {
            LoadFile("ids");
            ids_name = openFileDialog1.FileName;
            if (ids_name != ".ids")
            {
                textBox4.Text = openFileDialog1.SafeFileName;
            }
        }

        string filename_binary_save = string.Empty;
        //CreateBinary <inputtextfile> <inputhistogramchunk> <outputbinarychunkfile> <inputidsfile> 
        private void button9_Click(object sender, EventArgs e)
        {
            //"new" + text_name + ".chunk"
            try
            {
                saveFileDialog1.FileName = "";
                saveFileDialog1.Filter = "chunk files (*.chunk)|*.chunk|All files (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                filename_binary_save = saveFileDialog1.FileName;

                nfs.WriteFromText(text_name, mod_histogram_name, filename_binary_save, ids_name);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            //nfs.WriteFromText(textBox5.Text, textBox2.Text, "new" + text_name + ".chunk", textBox3.Text + ".ids");
        }







        private void readmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/erdem1999erdem/NFSToolHB/tree/main");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click_1(object sender, EventArgs e)
        {

        }

        
    }
}
