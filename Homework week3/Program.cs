using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Homework_week3
{
    class Program
    {
        static (int height, int width) fliesizeofbmp;
        static byte[] bitmapfile;
        static void Main(string[] args)
        {

            //Mixture of Essential task three and toughter two
            // The pragram will ask for a bmp file - than it will display its width and heights 
            // After it  will use those width and h to create  either a black or a multicolour image

            if (args.Length > 1)
            {
                //                 FileToLoad
                LoadImageDimentions(args[0]);
            }
            else
            {
                LoadImageDimentions();
            }


            if (args.Length > 3)
            {
                //          FileName, ColourType (black or multicolour)
                CreateImage(args[1], args[2]);
            }
            else
            {
                CreateImage();
            }

        }
        static void LoadImageDimentions(string loadimage = null)
        {
            if (loadimage == null && File.Exists(loadimage))
            {
                while (true) // robutness for wrong file type 
                {
                    Console.WriteLine("What image would you like to Load?");
                    loadimage = Console.ReadLine();
                    if (File.Exists(loadimage)) break;
                }
            }

            bitmapfile = File.ReadAllBytes(loadimage);

            int hightofbmp = (bitmapfile[19] << 8) | bitmapfile[18]; //bigger number first
            int widthofbmp = (bitmapfile[23] << 8) | bitmapfile[22];
            float sizeofbmp = (bitmapfile[5] << 24) + (bitmapfile[4] << 16) + (bitmapfile[3] << 8) + (bitmapfile[2]); // shifting  [00000000] [00000000] [00000000] [00000000] 
            fliesizeofbmp = (hightofbmp, widthofbmp);// setting the values 
            //displays contents
            Console.WriteLine("The widith of the BMP image is " + widthofbmp);
            Console.WriteLine("The Hight of the BMP image is " + hightofbmp);
            Console.WriteLine("The size of the file " + sizeofbmp / 1000000 + " mb"); // divides by a million becuase thats how many bytes in a mb
        }
        static void CreateImage(string filename = null, string colourType = null)
        {
            if (filename == null)
            {
                Console.WriteLine("What would you like to call your file");
                filename = Console.ReadLine();
            }
            int height = fliesizeofbmp.height;
            int width = fliesizeofbmp.width;
            int length = 3 * (height * width); // calculate how many bytes are in a file. equation = 3(r,g,b)(h x w)

            while (true)
            {
                Console.WriteLine("What type of image would you like");
                string input = Console.ReadLine();

                if (DoIfValid(input))
                {
                    break;
                }
            }
            bool DoIfValid(string input)
            {
                if (colourType.ToLower() == "blackimage")
                {
                    Blackimage(filename);
                    return true;
                }
                else if (colourType.ToLower() == "multicolour")
                {
                    Multicolor(filename);
                    return true;
                }
                else
                {
                    Console.WriteLine("Please pick from the following optins'blackimage' or 'multicolour'");
                    return false;
                }
            }
        }
        static void Blackimage(string filename)
        {
            // loop for black / white colour.
            int length = 3 * (fliesizeofbmp.height * fliesizeofbmp.width); //reusing the stored data to get how many bytes in the image.
            for (int i = 139; i < length; i += 3) // calculated that the colours starts at 139 in a bitmap image
            {
                Random rand = new Random();
                byte randomcolour = (byte)rand.Next(0, 255);

                // replacing 3 bytes with my random colour.
                bitmapfile[i] = randomcolour; //red, setting the first value of the random colour in red
                bitmapfile[i + 1] = randomcolour;// green
                bitmapfile[i + 2] = randomcolour; // blue
            }
            File.WriteAllBytes(filename + ".bmp", bitmapfile); // Saves to a file in binary format. 
        }
        static void Multicolor(string filename)
        {
            int length = 3 * (fliesizeofbmp.height * fliesizeofbmp.width);
            for (int i = 139; i < length; i++) // Creates a random colour which sets it 
            {
                Random rand = new Random();
                byte randomcolour = (byte)rand.Next(0, 255);

                bitmapfile[i] = randomcolour;
            }
            File.WriteAllBytes(filename + ".bmp", bitmapfile);
        }
    }
}

