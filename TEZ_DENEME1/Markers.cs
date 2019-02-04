using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEZ_DENEME1
{
    class APP0_Marker
    {
        public int id{ get; set; } //2 bytes
        
        public int length { get; set; } //2 bytes

        public char [] identifier { get; set; }

        public int major_version { get; set; }

        public int minor_version { get; set; }

        public int Density_units { get; set; }

        public int X_Density { get; set; }

        public int Y_Density { get; set; }

        public  int Thumbnail_width { get; set; }

        public int Thumbnail_height { get; set; }
    } 
    
    class DQT_Marker_Y
    {

        public int id { get; set; }
        public int length { get; set; }
        public int QTable_Y_information { get; set; }
        public int [] Y_table { get; set; }
        
        
        //WORD marker;  // = 0xFFDB
        // WORD length;  // = 132
        // BYTE QTYinfo;// = 0:  bit 0..3: number of QT = 0 (table for Y)
        //          //       bit 4..7: precision of QT, 0 = 8 bit
        // BYTE Ytable[64];
        // BYTE QTCbinfo; // = 1 (quantization table for Cb,Cr}
        // BYTE Cbtable[64];
    }

    class DQT_Marker_CbCr
    {
        public int id { get; set; }
        public int length { get; set; }
        public int QTable_CbCr_information { get; set; }
        public int [] CbCR_table { get; set; }
    }

    class SOF_Marker
    {
        public int id { get; set; }
        public int length { get; set; }
        public int precision { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int nr_of_image_components  { get; set; }
        public int Y_Id { get; set; }
        public int Y_HV { get; set; }
        public int Y_QuantTable { get; set; }
        public int Cb_Id { get; set; }
        public int Cb_HV { get; set; }
        public int Cb_QuantTable { get; set; }
        public int Cr_Id { get; set; }
        public int Cr_HV { get; set; }
        public int Cr_QuantTable { get; set; }

    }

    class DHT_Marker_Y_DC
    {
        public int id { get; set; }
        public int length { get; set; }
        public int HT_information { get; set; }
        public int [] nrcodes { get; set; }
        public int[] values { get; set; }

    }

    class DHT_Marker_Y_AC
    {
        public int id { get; set; }
        public int length { get; set; }
        public int HT_information { get; set; }
        public int[] nrcodes { get; set; }
        public int[] values { get; set; }
    }

    class DHT_Marker_CbCr_DC
    {
        public int id { get; set; }
        public int length { get; set; }
        public int HT_information { get; set; }
        public int[] nrcodes { get; set; }
        public int[] values { get; set; }
    }

    class DHT_Marker_CbCr_AC
    {
        public int id { get; set; }
        public int length { get; set; }
        public int HT_information { get; set; }
        public int[] nrcodes { get; set; }
        public int[] values { get; set; }
    }

    class SOS_Marker
    {
        public int id { get; set; }
        public int length { get; set; }
        public int number_of_image_component { get; set; }
        public int Y_Id { get; set; }
        public int Y_Huffman_Table { get; set; }
        public int Cb_Id { get; set; }
        public int Cb_Huffman_Table { get; set; }
        public int Cr_Id { get; set; }
        public int Cr_Huffman_Table { get; set; }
        public int Ss { get; set; }
        public int Se { get; set; }
        public int Bf { get; set; }

    }

}
