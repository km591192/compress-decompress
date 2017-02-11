using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace comp_decomp
{
    public class Controler
    {
        public string open(PictureBox pb, string file)
        {
            PictureBox imagecontrol = new PictureBox();
            imagecontrol.Height = 400;
            imagecontrol.Width = 400;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "File (" + file + ")|" + file;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Image inFile = Image.FromFile(ofd.FileName);
                    pb.SizeMode = PictureBoxSizeMode.CenterImage;
                    pb.Image = inFile;
                    MessageBox.Show("File load");
                }
            return ofd.FileName;
        }

        public string openfile(TextBox tb, string file)
        {
            PictureBox imagecontrol = new PictureBox();
            imagecontrol.Height = 400;
            imagecontrol.Width = 400;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "File (" + file + ")|" + file;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tb.Text = File.ReadAllText(ofd.FileName);
                MessageBox.Show("File load");
            }
            return ofd.FileName;
        }


        string sfdfilename;
       // public void save(TextBox tbinfo, string file)   
        public string save(TextBox tbinfo, string file)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Images (" + file + ")|" + file;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                RichTextBox richtextBox1 = new RichTextBox();
                richtextBox1.SaveFile(sfd.FileName);
                sfdfilename = sfd.FileName;

                tbinfo.Text += "Save File with path:" + sfd.FileName;
            }
            return sfdfilename;
        }

        public void compressWithBitMap(string inFile)
        {
            Bitmap bm = new Bitmap(inFile);
            FileStream a = new FileStream("a.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sa = new StreamWriter(a);
            FileStream g = new FileStream("g.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sg = new StreamWriter(g);
            FileStream r = new FileStream("r.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sr = new StreamWriter(r);
            FileStream b = new FileStream("b.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sb = new StreamWriter(b);
            string strg="", strr="", strb="",stra="";
            for (int i = 0; i < bm.Width; i++)
            {
                for (int j = 0; j < bm.Height; j++)
                {
                   /* if (bm.GetPixel(i, j).ToArgb() == Color.Black.ToArgb())
                    { sinf.Write("B"); }*/

                    Color color = bm.GetPixel(i, j);
                   // byte ba = color.A;
                    byte rb = color.R;
                    byte gb = color.G;
                    byte bb = color.B;
                    char cr = Convert.ToChar(rb);
                    char cg = Convert.ToChar(gb);
                    char cb = Convert.ToChar(bb);
                    stra = stra + cr+cg+cb;
                    strg  =  strg + cg ;
                    strr = strr + cr;
                    strb = strb + cb;

                }
            }

            sg.Write(strg);
            sr.Write(strr);
            sb.Write(strb);
            sa.Write(stra);
            
            sg.Dispose();
            sg.Close();
            g.Dispose();
           g.Close();
           sr.Dispose();
           sr.Close();
           r.Dispose();
           r.Close();
           sb.Dispose();
           sb.Close();
           b.Dispose();
           b.Close();
           sa.Dispose();
           sa.Close();
           a.Dispose();
           a.Close();
        }

      
    }
}
