// MIT License
// 
// Copyright(c) 2017 Luciano (Xeeynamo) Ciccariello
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// Part of this software belongs to XeEngine toolset and United Lines Studio
// and it is currently used to create commercial games by Luciano Ciccariello.
// Please do not redistribuite this code under your own name, stole it or use
// it artfully, but instead support it and its author. Thank you.

using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;

namespace RSDK.AnimationEditor.Content.Services
{
    public class Color
    {
        public byte r, g, b, a;
    }
    public static class ImageService
    {
        public static BitmapSource Open(string fileName)
        {

            if (File.Exists(fileName))
            {

                using (var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    //Temp :/
                    BitmapImage Image = new BitmapImage();
                    Image.CreateOptions = BitmapCreateOptions.None;
                    Image.UriSource = new Uri(fStream.Name);
                    return Image;
                }
            }
            else
            {
                //Debug.WriteLine($"Texture file {fileName} not found.");
            }
            return null;
        }

        public static void Save(this BitmapSource bitmap, string fileName)
        {
            using (var fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                //var encoder = new BitmapEncoder
                //{
                //     Interlace = PngInterlaceOption.Off
                //};
                //encoder.Frames.Add(BitmapFrame.Create(bitmap));
                //encoder.Save(fStream);
            }
        }
    }
}
