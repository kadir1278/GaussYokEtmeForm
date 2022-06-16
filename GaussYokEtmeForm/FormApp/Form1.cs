using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace FormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double FirstA = Convert.ToDouble(txtA_Bir.Text);
                double SecondA = Convert.ToDouble(txtA_Iki.Text);
                double ThirdA = Convert.ToDouble(txtA_Uc.Text);
                double Answer1 = Convert.ToDouble(txtCevap_Bir.Text);

                double FirstB = Convert.ToDouble(txtB_Bir.Text);
                double SecondB = Convert.ToDouble(txtB_Iki.Text);
                double ThirdB = Convert.ToDouble(txtB_Uc.Text);
                double Answer2 = Convert.ToDouble(txtCevap_Iki.Text);

                double FirstC = Convert.ToDouble(txtC_Bir.Text);
                double SecondC = Convert.ToDouble(txtC_Iki.Text);
                double ThirdC = Convert.ToDouble(txtC_Uc.Text);
                double Answer3 = Convert.ToDouble(txtCevap_Uc.Text);


                double[,] matris = new double[3, 3];
                double[] deger = new double[3];


                matris[0, 0] = FirstA;
                matris[0, 1] = FirstB;
                matris[0, 2] = FirstC;

                matris[1, 0] = SecondA;
                matris[1, 1] = SecondB;
                matris[1, 2] = SecondC;

                matris[2, 0] = ThirdA;
                matris[2, 1] = ThirdB;
                matris[2, 2] = ThirdC;

                deger[0] = Answer1;
                deger[1] = Answer2;
                deger[2] = Answer3;



                var sonuclar = GaussEleme(matris, deger);


                double hava = 0;
                double kahvalti = 0;
                double borsa = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (i == 0)
                        hava = sonuclar[i];
                    else if (i == 1)
                        kahvalti = sonuclar[i];
                    else if (i == 2)
                        borsa = sonuclar[i];
                }

                txtA.Text = hava.ToString();
                txtB.Text = kahvalti.ToString();
                txtC.Text = borsa.ToString();
                txtError.Text = "";
            }
            catch (Exception)
            {
                txtError.Text = "Tüm Alanları Doldurunuz";
            }
        }

        static double[] GaussEleme(double[,] matris, double[] deger)
        {
            for (int i = 0; i < 3; i++)
            {
                double geciciIlk = matris[i, i];
                for (int k = 0; k < 3; k++)
                {
                    matris[i, k] /= geciciIlk;
                }
                deger[i] /= geciciIlk;


                for (int j = i + 1; j < 3; j++)
                {
                    double carpim = matris[j, i] / matris[i, i];

                    for (int l = 0; l < 3; l++)
                    {
                        matris[j, l] = matris[j, l] - (carpim * matris[i, l]);
                    }
                    deger[j] = deger[j] - (carpim * deger[i]);
                }
            }

            double[] sonuclar = new double[3];
            sonuclar[3 - 1] = deger[3 - 1];

            for (int i = 3 - 2; i >= 0; i--)
            {
                double toplam = 0;
                for (int j = i + 1; j < 3; j++)
                {
                    toplam += matris[i, j] * sonuclar[j];
                }
                sonuclar[i] = deger[i] - toplam;
            }

            return sonuclar;
        }

        private void btnCevap_Click(object sender, EventArgs e)
        {
            try
            {

                double A, B, C;
                A = Convert.ToDouble(txtA_Dort.Text) * Convert.ToDouble(txtA.Text);
                B = Convert.ToDouble(txtB_Dort.Text) * Convert.ToDouble(txtB.Text);
                C = Convert.ToDouble(txtC_Dort.Text) * Convert.ToDouble(txtC.Text);

                txtCevap_Dort.Text = (A + B + C).ToString();
            }
            catch (Exception)
            {
                txtError.Text = "Cevap Hesaplanamadı. ";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OleDbConnection baglanti;
            OleDbCommand komut;
            OleDbDataAdapter da;
            string sorgu = "INSERT INTO GaussYokEtme(Gun,A Degeri,B Degeri,C Degeri,Cevap) values (@gun,@adeger,@bdeger,@cdeger,@cevap)";
            baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=GaussYokEtme.accdb");
            komut = new OleDbCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@gun", "1");
            komut.Parameters.AddWithValue("@adeger", txtA_Bir.Text);
            komut.Parameters.AddWithValue("@bdeger", txtB_Bir.Text);
            komut.Parameters.AddWithValue("@cdeger", txtC_Bir.Text);
            komut.Parameters.AddWithValue("@cevap", txtCevap_Bir.Text);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
        }
    }
}
