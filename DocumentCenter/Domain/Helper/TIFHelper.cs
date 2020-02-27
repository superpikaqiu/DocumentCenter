using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Media.Imaging;

namespace DocumentCenter.Domain.Helper
{
    public class TIFHelper
    {
        public static void TIFToGif(string imagePath,string savePath)
        {
            FileStream imageStream = new FileStream(imagePath, FileMode.Open);
            TiffBitmapDecoder decoder = new TiffBitmapDecoder(imageStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];//此处只取tiff中的第一帧，可以根据情况取多帧，从Frames.Count中取
            GifBitmapEncoder encoder = new GifBitmapEncoder();
            foreach(var frame in decoder.Frames)
            {
                encoder.Frames.Add(frame);
            }

            FileStream gifStream = new FileStream(savePath, FileMode.Create);
            encoder.Save(gifStream);
            gifStream.Close();
            imageStream.Close();
        }
    }
}