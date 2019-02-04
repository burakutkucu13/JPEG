using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace TEZ_DENEME1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int ffff = 0;
        public int image_blocks = 0;
        public int burak;
        public int[,] GreyImage;             //GreyScale Image Array Generated from input Image
        public int matris_blok_sayac = 0;

        static Encoding targetEncoding = Encoding.GetEncoding(1252);
        public StreamWriter JPEG_File = new System.IO.StreamWriter(@"C:\Users\Burak\Desktop\JPEG.txt", true, targetEncoding);
        //public StreamWriter donusum_dct = new System.IO.StreamWriter(@"C:\Users\Burak\Desktop\donusum_dct.txt");
        public StreamWriter donusum_burak = new System.IO.StreamWriter(@"C:\Users\Burak\Desktop\donusum_burak.txt");
        public StreamWriter test = new System.IO.StreamWriter(@"C:\Users\Burak\Desktop\test.txt");

        #region Data Variables

        public double[,] temp = new double[8, 8];
        public char[] blok_tut = new char[8];
        public double onceki_DC_degeri = 0;
        public int sayiSatir = 0, sayiSütün = 0, artan_bit_sayisi = 0;
        public double[,] sifirSayilari_sifirsizHali;
        public double[] kategori;
        public string[,] huffman_dizisi;
        public double[,] quantization_table_Luminances = 
                                { { 16 ,11 ,10 ,16 ,24 ,40 ,51 ,61     }, 
                                  { 12 ,12 ,14 ,19 ,26 ,58 ,60 ,55     }, 
                                  { 14 ,13 ,16 ,24 ,40 ,57 ,69 ,56     }, 
                                  { 14 ,17 ,22 ,29 ,51 ,87 ,80 ,62     }, 
                                  { 18 ,22 ,37 ,56 ,68 ,109 ,103 ,77   },
                                  { 24 ,35 ,55 ,64 ,81 ,104 ,113 ,92   },
                                  { 49 ,64 ,78 ,87 ,103 ,121 ,120 ,101 },
                                  { 72 ,92 ,95 ,98 ,112 ,100 ,103 ,99  },
                                };


        public double[,] quantization_table_Chrominances =
        {
            {17 ,18, 24, 47 ,99 ,99 ,99 ,99 },
            {18 ,21 ,26, 66 ,99 ,99 ,99, 99 },
            {24 ,26 ,56, 99, 99, 99, 99 ,99 },
            {47, 66 ,99, 99, 99, 99 ,99 ,99 },
            {99 ,99, 99, 99 ,99 ,99 ,99 ,99 },
            {99, 99 ,99 ,99 ,99 ,99 ,99 ,99 },
            {99 ,99 ,99, 99 ,99 ,99 ,99 ,99 },
            {99, 99, 99, 99 ,99 ,99 ,99 ,99 }
        };

        public double[,] donüsümMatrisi =
                                { { 0.353553390593274 , 0.353553390593274 ,0.353553390593274 ,0.353553390593274 ,0.353553390593274 ,0.353553390593274 ,0.353553390593274 ,0.353553390593274 }, 
                                  { 0.490392640201615	,0.415734806151273   ,	0.277785116509801,	0.0975451610080642	,-0.0975451610080641,	-0.277785116509801,	-0.415734806151273,	-0.490392640201615}, 
                                  { 0.461939766255643	,0.191341716182545	  ,-0.191341716182545	,-0.461939766255643	,-0.461939766255643,	-0.191341716182545,	0.191341716182545,	0.461939766255643}, 
                                  { 0.415734806151273	,-0.0975451610080641  ,	-0.490392640201615	,-0.277785116509801	,0.277785116509801,	0.490392640201615,	0.0975451610080640,	-0.415734806151272}, 
                                  { 0.353553390593274	,-0.353553390593274  ,	-0.353553390593274	,0.353553390593274,	0.353553390593274,	-0.353553390593273	,-0.353553390593274,	0.353553390593273   },
                                  { 0.277785116509801	,-0.490392640201615  ,	0.0975451610080642	,0.415734806151273	,-0.415734806151273	,-0.0975451610080649,	0.490392640201615,	-0.277785116509801   },
                                  { 0.191341716182545	,-0.461939766255643 	,0.461939766255643	,-0.191341716182545	,-0.191341716182545,	0.461939766255644,	-0.461939766255644,	0.191341716182543 },
                                  { 0.0975451610080642	,-0.277785116509801  ,	0.415734806151273	,-0.490392640201615	,0.490392640201615,	-0.415734806151272,	0.277785116509802,	-0.0975451610080625}
                                };


        public double[,] donüsümMatrisiTranspoz = new double[8, 8];

        public byte[] raw_orjinal_bytes;

        #endregion

        #region Marker Variables

        public object a;
        public EventArgs b;

        public double[,] yeni = 
                                { { 100 ,100 ,100,100,100,100,100,100}, 
                                  { 100 ,100 ,100,100,100,100,100,100}, 
                                  { 100 ,100 ,100,100,100,100,100,100}, 
                                  { 100 ,100 ,100,100,100,100,100,100}, 
                                  { 100 ,100 ,100,100,100,100,100,100},
                                  { 100 ,100 ,100,100,100,100,100,100},
                                  { 100 ,100 ,100,100,100,100,100,100},
                                  { 100 ,100 ,100,100,100,100,100,100}
                                };

        //------------Standart Quantization Tables-->>Luminance(Y) and Chrominance(CbCr)--------------------
        int[] std_luminance_qt = { 
            16,  11,  10,  16,  24,  40,  51,  61,
            12,  12,  14,  19,  26,  58,  60,  55,
            14,  13,  16,  24,  40,  57,  69,  56,
            14,  17,  22,  29,  51,  87,  80,  62,
            18,  22,  37,  56,  68, 109, 103,  77,
            24,  35,  55,  64,  81, 104, 113,  92,
            49,  64,  78,  87, 103, 121, 120, 101,
            72,  92,  95,  98, 112, 100, 103,  99
            };

        public int scale_factor = 20;
        public double[,] son_hali = new double[8, 8];

        int[] std_chrominance_qt = {
	        17,  18,  24,  47,  99,  99,  99,  99,
	        18,  21,  26,  66,  99,  99,  99,  99,
	        24,  26,  56,  99,  99,  99,  99,  99,
	        47,  66,  99,  99,  99,  99,  99,  99,
	        99,  99,  99,  99,  99,  99,  99,  99,
	        99,  99,  99,  99,  99,  99,  99,  99,
	        99,  99,  99,  99,  99,  99,  99,  99,
	        99,  99,  99,  99,  99,  99,  99,  99
            };


        int[] zigzag = {
            0, 1, 5, 6,14,15,27,28,
		    2, 4, 7,13,16,26,29,42,
		    3, 8,12,17,25,30,41,43,
		    9,11,18,24,31,40,44,53,
		    10,19,23,32,39,45,52,54,
		    20,22,33,38,46,51,55,60,
		    21,34,37,47,50,56,59,61,
		    35,36,48,49,57,58,62,63 
                           };

        //----------Standart Huffman Tables-->>Luminance(Y) and Chrominance(CbCr)--------------------
        int[] std_dc_luminance_nrcodes = { 0, 0, 1, 5, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
        int[] std_dc_luminance_values = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        int[] std_dc_chrominance_nrcodes = { 0, 0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
        int[] std_dc_chrominance_values = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        int[] std_ac_luminance_nrcodes = { 0, 0, 2, 1, 3, 3, 2, 4, 3, 5, 5, 4, 4, 0, 0, 1, 0x7d };
        int[] std_ac_luminance_values = {
	  0x01, 0x02, 0x03, 0x00, 0x04, 0x11, 0x05, 0x12,
	  0x21, 0x31, 0x41, 0x06, 0x13, 0x51, 0x61, 0x07,
	  0x22, 0x71, 0x14, 0x32, 0x81, 0x91, 0xa1, 0x08,
	  0x23, 0x42, 0xb1, 0xc1, 0x15, 0x52, 0xd1, 0xf0,
	  0x24, 0x33, 0x62, 0x72, 0x82, 0x09, 0x0a, 0x16,
	  0x17, 0x18, 0x19, 0x1a, 0x25, 0x26, 0x27, 0x28,
	  0x29, 0x2a, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39,
	  0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49,
	  0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59,
	  0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
	  0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79,
	  0x7a, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89,
	  0x8a, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98,
	  0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7,
	  0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6,
	  0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3, 0xc4, 0xc5,
	  0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2, 0xd3, 0xd4,
	  0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xe1, 0xe2,
	  0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea,
	  0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
	  0xf9, 0xfa };

        int[] std_ac_chrominance_nrcodes = { 0, 0, 2, 1, 2, 4, 4, 3, 4, 7, 5, 4, 4, 0, 1, 2, 0x77 };
        int[] std_ac_chrominance_values = {
	  0x00, 0x01, 0x02, 0x03, 0x11, 0x04, 0x05, 0x21,
	  0x31, 0x06, 0x12, 0x41, 0x51, 0x07, 0x61, 0x71,
	  0x13, 0x22, 0x32, 0x81, 0x08, 0x14, 0x42, 0x91,
	  0xa1, 0xb1, 0xc1, 0x09, 0x23, 0x33, 0x52, 0xf0,
	  0x15, 0x62, 0x72, 0xd1, 0x0a, 0x16, 0x24, 0x34,
	  0xe1, 0x25, 0xf1, 0x17, 0x18, 0x19, 0x1a, 0x26,
	  0x27, 0x28, 0x29, 0x2a, 0x35, 0x36, 0x37, 0x38,
	  0x39, 0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48,
	  0x49, 0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58,
	  0x59, 0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68,
	  0x69, 0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78,
	  0x79, 0x7a, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87,
	  0x88, 0x89, 0x8a, 0x92, 0x93, 0x94, 0x95, 0x96,
	  0x97, 0x98, 0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5,
	  0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4,
	  0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3,
	  0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2,
	  0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda,
	  0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9,
	  0xea, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
	  0xf9, 0xfa };

        #endregion

        
        public double[,] donusum_matris = new double[8, 8];//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public double[,] donusum_matris_transpoz = new double[8, 8]; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public double[,] quant =
                                {
            {-415,-33,-58,35,58,-51,-15,-12},
            {5,-34,49,18,27,1,-5,3},
            {-46,14,80,-35,-50,19,7,-18},
            {-53,21,34,-20,2,34,36,12},
            {9,-2,9,-5,-32,-15,45,37},
            {-8,15,-16,7,-8,11,4,7},
            {19,-28,-2,-26,-2,7,-44,-21},
            {18,25,-12,-44,35,48,-37,-3}
                                };
            

        public void button1_Click(object sender, EventArgs e)
        {
            double  s = 2.5;
            s = Math.Round(s);
            int d = 5;
        }
        
        private void button2_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < 8; i++)
            {
                for (int h = 0; h < 8; h++)
                {
                    quantization_table_Luminances[i, h] = Math.Floor((quantization_table_Luminances[i, h] * scale_factor + 50) / 100.0);
                    if (quantization_table_Luminances[i, h] <= 0)
                        quantization_table_Luminances[i, h] = 1;
                    if (quantization_table_Luminances[i, h] > 255)
                        quantization_table_Luminances[i, h] = 255;
                }
            }   

            //for (int i = 0; i < 8; i++)
            //{
            //    for (int h = 0; h < 8; h++)
            //    {
            //        quantization_table_Luminances[i, h] = Math.Floor(quantization_table_Luminances[i, h] * 0.2);
            //        //if (quantization_table_Luminances[i, h] <= 0)
            //        //    quantization_table_Luminances[i, h] = 1;
            //        //if (quantization_table_Luminances[i, h] > 255)
            //            //quantization_table_Luminances[i, h] = 255;
            //    }
            //}   

            OpenFileDialog dlg = new OpenFileDialog();

            //dlg.Filter = "raw files (*.raw)|*.raw|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                raw_orjinal_bytes = GetRawImage(dlg.FileName);

            }
            int width = 800, height = 600;
            double[,] R = new double[height, width];
            double[,] G = new double[height, width];
            double[,] B = new double[height, width];


            image_blocks = (width / 8) * (height / 8);

            int sayac = 0;
            for (int i = 0; i < height; i++)//600
                for (int j = 0; j < width; j++)//800
                {

                    R[i, j] = raw_orjinal_bytes[sayac];
                    ++sayac;
                    //Y[i, j] = 0.257 * renk.R + 0.504 * renk.G + 0.098 * renk.B + 16;
                }
            G = R;
            B = R;

            Bitmap resim = new Bitmap(800, 600);
            for (int i = 0; i < height; i++)//600
                for (int j = 0; j < width; j++)//800
                {
                    double red = R[i, j];
                    double green = G[i, j];
                    double blue = B[i, j];
                    Color renk = Color.FromArgb(Convert.ToInt32(red), Convert.ToInt32(green), Convert.ToInt32(blue));
                    resim.SetPixel(j, i, renk);
                }

            pictureBox1.Image = resim;

            //resim.Save("lena_800_600.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            double[,] Luminances = new double[height, width];
            double[,] Cb = new double[height, width];
            double[,] Cr = new double[height, width];
            //İmgenin her pixelinin Y(luminances) , Cb, Cr değerleri hesaplanıyor.
            for (int i = 0; i < height; i++)//600
                for (int j = 0; j < width; j++)//800
                {
                    Color color = resim.GetPixel(j, i);
                    Luminances[i, j] = 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
                    Cb[i, j] = 128 - (0.1687 * color.R) - (0.3312 * color.G) + (0.5 * color.B);
                    Cr[i, j] = 128 + (0.5 * color.R) - (0.4186 * color.G) - (0.0813 * color.B);

                }

            //---------Set and Write Markers-------
            set_and_write_SOI_Marker();
            set_and_write_APP0_Marker();
            set_and_write_DQT_Marker();
            set_and_write_SOF0_Marker();
            set_and_write_DHT_Marker();
            set_and_write_SOS_Marker();

            //--------Set and Write Data----------
            ABC(resim, Luminances);
            set_and_write_EOI_Marker();
            JPEG_File.Close();
            test.Close();
            MessageBox.Show("---TAMAM---");
            
            

        }

        private void write_data_byte(string[,] hhh, int sayac_blok)//7,2 boyutunda.. her blokta bu boyut değişebilir...
        {
            string toplam_dizi = "";
            int ss = hhh.Length;//14
            int sayac = ss / 2;//7

            for (int i = 0; i < sayac; i++)
            {
                toplam_dizi += hhh[i, 0] + hhh[i, 1];//10100100100100110....(54 elemanlı)
            }

            if(artan_bit_sayisi == 0)
            {
                int size = toplam_dizi.Length;//54
                artan_bit_sayisi = size % 8;//6

                int sayi_tut = 0;
                for (int i = 0; i < size - artan_bit_sayisi; i++) //48
                {
                    if (i == 0)
                    {
                        blok_tut[i] = toplam_dizi[i];
                        ++sayi_tut;
                    }
                    else
                    {
                        if (i % 8 != 0)
                        {
                            blok_tut[sayi_tut] = toplam_dizi[i];
                            ++sayi_tut;
                        }
                        else
                        {
                            onluk_taban_karsiligi_bul(blok_tut, sayac_blok);
                            //blok_tut = null;
                            sayi_tut = 0;
                            blok_tut[sayi_tut] = toplam_dizi[i];
                            ++sayi_tut;
                        }
                    }
                }

                if (artan_bit_sayisi == 0)
                {
                    onluk_taban_karsiligi_bul(blok_tut, sayac_blok);
                }

                else
                {
                    onluk_taban_karsiligi_bul(blok_tut, sayac_blok);
                    int x = 0;
                    for (int m = size - artan_bit_sayisi; m < size; m++)
                    {
                        blok_tut[x] = toplam_dizi[m];
                        ++x;
                    }

                    if (sayac_blok == image_blocks)
                    {
                        
                        for (int m = x; m < 8; m++)
                        {
                            blok_tut[m] = '1';
                        }

                        onluk_taban_karsiligi_bul(blok_tut, sayac_blok);
                    }

                }
                
            }
            
            else //artan bit sayisi = 6;
            {
                int sayici = 0;
                for (int i = artan_bit_sayisi; i < 8; i++)
                {
                    blok_tut[i] = toplam_dizi[sayici];
                    ++sayici;
                }
                onluk_taban_karsiligi_bul(blok_tut, sayac_blok);//eksik kalan blogu da 8 e tamamlayıp yazdır...
                int boyut = toplam_dizi.Length;//54
                artan_bit_sayisi = (boyut - sayici) % 8;// (54 - 2) % 8 = 4;

                int sayi_tut = 0;
                for (int i = sayici; i < boyut - artan_bit_sayisi ; i++) //54 - 4 = 50
                {
                    if(i == sayici)
                    {
                        blok_tut[sayi_tut] = toplam_dizi[i];
                        ++sayi_tut;
                    }
                    else
                    {
                        if ((i - sayici) % 8 != 0)
                        {
                            blok_tut[sayi_tut] = toplam_dizi[i];
                            ++sayi_tut;
                        }
                        else
                        {
                            onluk_taban_karsiligi_bul(blok_tut, sayac_blok);
                            //blok_tut = null;
                            sayi_tut = 0;
                            blok_tut[sayi_tut] = toplam_dizi[i];
                            ++sayi_tut;
                        }
                    }     

                }


                if(artan_bit_sayisi == 0)
                {
                    onluk_taban_karsiligi_bul(blok_tut, sayac_blok);
                }
                else
                {
                    onluk_taban_karsiligi_bul(blok_tut, sayac_blok);
                    int x = 0;
                    for (int m = boyut - artan_bit_sayisi; m < boyut; m++)
                    {
                        blok_tut[x] = toplam_dizi[m];
                        ++x;
                    }

                    if (sayac_blok == image_blocks)
                    {
                    
                        for (int m = x; m < 8; m++)
                        {
                            blok_tut[m] = '1';
                        }

                        onluk_taban_karsiligi_bul(blok_tut, sayac_blok);

                    }
                }
            }
            
            int ff = 66;
        }

        public void onluk_taban_karsiligi_bul(char[] blok_tut, int sayac_blok )//ardından yazdır!!!!!!!!
        {
            int toplam = 0;//172
            int sayac = 0;
            for (int j = blok_tut.Length - 1; j >= 0; --j)
            {
                double carpan = Math.Pow(2, sayac);
                if (Convert.ToInt32(blok_tut[j]) == 49)
                {
                    toplam += Convert.ToInt32(1 * carpan);
                }
                else
                {
                    toplam += Convert.ToInt32(0 * carpan);
                }
                ++sayac;
            }
            if(toplam == 19)
            {

            }
            writebyte(toplam);
            if (toplam == 255)
            {
                char[] bbb = new char[8];
                string burak = "00000000";
                for (int g = 0; g < burak.Length; g++)
                {
                    bbb[g] = burak[g];
                }
                onluk_taban_karsiligi_bul(bbb, sayac_blok);
            }
                
            
        }

        private double[,] multiply(double[,] m1, double[,] m2)
        {
            int row, col, i, j, k;
            row = col = m1.GetLength(0);
            double[,] m3 = new double[row, col];
            double sum;
            for (i = 0; i <= row - 1; i++)
            {
                for (j = 0; j <= col - 1; j++)
                {
                    Application.DoEvents();
                    sum = 0;
                    for (k = 0; k <= row - 1; k++)
                    {
                        sum = sum + m1[i, k] * m2[k, j];
                    }
                    m3[i, j] = sum;
                }
            }
            return m3;
        }

        private double[,] Transpose(double[,] m)
        {
            int i, j;
            int Width, Height;
            Width = m.GetLength(0);
            Height = m.GetLength(1);

            double[,] mt = new double[m.GetLength(0), m.GetLength(1)];

            for (i = 0; i <= Height - 1; i++)
                for (j = 0; j <= Width - 1; j++)
                {
                    mt[j, i] = m[i, j];
                }
            return (mt);
        }

        #region Marker Functions

        private void set_and_write_SOI_Marker()
        {
            //SOI
            writeword(0xFFD8);//65504
        }

        private void set_and_write_APP0_Marker()
        {
            //------------------APP0 Marker-----------------------
            APP0_Marker APP0 = new APP0_Marker();
            APP0.id = 0xFFE0;
            APP0.length = 16;
            APP0.identifier = new char[5];
            APP0.identifier[0] = 'J';
            APP0.identifier[1] = 'F';
            APP0.identifier[2] = 'I';
            APP0.identifier[3] = 'F';
            APP0.major_version = 1;
            APP0.minor_version = 1;
            APP0.Density_units = 1;
            APP0.X_Density = 96;
            APP0.Y_Density = 96;
            APP0.Thumbnail_width = 0;
            APP0.Thumbnail_height = 0;


            //APPO
            writeword(APP0.id);//65433
            writeword(APP0.length);
            writebyte(APP0.identifier[0]);//74'J'
            writebyte(APP0.identifier[1]);
            writebyte(APP0.identifier[2]);
            writebyte(APP0.identifier[3]);
            writebyte(APP0.identifier[4]);
            writebyte(APP0.major_version);
            writebyte(APP0.minor_version);
            writebyte(APP0.Density_units);
            writeword(APP0.X_Density);
            writeword(APP0.Y_Density);
            writebyte(APP0.Thumbnail_width);
            writebyte(APP0.Thumbnail_height);
        }

        private void set_and_write_DQT_Marker()
        {
            //------------------------------DQT MARKER---------------

            

            DQT_Marker_Y DQT_Y = new DQT_Marker_Y();
            DQT_Y.Y_table = new int[64];

            DQT_Y.id = 0xFFDB;
            DQT_Y.length = 67;
            DQT_Y.QTable_Y_information = 0;
            DQT_Y.Y_table = set_quant_table(std_luminance_qt, scale_factor);

            //DQT_Marker_CbCr DQT_CbCr = new DQT_Marker_CbCr();
            //DQT_CbCr.CbCR_table = new int[64];

            //DQT_CbCr.id = 0xFFDB;
            //DQT_CbCr.length = 67;
            //DQT_CbCr.QTable_CbCr_information = 1;
            //DQT_CbCr.CbCR_table = set_quant_table(std_chrominance_qt, scale_factor);

            writeword(DQT_Y.id);
            writeword(DQT_Y.length);
            writebyte(DQT_Y.QTable_Y_information);
            for (int i = 0; i < 64; i++) writebyte(DQT_Y.Y_table[i]);

            //writeword(DQT_CbCr.id);
            //writeword(DQT_CbCr.length);
            //writebyte(DQT_CbCr.QTable_CbCr_information);
            //for (int i = 0; i < 64; i++) writebyte(DQT_CbCr.CbCR_table[i]);
        }

        private void set_and_write_SOF0_Marker()
        {
            //---------------------------------SOF0 Marker-------------------------------
            SOF_Marker SOF0 = new SOF_Marker();
            SOF0.id = 0xFFC0;
            SOF0.length = 11;  //17
            SOF0.precision = 8;
            SOF0.height = 600;
            SOF0.width = 800;
            SOF0.nr_of_image_components = 1;  //3
            SOF0.Y_Id = 1;
            SOF0.Y_HV = 0x11;   //SOF0.Y_HV = 0x22;
            SOF0.Y_QuantTable = 0;
            //SOF0.Cb_Id = 2;
            //SOF0.Cb_HV = 0x11;
            //SOF0.Cb_QuantTable = 1;
            //SOF0.Cr_Id = 3;
            //SOF0.Cr_HV = 0x11;
            //SOF0.Cr_QuantTable = 1;

            writeword(SOF0.id);
            writeword(SOF0.length);
            writebyte(SOF0.precision);
            writeword(SOF0.height);
            writeword(SOF0.width);
            writebyte(SOF0.nr_of_image_components);
            writebyte(SOF0.Y_Id);
            writebyte(SOF0.Y_HV);
            writebyte(SOF0.Y_QuantTable);
            //writebyte(SOF0.Cb_Id);
            //writebyte(SOF0.Cb_HV);
            //writebyte(SOF0.Cb_QuantTable);
            //writebyte(SOF0.Cr_Id);
            //writebyte(SOF0.Cr_HV);
            //writebyte(SOF0.Cr_QuantTable);
        }

        private void set_and_write_DHT_Marker()
        {
            //-----------------DHT Marker-------------------------------------
            DHT_Marker_Y_DC DHT_Y_DC = new DHT_Marker_Y_DC();
            DHT_Marker_Y_AC DHT_Y_AC = new DHT_Marker_Y_AC();
            //DHT_Marker_CbCr_DC DHT_CbCr_DC = new DHT_Marker_CbCr_DC();
            //DHT_Marker_CbCr_AC DHT_CbCr_AC = new DHT_Marker_CbCr_AC();

            DHT_Y_DC.id = 0xFFC4;
            DHT_Y_DC.length = 31;
            DHT_Y_DC.HT_information = 0;
            DHT_Y_DC.nrcodes = new int[16];
            DHT_Y_DC.values = new int[12];
            for (int i = 0; i < 16; i++) DHT_Y_DC.nrcodes[i] = std_dc_luminance_nrcodes[i + 1];
            for (int i = 0; i <= 11; i++) DHT_Y_DC.values[i] = std_dc_luminance_values[i];

            DHT_Y_AC.id = 0xFFC4;
            DHT_Y_AC.length = 181;
            DHT_Y_AC.HT_information = 16;
            DHT_Y_AC.nrcodes = new int[16];
            DHT_Y_AC.values = new int[162];
            for (int i = 0; i < 16; i++) DHT_Y_AC.nrcodes[i] = std_ac_luminance_nrcodes[i + 1];
            for (int i = 0; i <= 161; i++) DHT_Y_AC.values[i] = std_ac_luminance_values[i];

            //DHT_CbCr_DC.id = 0xFFC4;
            //DHT_CbCr_DC.length = 31;
            //DHT_CbCr_DC.HT_information = 1;
            //DHT_CbCr_DC.nrcodes = new int[16];
            //DHT_CbCr_DC.values = new int[12];
            //for (int i = 0; i < 16; i++) DHT_CbCr_DC.nrcodes[i] = std_dc_chrominance_nrcodes[i + 1];
            //for (int i = 0; i <= 11; i++) DHT_CbCr_DC.values[i] = std_dc_chrominance_values[i];

            //DHT_CbCr_AC.id = 0xFFC4;
            //DHT_CbCr_AC.length = 181;
            //DHT_CbCr_AC.HT_information = 17;
            //DHT_CbCr_AC.nrcodes = new int[16];
            //DHT_CbCr_AC.values = new int[162];
            //for (int i = 0; i < 16; i++) DHT_CbCr_AC.nrcodes[i] = std_ac_chrominance_nrcodes[i + 1];
            //for (int i = 0; i <= 161; i++) DHT_CbCr_AC.values[i] = std_ac_chrominance_values[i];

            writeword(DHT_Y_DC.id);
            writeword(DHT_Y_DC.length);
            writebyte(DHT_Y_DC.HT_information);
            for (int i = 0; i < 16; i++) writebyte(DHT_Y_DC.nrcodes[i]);
            for (int i = 0; i <= 11; i++) writebyte(DHT_Y_DC.values[i]);

            writeword(DHT_Y_AC.id);
            writeword(DHT_Y_AC.length);
            writebyte(DHT_Y_AC.HT_information);
            for (int i = 0; i < 16; i++) writebyte(DHT_Y_AC.nrcodes[i]);
            for (int i = 0; i <= 161; i++) writebyte(DHT_Y_AC.values[i]);

            //writeword(DHT_CbCr_DC.id);
            //writeword(DHT_CbCr_DC.length);
            //writebyte(DHT_CbCr_DC.HT_information);
            //for (int i = 0; i < 16; i++) writebyte(DHT_CbCr_DC.nrcodes[i]);
            //for (int i = 0; i <= 11; i++) writebyte(DHT_CbCr_DC.values[i]);

            //writeword(DHT_CbCr_AC.id);
            //writeword(DHT_CbCr_AC.length);
            //writebyte(DHT_CbCr_AC.HT_information);
            //for (int i = 0; i < 16; i++) writebyte(DHT_CbCr_AC.nrcodes[i]);
            //for (int i = 0; i <= 161; i++) writebyte(DHT_CbCr_AC.values[i]);
        }

        private void set_and_write_SOS_Marker()
        {
            SOS_Marker SOS = new SOS_Marker();
            SOS.id = 0xFFDA;
            SOS.length = 8;  //12
            SOS.number_of_image_component = 1;  //3
            SOS.Y_Id = 1;
            SOS.Y_Huffman_Table = 0;
            //SOS.Cb_Id = 2;
            //SOS.Cb_Huffman_Table = 0x11;
            //SOS.Cr_Id = 3;
            //SOS.Cr_Huffman_Table = 0x11;
            SOS.Ss = 0;
            SOS.Se = 0x3f;
            SOS.Bf = 0;

            writeword(SOS.id);
            writeword(SOS.length);
            writebyte(SOS.number_of_image_component);
            writebyte(SOS.Y_Id);
            writebyte(SOS.Y_Huffman_Table);
            //writebyte(SOS.Cb_Id);
            //writebyte(SOS.Cb_Huffman_Table);
            //writebyte(SOS.Cr_Id);
            //writebyte(SOS.Cr_Huffman_Table);
            writebyte(SOS.Ss);
            writebyte(SOS.Se);
            writebyte(SOS.Bf);

        }

        private void set_and_write_EOI_Marker()
        {
            writeword(0xFFD9);
        }

        private int[] set_quant_table(int[] std_luminance_qt, int scalefactor)
        {
            //Q can be between 1 and 100             (90 alırsak)
            //S = (Q < 50) ? 5000//Q : 200 – 2Q      (200 - 2*90 = 20)
            int[] new_table = new int[64];
            int temp;
            for (int i = 0; i < 64; i++)
            {
                temp = (std_luminance_qt[i] * scalefactor + 50) / 100;
                //limit the values to the valid range
                if (temp <= 0) temp = 1;
                if (temp > 255) temp = 255; //limit to baseline range if requested
                new_table[zigzag[i]] = temp;
            }
            return new_table;
        }

        public void writeword(int sayi)//65504
        {
            writebyte(sayi / 256);//255
            writebyte(sayi % 256);//216
        }

        public void writebyte(int sayi)//74(-84)
        {
                byte[] dizi = new byte[1];
                dizi[0] = Convert.ToByte(sayi);
                var memo = targetEncoding.GetString(dizi);// y

                JPEG_File.Write(memo);

        }

        #endregion

        #region Data Functions

        private void ABC(Bitmap bmp, double[,] luminances)
        {
            //--------------------------AYRIK KOSİNÜS DÖNÜŞÜMÜ HESAPLANMASI---------------------
            int satir_sayac, sütün_sayac; //satir = 75, sütun = 100
            int x = 0, y = 0;
            double[,] matris_blok = new double[8, 8];//Her bir 8x8 lik matris örneğini tutar.for döngüsü içinde sürekli olarak değişir.
            //Math.Cos((Math.PI * derece) / 180);
            for (satir_sayac = 0; satir_sayac < bmp.Size.Height / 8; satir_sayac++) //75
            {
                for (sütün_sayac = 0; sütün_sayac < bmp.Size.Width / 8; sütün_sayac++) //100
                {
                    for (int a = 0; a < 8; a++)//75
                    {
                        for (int b = 0; b < 8; b++)//100
                        {
                            matris_blok[a, b] = luminances[x, y]; //8 * 8 lik matris bloklarının her biri...
                            ++y;
                        }
                        y = y - 8;
                        ++x;
                    }
                    ++matris_blok_sayac;
                    ayrik_kosinüs_hesapla(matris_blok, matris_blok_sayac);//her blok tek tek hesaplanmak için gönderiliyor...

                    x = x - 8;
                    y = y + 8;
                }
                x = x + 8;
                y = 0;
            }
        }

        private void ayrik_kosinüs_hesapla(double[,] matris_blok, int sayac_blok) //her seferinde tek bir 8x8 lik blok matris gelmektedir...
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    matris_blok[i, j] = matris_blok[i, j] - 128.0;
                }

            }
            //her 8x8 lik matris blogunun ayrık kosinüs katsayısı hesaplanıyor.Sonuc yine 8x8 lik bir matris...

            temp = multiply(donüsümMatrisi, matris_blok);
            double[,]  ayrik_kosinüs_katsayisi = multiply(temp, Transpose(donüsümMatrisi));
            double[,] nicemlenmis_blok = nicemleme_islemleri(ayrik_kosinüs_katsayisi, sayac_blok);
            double[] zigzag_dizisi = Zigzag_Tarama(nicemlenmis_blok);
            string[,] Huffman_kodlanmis_dizi = rle_kodlama(zigzag_dizisi);//zigzag_dizisi !!!!!!!!!!!!!

            yaz(Huffman_kodlanmis_dizi, sayac_blok);
            write_data_byte(Huffman_kodlanmis_dizi, sayac_blok);
        }

        private void yaz(string[,] Huffman_kodlanmis_dizi, int sayac_blok)
        {

            test.Write("\r\n" + sayac_blok.ToString() + " . Blok\r\n");

            for (int i = 0; i < burak; i++)
            {
                test.Write(Huffman_kodlanmis_dizi[i, 0]);
                test.Write("  ");
                test.Write(Huffman_kodlanmis_dizi[i, 1]);
                test.Write("\r\n");
            }
            
        }

        private string[,] rle_kodlama(double[] zigzag_dizisi)
        {
            int sayac = 0, sakla = 0;
            for (int i = 0; i < zigzag_dizisi.Length; i++)//Zigzag dizisinde 0 olmayan eleman sayısını hesaplama...
            {
                if (zigzag_dizisi[i] != 0)
                {
                    ++sayac;
                }
                else
                    continue;
            }
            if(zigzag_dizisi[0] == 0)
            {
                ++sayac;
            }
            sifirSayilari_sifirsizHali = new double[sayac + 1, 2];
            int k = 1, j;
            sifirSayilari_sifirsizHali[0, 0] = -1;//DC değeri olduğunu belirtmek için
            sifirSayilari_sifirsizHali[0, 1] = zigzag_dizisi[0];
            sifirSayilari_sifirsizHali[sayac, 0] = 0;
            sifirSayilari_sifirsizHali[sayac, 1] = 0;
            for (int i = 1; i < zigzag_dizisi.Length; ++i)//15
            {
                if (zigzag_dizisi[i] != 0)
                {
                    if (zigzag_dizisi[i - 1] == 0)
                    {
                        j = i;//j=16
                        sakla = i;
                        if (sakla == 1)
                        {
                            sifirSayilari_sifirsizHali[k, 0] = 0;
                            sifirSayilari_sifirsizHali[k, 1] = zigzag_dizisi[i];
                            ++k;
                            continue;
                        }
                            
                        while (zigzag_dizisi[j - 1] == 0)
                        {
                            sifirSayilari_sifirsizHali[k, 1] = zigzag_dizisi[i];
                            sifirSayilari_sifirsizHali[k, 0] += 1;
                            --j;
                            if (j == 1)
                                break;
                        }
                        ++k;//2
                    }
                    else
                    {
                        sifirSayilari_sifirsizHali[k, 0] = 0;
                        sifirSayilari_sifirsizHali[k, 1] = zigzag_dizisi[i];
                        ++k;
                    }
                }
                else
                {
                    continue;
                }
            }

            //2. değişken-->>zigzag dizisi elemanları hangi kategoride olduğu bilgisini bulma işlemi
            kategori = new double[sayac + 1];//21
            for (int m = 0; m < sayac + 1; m++)
            {
                kategori[m] = kategori_bul(sifirSayilari_sifirsizHali[m, 1]);
            }

            huffman_dizisi = new string[sayac + 1, 2];
            for (int h = 0; h < sayac + 1; h++)
            {
                string[,] huffman_degeri = huffman_kod_bul(sifirSayilari_sifirsizHali[h, 0], kategori[h], sifirSayilari_sifirsizHali[h, 1], zigzag_dizisi);//0,7,80
                huffman_dizisi[h, 0] = huffman_degeri[0, 0];//kategori karşılıgı huffman kodu..
                huffman_dizisi[h, 1] = huffman_degeri[0, 1];//sayının kendisine karşılık gelen huffman kodu..
            }
            burak = sayac + 1;
            return huffman_dizisi;
        }

        private string[,] huffman_kod_bul(double sifir_sayisi, double kategori, double sayi, double[] zigzag_dizisi)//0,7,80
        {
            string[,] code_word = new string[1, 2];//kategori karşılığı olan huffman kodu, sayi karşılığı olan huffman kodu...

            if(sifir_sayisi >= 16)
            {
                string[,] sakla = new string[1, 2];
                code_word[0, 0] = "11111111001";
                sakla = huffman_kod_bul(sifir_sayisi - 16, kategori, sayi, zigzag_dizisi);
                code_word[0, 1] = sakla[0, 0] + sakla[0, 1];
                return code_word;
            }

            if (sifir_sayisi == -1)//DC değeri...
            {
                if (kategori == 0)
                    code_word[0, 0] = "00";
                else if (kategori == 1)
                    code_word[0, 0] = "010";
                else if (kategori == 2)
                    code_word[0, 0] = "011";
                else if (kategori == 3)
                    code_word[0, 0] = "100";
                else if (kategori == 4)
                    code_word[0, 0] = "101";
                else if (kategori == 5)
                    code_word[0, 0] = "110";
                else if (kategori == 6)
                    code_word[0, 0] = "1110";
                else if (kategori == 7)
                    code_word[0, 0] = "11110";
                else if (kategori == 8)
                    code_word[0, 0] = "111110";
                else if (kategori == 9)
                    code_word[0, 0] = "1111110";
                else if (kategori == 10)
                    code_word[0, 0] = "11111110";
                else if (kategori == 11)
                    code_word[0, 0] = "111111110";
                char[] char_dizi = ikilik_taban_karsiligi_bul(sayi);
                for (int i = 0; i < char_dizi.Length; i++)
                {
                    code_word[0, 1] += char_dizi[i];
                }
                //code_word[0, 1] = ikilik_taban_karsiligi_bul(sayi);
            }

            else
            {
                #region AC değerleri

                if (kategori == 0 && sayi == 0) //bittiğini gösterir...
                {
                    if(zigzag_dizisi[63] == 0)
                    {
                        code_word[0, 0] = "10";
                        code_word[0, 1] = "10";//ilgili blogun son elemanını belirtir.
                    }
                    else
                    {

                    }
                    
                }
                else //diğer tüm AC değerleri
                {
                    if (sifir_sayisi == 0 && kategori == 1)
                        code_word[0, 0] = "00";
                    else if (sifir_sayisi == 0 && kategori == 2)
                        code_word[0, 0] = "01";
                    else if (sifir_sayisi == 0 && kategori == 3)
                        code_word[0, 0] = "100";
                    else if (sifir_sayisi == 0 && kategori == 4)
                        code_word[0, 0] = "1011";
                    else if (sifir_sayisi == 0 && kategori == 5)
                        code_word[0, 0] = "11010";
                    else if (sifir_sayisi == 0 && kategori == 6)
                        code_word[0, 0] = "1111000";
                    else if (sifir_sayisi == 0 && kategori == 7)
                        code_word[0, 0] = "11111000";
                    else if (sifir_sayisi == 0 && kategori == 8)
                        code_word[0, 0] = "1111110110";
                    else if (sifir_sayisi == 0 && kategori == 9)
                        code_word[0, 0] = "1111111110000010";
                    else if (sifir_sayisi == 0 && kategori == 10)
                        code_word[0, 0] = "1111111110000011";

                    else if (sifir_sayisi == 1 && kategori == 1)
                        code_word[0, 0] = "1100";
                    else if (sifir_sayisi == 1 && kategori == 2)
                        code_word[0, 0] = "11011";
                    else if (sifir_sayisi == 1 && kategori == 3)
                        code_word[0, 0] = "1111001";
                    else if (sifir_sayisi == 1 && kategori == 4)
                        code_word[0, 0] = "111110110";
                    else if (sifir_sayisi == 1 && kategori == 5)
                        code_word[0, 0] = "11111110110";
                    else if (sifir_sayisi == 1 && kategori == 6)
                        code_word[0, 0] = "1111111110000100";
                    else if (sifir_sayisi == 1 && kategori == 7)
                        code_word[0, 0] = "1111111110000101";
                    else if (sifir_sayisi == 1 && kategori == 8)
                        code_word[0, 0] = "1111111110000110";
                    else if (sifir_sayisi == 1 && kategori == 9)
                        code_word[0, 0] = "1111111110000111";
                    else if (sifir_sayisi == 1 && kategori == 10)
                        code_word[0, 0] = "1111111110001000";

                    else if (sifir_sayisi == 2 && kategori == 1)
                        code_word[0, 0] = "11100";
                    else if (sifir_sayisi == 2 && kategori == 2)
                        code_word[0, 0] = "11111001";
                    else if (sifir_sayisi == 2 && kategori == 3)
                        code_word[0, 0] = "1111110111";
                    else if (sifir_sayisi == 2 && kategori == 4)
                        code_word[0, 0] = "111111110100";
                    else if (sifir_sayisi == 2 && kategori == 5)
                        code_word[0, 0] = "1111111110001001";
                    else if (sifir_sayisi == 2 && kategori == 6)
                        code_word[0, 0] = "1111111110001010";
                    else if (sifir_sayisi == 2 && kategori == 7)
                        code_word[0, 0] = "1111111110001011";
                    else if (sifir_sayisi == 2 && kategori == 8)
                        code_word[0, 0] = "1111111110001100";
                    else if (sifir_sayisi == 2 && kategori == 9)
                        code_word[0, 0] = "1111111110001101";
                    else if (sifir_sayisi == 2 && kategori == 10)
                        code_word[0, 0] = "1111111110001110";

                    else if (sifir_sayisi == 3 && kategori == 1)
                        code_word[0, 0] = "111010";
                    else if (sifir_sayisi == 3 && kategori == 2)
                        code_word[0, 0] = "111110111";
                    else if (sifir_sayisi == 3 && kategori == 3)
                        code_word[0, 0] = "111111110101";
                    else if (sifir_sayisi == 3 && kategori == 4)
                        code_word[0, 0] = "1111111110001111";
                    else if (sifir_sayisi == 3 && kategori == 5)
                        code_word[0, 0] = "1111111110010000";
                    else if (sifir_sayisi == 3 && kategori == 6)
                        code_word[0, 0] = "1111111110010001";
                    else if (sifir_sayisi == 3 && kategori == 7)
                        code_word[0, 0] = "1111111110010010";
                    else if (sifir_sayisi == 3 && kategori == 8)
                        code_word[0, 0] = "1111111110010011";
                    else if (sifir_sayisi == 3 && kategori == 9)
                        code_word[0, 0] = "1111111110010100";
                    else if (sifir_sayisi == 3 && kategori == 10)
                        code_word[0, 0] = "1111111110010101";

                    else if (sifir_sayisi == 4 && kategori == 1)
                        code_word[0, 0] = "111011";
                    else if (sifir_sayisi == 4 && kategori == 2)
                        code_word[0, 0] = "1111111000";
                    else if (sifir_sayisi == 4 && kategori == 3)
                        code_word[0, 0] = "1111111110010110";
                    else if (sifir_sayisi == 4 && kategori == 4)
                        code_word[0, 0] = "1111111110010111";
                    else if (sifir_sayisi == 4 && kategori == 5)
                        code_word[0, 0] = "1111111110011000";
                    else if (sifir_sayisi == 4 && kategori == 6)
                        code_word[0, 0] = "1111111110011001";
                    else if (sifir_sayisi == 4 && kategori == 7)
                        code_word[0, 0] = "1111111110011010";
                    else if (sifir_sayisi == 4 && kategori == 8)
                        code_word[0, 0] = "1111111110011011";
                    else if (sifir_sayisi == 4 && kategori == 9)
                        code_word[0, 0] = "1111111110011100";
                    else if (sifir_sayisi == 4 && kategori == 10)
                        code_word[0, 0] = "1111111110011101";

                    else if (sifir_sayisi == 5 && kategori == 1)
                        code_word[0, 0] = "1111010";
                    else if (sifir_sayisi == 5 && kategori == 2)
                        code_word[0, 0] = "11111110111";
                    else if (sifir_sayisi == 5 && kategori == 3)
                        code_word[0, 0] = "1111111110011110";
                    else if (sifir_sayisi == 5 && kategori == 4)
                        code_word[0, 0] = "1111111110011111";
                    else if (sifir_sayisi == 5 && kategori == 5)
                        code_word[0, 0] = "1111111110100000";
                    else if (sifir_sayisi == 5 && kategori == 6)
                        code_word[0, 0] = "1111111110100001";
                    else if (sifir_sayisi == 5 && kategori == 7)
                        code_word[0, 0] = "1111111110100010";
                    else if (sifir_sayisi == 5 && kategori == 8)
                        code_word[0, 0] = "1111111110100011";
                    else if (sifir_sayisi == 5 && kategori == 9)
                        code_word[0, 0] = "1111111110100100";
                    else if (sifir_sayisi == 5 && kategori == 10)
                        code_word[0, 0] = "1111111110100101";

                    else if (sifir_sayisi == 6 && kategori == 1)
                        code_word[0, 0] = "1111011";
                    else if (sifir_sayisi == 6 && kategori == 2)
                        code_word[0, 0] = "111111110110";
                    else if (sifir_sayisi == 6 && kategori == 3)
                        code_word[0, 0] = "1111111110100110";
                    else if (sifir_sayisi == 6 && kategori == 4)
                        code_word[0, 0] = "1111111110100111";
                    else if (sifir_sayisi == 6 && kategori == 5)
                        code_word[0, 0] = "1111111110101000";
                    else if (sifir_sayisi == 6 && kategori == 6)
                        code_word[0, 0] = "1111111110101001";
                    else if (sifir_sayisi == 6 && kategori == 7)
                        code_word[0, 0] = "1111111110101010";
                    else if (sifir_sayisi == 6 && kategori == 8)
                        code_word[0, 0] = "1111111110101011";
                    else if (sifir_sayisi == 6 && kategori == 9)
                        code_word[0, 0] = "1111111110101100";
                    else if (sifir_sayisi == 6 && kategori == 10)
                        code_word[0, 0] = "1111111110101101";

                    else if (sifir_sayisi == 7 && kategori == 1)
                        code_word[0, 0] = "11111010";
                    else if (sifir_sayisi == 7 && kategori == 2)
                        code_word[0, 0] = "111111110111";
                    else if (sifir_sayisi == 7 && kategori == 3)
                        code_word[0, 0] = "1111111110101110";
                    else if (sifir_sayisi == 7 && kategori == 4)
                        code_word[0, 0] = "1111111110101111";
                    else if (sifir_sayisi == 7 && kategori == 5)
                        code_word[0, 0] = "1111111110110000";
                    else if (sifir_sayisi == 7 && kategori == 6)
                        code_word[0, 0] = "1111111110110001";
                    else if (sifir_sayisi == 7 && kategori == 7)
                        code_word[0, 0] = "1111111110110010";
                    else if (sifir_sayisi == 7 && kategori == 8)
                        code_word[0, 0] = "1111111110110011";
                    else if (sifir_sayisi == 7 && kategori == 9)
                        code_word[0, 0] = "1111111110110100";
                    else if (sifir_sayisi == 7 && kategori == 10)
                        code_word[0, 0] = "1111111110110101";

                    else if (sifir_sayisi == 8 && kategori == 1)
                        code_word[0, 0] = "111111000";
                    else if (sifir_sayisi == 8 && kategori == 2)
                        code_word[0, 0] = "111111111000000";
                    else if (sifir_sayisi == 8 && kategori == 3)
                        code_word[0, 0] = "1111111110110110";
                    else if (sifir_sayisi == 8 && kategori == 4)
                        code_word[0, 0] = "1111111110110111";
                    else if (sifir_sayisi == 8 && kategori == 5)
                        code_word[0, 0] = "1111111110111000";
                    else if (sifir_sayisi == 8 && kategori == 6)
                        code_word[0, 0] = "1111111110111001";
                    else if (sifir_sayisi == 8 && kategori == 7)
                        code_word[0, 0] = "1111111110111010";
                    else if (sifir_sayisi == 8 && kategori == 8)
                        code_word[0, 0] = "1111111110111011";
                    else if (sifir_sayisi == 8 && kategori == 9)
                        code_word[0, 0] = "1111111110111100";
                    else if (sifir_sayisi == 8 && kategori == 10)
                        code_word[0, 0] = "1111111110111101";

                    else if (sifir_sayisi == 9 && kategori == 1)
                        code_word[0, 0] = "111111001";
                    else if (sifir_sayisi == 9 && kategori == 2)
                        code_word[0, 0] = "1111111110111110";
                    else if (sifir_sayisi == 9 && kategori == 3)
                        code_word[0, 0] = "1111111110111111";
                    else if (sifir_sayisi == 9 && kategori == 4)
                        code_word[0, 0] = "1111111111000000";
                    else if (sifir_sayisi == 9 && kategori == 5)
                        code_word[0, 0] = "1111111111000001";
                    else if (sifir_sayisi == 9 && kategori == 6)
                        code_word[0, 0] = "1111111111000010";
                    else if (sifir_sayisi == 9 && kategori == 7)
                        code_word[0, 0] = "1111111111000011";
                    else if (sifir_sayisi == 9 && kategori == 8)
                        code_word[0, 0] = "1111111111000100";
                    else if (sifir_sayisi == 9 && kategori == 9)
                        code_word[0, 0] = "1111111111000101";
                    else if (sifir_sayisi == 9 && kategori == 10)
                        code_word[0, 0] = "1111111111000110";

                    else if (sifir_sayisi == 10 && kategori == 1)
                        code_word[0, 0] = "111111010";
                    else if (sifir_sayisi == 10 && kategori == 2)
                        code_word[0, 0] = "1111111111000111";
                    else if (sifir_sayisi == 10 && kategori == 3)
                        code_word[0, 0] = "1111111111001000";
                    else if (sifir_sayisi == 10 && kategori == 4)
                        code_word[0, 0] = "1111111111001001";
                    else if (sifir_sayisi == 10 && kategori == 5)
                        code_word[0, 0] = "1111111111001010";
                    else if (sifir_sayisi == 10 && kategori == 6)
                        code_word[0, 0] = "1111111111001011";
                    else if (sifir_sayisi == 10 && kategori == 7)
                        code_word[0, 0] = "1111111111001100";
                    else if (sifir_sayisi == 10 && kategori == 8)
                        code_word[0, 0] = "1111111111001101";
                    else if (sifir_sayisi == 10 && kategori == 9)
                        code_word[0, 0] = "1111111111001110";
                    else if (sifir_sayisi == 10 && kategori == 10)
                        code_word[0, 0] = "1111111111001111";

                    else if (sifir_sayisi == 11 && kategori == 1)
                        code_word[0, 0] = "1111111001";
                    else if (sifir_sayisi == 11 && kategori == 2)
                        code_word[0, 0] = "1111111111010000";
                    else if (sifir_sayisi == 11 && kategori == 3)
                        code_word[0, 0] = "1111111111010001";
                    else if (sifir_sayisi == 11 && kategori == 4)
                        code_word[0, 0] = "1111111111010010";
                    else if (sifir_sayisi == 11 && kategori == 5)
                        code_word[0, 0] = "1111111111010011";
                    else if (sifir_sayisi == 11 && kategori == 6)
                        code_word[0, 0] = "1111111111010100";
                    else if (sifir_sayisi == 11 && kategori == 7)
                        code_word[0, 0] = "1111111111010101";
                    else if (sifir_sayisi == 11 && kategori == 8)
                        code_word[0, 0] = "1111111111010110";
                    else if (sifir_sayisi == 11 && kategori == 9)
                        code_word[0, 0] = "1111111111010111";
                    else if (sifir_sayisi == 11 && kategori == 10)
                        code_word[0, 0] = "1111111111011000";

                    else if (sifir_sayisi == 12 && kategori == 1)
                        code_word[0, 0] = "1111111010";
                    else if (sifir_sayisi == 12 && kategori == 2)
                        code_word[0, 0] = "1111111111011001";
                    else if (sifir_sayisi == 12 && kategori == 3)
                        code_word[0, 0] = "1111111111011010";
                    else if (sifir_sayisi == 12 && kategori == 4)
                        code_word[0, 0] = "1111111111011011";
                    else if (sifir_sayisi == 12 && kategori == 5)
                        code_word[0, 0] = "1111111111011100";
                    else if (sifir_sayisi == 12 && kategori == 6)
                        code_word[0, 0] = "1111111111011101";
                    else if (sifir_sayisi == 12 && kategori == 7)
                        code_word[0, 0] = "1111111111011110";
                    else if (sifir_sayisi == 12 && kategori == 8)
                        code_word[0, 0] = "1111111111011111";
                    else if (sifir_sayisi == 12 && kategori == 9)
                        code_word[0, 0] = "1111111111100000";
                    else if (sifir_sayisi == 12 && kategori == 10)
                        code_word[0, 0] = "1111111111100001";

                    else if (sifir_sayisi == 13 && kategori == 1)
                        code_word[0, 0] = "11111111000";
                    else if (sifir_sayisi == 13 && kategori == 2)
                        code_word[0, 0] = "1111111111100010";
                    else if (sifir_sayisi == 13 && kategori == 3)
                        code_word[0, 0] = "1111111111100011";
                    else if (sifir_sayisi == 13 && kategori == 4)
                        code_word[0, 0] = "1111111111100100";
                    else if (sifir_sayisi == 13 && kategori == 5)
                        code_word[0, 0] = "1111111111100101";
                    else if (sifir_sayisi == 13 && kategori == 6)
                        code_word[0, 0] = "1111111111100110";
                    else if (sifir_sayisi == 13 && kategori == 7)
                        code_word[0, 0] = "1111111111100111";
                    else if (sifir_sayisi == 13 && kategori == 8)
                        code_word[0, 0] = "1111111111101000";
                    else if (sifir_sayisi == 13 && kategori == 9)
                        code_word[0, 0] = "1111111111101001";
                    else if (sifir_sayisi == 13 && kategori == 10)
                        code_word[0, 0] = "1111111111101010";

                    else if (sifir_sayisi == 14 && kategori == 1)
                        code_word[0, 0] = "1111111111101011";
                    else if (sifir_sayisi == 14 && kategori == 2)
                        code_word[0, 0] = "1111111111101100";
                    else if (sifir_sayisi == 14 && kategori == 3)
                        code_word[0, 0] = "1111111111101101";
                    else if (sifir_sayisi == 14 && kategori == 4)
                        code_word[0, 0] = "1111111111101110";
                    else if (sifir_sayisi == 14 && kategori == 5)
                        code_word[0, 0] = "1111111111101111";
                    else if (sifir_sayisi == 14 && kategori == 6)
                        code_word[0, 0] = "1111111111110000";
                    else if (sifir_sayisi == 14 && kategori == 7)
                        code_word[0, 0] = "1111111111110001";
                    else if (sifir_sayisi == 14 && kategori == 8)
                        code_word[0, 0] = "1111111111110010";
                    else if (sifir_sayisi == 14 && kategori == 9)
                        code_word[0, 0] = "1111111111110011";
                    else if (sifir_sayisi == 14 && kategori == 10)
                        code_word[0, 0] = "1111111111110100";

                    else if (sifir_sayisi == 15 && kategori == 0)
                        code_word[0, 0] = "11111111001";
                    else if (sifir_sayisi == 15 && kategori == 1)
                        code_word[0, 0] = "1111111111110101";
                    else if (sifir_sayisi == 15 && kategori == 2)
                        code_word[0, 0] = "1111111111110110";
                    else if (sifir_sayisi == 15 && kategori == 3)
                        code_word[0, 0] = "1111111111110111";
                    else if (sifir_sayisi == 15 && kategori == 4)
                        code_word[0, 0] = "1111111111111000";
                    else if (sifir_sayisi == 15 && kategori == 5)
                        code_word[0, 0] = "1111111111111001";
                    else if (sifir_sayisi == 15 && kategori == 6)
                        code_word[0, 0] = "1111111111111010";
                    else if (sifir_sayisi == 15 && kategori == 7)
                        code_word[0, 0] = "1111111111111011";
                    else if (sifir_sayisi == 15 && kategori == 8)
                        code_word[0, 0] = "1111111111111100";
                    else if (sifir_sayisi == 15 && kategori == 9)
                        code_word[0, 0] = "1111111111111101";
                    else if (sifir_sayisi == 15 && kategori == 10)
                        code_word[0, 0] = "1111111111111110";

                    //---------------------------------
                    char[] char_dizi = ikilik_taban_karsiligi_bul(sayi);
                    for (int i = 0; i < char_dizi.Length; i++)
                    {
                        code_word[0, 1] += char_dizi[i];
                    }
                    //code_word[0, 1] = ikilik_taban_karsiligi_bul(sayi);

                }
                #endregion
            }
            return code_word;
        }

        private char [] ikilik_taban_karsiligi_bul(double deger)
        {
            int sayi, bolum, kalan;

            string sonuc = "", kalann;

            sayi = Convert.ToInt32(deger);
            //!!!- sayıların binary kodu bulunurken norm
            if (sayi < 0)//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
            {
                sayi = sayi * -1;
                while (sayi >= 2)
                {

                    bolum = sayi / 2;
                    kalan = sayi % 2;
                    kalann = kalan.ToString();
                    sonuc = kalan + sonuc;
                    sayi = bolum;
                }
                double result = Convert.ToDouble(sayi + sonuc);
                string tut = result.ToString();
                char[] degiskenler;
                degiskenler = new char[tut.Length];
                for (int m = 0; m < tut.Length; m++)
                {
                    degiskenler[m] = tut[m];
                    if(degiskenler[m] == 49)
                    {
                        degiskenler[m] = Convert.ToChar(48);
                    }
                    else
                    {
                        degiskenler[m] = Convert.ToChar(49);
                    }
                }
                
                return degiskenler;
            }
            else
            {
                while (sayi >= 2)
                {
                    bolum = sayi / 2;
                    kalan = sayi % 2;
                    kalann = kalan.ToString();
                    sonuc = kalan + sonuc;
                    sayi = bolum;
                }
                double result = Convert.ToDouble(sayi + sonuc);
                string tut = result.ToString();
                char[] degiskenler;
                degiskenler = new char[tut.Length];
                for (int m = 0; m < tut.Length; m++)
                {
                    degiskenler[m] = tut[m];
                }
                return degiskenler;
                //MessageBox.Show(sayi + sonuc);
            }
            

        }

        private double kategori_bul(double p)
        {
            double kategori = 0;
            if (p == 0)
                kategori = 0;//end of block
            if (p == -1 || p == 1)
                kategori = 1;
            if (p == -3 || p == -2 || p == 2 || p == 3)
                kategori = 2;
            if (p >= -7 && p <= -4 || p >= 4 && p <= 7)
                kategori = 3;
            if (p >= -15 && p <= -8 || p >= 8 && p <= 15)
                kategori = 4;
            if (p >= -31 && p <= -16 || p >= 16 && p <= 31)
                kategori = 5;
            if (p >= -63 && p <= -32 || p >= 32 && p <= 63)
                kategori = 6;
            if (p >= -127 && p <= -64 || p >= 64 && p <= 127)
                kategori = 7;
            if (p >= -255 && p <= -128 || p >= 128 && p <= 255)
                kategori = 8;
            if (p >= -511 && p <= -256 || p >= 256 && p <= 511)
                kategori = 9;
            if (p >= -1023 && p <= -512 || p >= 512 && p <= 1023)
                kategori = 10;
            if (p >= -2047 && p <= -1024 || p >= 1024 && p <= 2047)
                kategori = 11;
            if (p >= -4095 && p <= -2048 || p >= 2048 && p <= 4095)
                kategori = 12;
            if (p >= -8191 && p <= -4096 || p >= 4096 && p <= 8191)
                kategori = 13;
            if (p >= -16383 && p <= -8192 || p >= 8192 && p <= 16383)
                kategori = 14;
            if (p >= -32767 && p <= -16384 || p >= 16384 && p <= 32767)
                kategori = 15;

            return kategori;
        }

        private double[] Zigzag_Tarama(double[,] nicemlenmis_blok)
        {
            double tut = 0;

            double[] zigzag_dizisi = new double[64];

            zigzag_dizisi[0] = nicemlenmis_blok[0, 0];//DC değeri..
            zigzag_dizisi[1] = nicemlenmis_blok[0, 1];
            zigzag_dizisi[2] = nicemlenmis_blok[1, 0];
            zigzag_dizisi[3] = nicemlenmis_blok[2, 0];
            zigzag_dizisi[4] = nicemlenmis_blok[1, 1];
            zigzag_dizisi[5] = nicemlenmis_blok[0, 2];
            zigzag_dizisi[6] = nicemlenmis_blok[0, 3];
            zigzag_dizisi[7] = nicemlenmis_blok[1, 2];
            zigzag_dizisi[8] = nicemlenmis_blok[2, 1];
            zigzag_dizisi[9] = nicemlenmis_blok[3, 0];
            zigzag_dizisi[10] = nicemlenmis_blok[4, 0];
            zigzag_dizisi[11] = nicemlenmis_blok[3, 1];
            zigzag_dizisi[12] = nicemlenmis_blok[2, 2];
            zigzag_dizisi[13] = nicemlenmis_blok[1, 3];
            zigzag_dizisi[14] = nicemlenmis_blok[0, 4];
            zigzag_dizisi[15] = nicemlenmis_blok[0, 5];
            zigzag_dizisi[16] = nicemlenmis_blok[1, 4];
            zigzag_dizisi[17] = nicemlenmis_blok[2, 3];
            zigzag_dizisi[18] = nicemlenmis_blok[3, 2];
            zigzag_dizisi[19] = nicemlenmis_blok[4, 1];
            zigzag_dizisi[20] = nicemlenmis_blok[5, 0];
            zigzag_dizisi[21] = nicemlenmis_blok[6, 0];
            zigzag_dizisi[22] = nicemlenmis_blok[5, 1];
            zigzag_dizisi[23] = nicemlenmis_blok[4, 2];
            zigzag_dizisi[24] = nicemlenmis_blok[3, 3];
            zigzag_dizisi[25] = nicemlenmis_blok[2, 4];
            zigzag_dizisi[26] = nicemlenmis_blok[1, 5];
            zigzag_dizisi[27] = nicemlenmis_blok[0, 6];
            zigzag_dizisi[28] = nicemlenmis_blok[0, 7];
            zigzag_dizisi[29] = nicemlenmis_blok[1, 6];
            zigzag_dizisi[30] = nicemlenmis_blok[2, 5];
            zigzag_dizisi[31] = nicemlenmis_blok[3, 4];
            zigzag_dizisi[32] = nicemlenmis_blok[4, 3];
            zigzag_dizisi[33] = nicemlenmis_blok[5, 2];
            zigzag_dizisi[34] = nicemlenmis_blok[6, 1];
            zigzag_dizisi[35] = nicemlenmis_blok[7, 0];
            zigzag_dizisi[36] = nicemlenmis_blok[7, 1];
            zigzag_dizisi[37] = nicemlenmis_blok[6, 2];
            zigzag_dizisi[38] = nicemlenmis_blok[5, 3];
            zigzag_dizisi[39] = nicemlenmis_blok[4, 4];
            zigzag_dizisi[40] = nicemlenmis_blok[3, 5];
            zigzag_dizisi[41] = nicemlenmis_blok[2, 6];
            zigzag_dizisi[42] = nicemlenmis_blok[1, 7];
            zigzag_dizisi[43] = nicemlenmis_blok[2, 7];
            zigzag_dizisi[44] = nicemlenmis_blok[3, 6];
            zigzag_dizisi[45] = nicemlenmis_blok[4, 5];
            zigzag_dizisi[46] = nicemlenmis_blok[5, 4];
            zigzag_dizisi[47] = nicemlenmis_blok[6, 3];
            zigzag_dizisi[48] = nicemlenmis_blok[7, 2];
            zigzag_dizisi[49] = nicemlenmis_blok[7, 3];
            zigzag_dizisi[50] = nicemlenmis_blok[6, 4];
            zigzag_dizisi[51] = nicemlenmis_blok[5, 5];
            zigzag_dizisi[52] = nicemlenmis_blok[4, 6];
            zigzag_dizisi[53] = nicemlenmis_blok[3, 7];
            zigzag_dizisi[54] = nicemlenmis_blok[4, 7];
            zigzag_dizisi[55] = nicemlenmis_blok[5, 6];
            zigzag_dizisi[56] = nicemlenmis_blok[6, 5];
            zigzag_dizisi[57] = nicemlenmis_blok[7, 4];
            zigzag_dizisi[58] = nicemlenmis_blok[7, 5];
            zigzag_dizisi[59] = nicemlenmis_blok[6, 6];
            zigzag_dizisi[60] = nicemlenmis_blok[5, 7];
            zigzag_dizisi[61] = nicemlenmis_blok[6, 7];
            zigzag_dizisi[62] = nicemlenmis_blok[7, 6];
            zigzag_dizisi[63] = nicemlenmis_blok[7, 7];

            tut = zigzag_dizisi[0];
            zigzag_dizisi[0] -= onceki_DC_degeri;
            onceki_DC_degeri = tut;

            return zigzag_dizisi;
        }

        private double[,] nicemleme_islemleri(double[,] ayrik_kosinüs_katsayisi, int sayac_blok)
        {
            double[,] nicemlenmis_katsayilar = new double[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    double temp;
                    double bolum = ayrik_kosinüs_katsayisi[i, j] / quantization_table_Luminances[i, j];//
                    //bolum = Math.Round(bolum, 1);

                    if (Math.Round(ayrik_kosinüs_katsayisi[3,0], 2) == -64.47)
                    {

                    }

                    if (sayac_blok == 12)
                    {
                    }

                    double sayi = 0;
                    if (bolum < 0.0)
                    {
                        sayi = Math.Ceiling(bolum);
                    }
                    else
                    {
                        //++rrr;
                        sayi = Math.Floor(bolum);
                    }

                    if (bolum - sayi == -0.5 || bolum - sayi == -0.4895150130869839 || bolum - sayi == -0.49322928643609387)
                    {
                        ++ffff;
                        nicemlenmis_katsayilar[i, j] = Math.Floor(bolum);
                    }
                    else if (bolum - sayi == 0.5 || bolum - sayi == 0.49093042335023675 || bolum - sayi == 0.48237005324047288)
                    {
                        ++ffff;
                        nicemlenmis_katsayilar[i, j] = Math.Ceiling(bolum);
                    }
                    else
                        nicemlenmis_katsayilar[i, j] = Math.Round((ayrik_kosinüs_katsayisi[i, j] / quantization_table_Luminances[i, j])); //
                }
            }
            return nicemlenmis_katsayilar;

            //double[,] nicemlenmis_katsayilar = new double[8, 8];
            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        nicemlenmis_katsayilar[i, j] = Math.Round((ayrik_kosinüs_katsayisi[i, j] / quantization_table_Luminances[i, j]));
            //    }
            //}
            //return nicemlenmis_katsayilar;
        }

        public byte[] GetRawImage(string filename)
        {
            byte[] bt;
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            // Raw file don't have any "Header" 
            BinaryReader br = new BinaryReader(fs);
            bt = br.ReadBytes((int)br.BaseStream.Length);
            br.Close();
            return bt;
        }

       

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        #endregion

    }
}
