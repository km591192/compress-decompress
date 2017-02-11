using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace comp_decomp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           checkBox1.Visible = false;
        }
        Controler control = new Controler();
        Classcontrol classcontrol = new Classcontrol();
        Classcontrol2 classcontrol2 = new Classcontrol2();
        ClassControl24 cc24 = new ClassControl24();
        LZWcd lzwcd = new LZWcd();
        LZW_cd_i lzwcdi = new LZW_cd_i();
        LZW_cd_i2 lzwcdi2 = new LZW_cd_i2();
        LZWIControll lzwicontrol = new LZWIControll();
        Hcontrol hufcontrol = new Hcontrol();
        string name;

        private void btnchoose_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.Equals("RLE"))
            name = control.open(pbprimary, "*.bmp;*.tiff");
            if (comboBox1.SelectedItem.Equals("RLE(2)"))
                name = control.open(pbprimary, "*.bmp;*.tiff");

            if (comboBox1.SelectedItem.Equals("LZW"))
                if (checkBox2.Checked == true)
                    name = control.openfile(tbinput, "*.txt;");
            if (checkBox2.Checked == false)
                name = control.open(pbprimary, "*.bmp;*.tiff;*.jpg;");

            if (comboBox1.SelectedItem.Equals("Huffman"))
                name = control.open(pbprimary, "*.bmp;*.tiff");
        }

        private void btncompress_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true && comboBox1.SelectedItem.Equals("RLE"))
            {
                control.compressWithBitMap(name);
                //cc24.compressrle24("r.txt", "g.txt", "b.txt", tbinfo);
                cc24.Encode();
            }

            if (comboBox1.SelectedItem.Equals("RLE") && checkBox1.Checked == false)
            classcontrol.compressrle(name,tbinfo);

            if (comboBox1.SelectedItem.Equals("RLE(2)") && checkBox1.Checked == false)
                classcontrol2.compressrle(name, tbinfo);

            if (comboBox1.SelectedItem.Equals("LZW") && checkBox2.Checked == true)
                lzwcd.TestCompress(name, tbinput,tbinfo);
            if (comboBox1.SelectedItem.Equals("LZW") && checkBox2.Checked == false)
                lzwicontrol.TestCompress(name, tbinfo);
                // lzwcdi.TestCompress(name, tbinfo);
            if (comboBox1.SelectedItem.Equals("Huffman") && checkBox1.Checked == false)
                hufcontrol.TestCompress(name, tbinfo);
  
        }

        private void btndecompress_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true && comboBox1.SelectedItem.Equals("RLE"))
            {
               // cc24.decompressrle24(pbdecompress,tbinfo);
                cc24.Decode();
            }

            if (comboBox1.SelectedItem.Equals("RLE") && checkBox1.Checked == false)
                classcontrol.decompressrle(pbdecompress, tbinfo);
            if(comboBox1.SelectedItem.Equals("RLE(2)") && checkBox1.Checked == false)
            classcontrol2.decompressrle(pbdecompress,tbinfo);

            if (comboBox1.SelectedItem.Equals("LZW") && checkBox2.Checked == true)
                lzwcd.TestDecompres(tboutput,tbinfo);
            if (comboBox1.SelectedItem.Equals("LZW") && checkBox2.Checked == false)
               // lzwicontrol.TestDecompress(pbdecompress, tbinfo);
                lzwcdi.TestDecompres(pbdecompress ,tbinfo);
            if (comboBox1.SelectedItem.Equals("Huffman") && checkBox1.Checked == false)
                hufcontrol.TestDecompress(pbdecompress, tbinfo);
             
        }

       

       
    }
}
