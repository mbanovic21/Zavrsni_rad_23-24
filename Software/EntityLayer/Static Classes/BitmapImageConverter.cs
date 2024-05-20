using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace EntityLayer
{
    public static class BitmapImageConverter
    {
        //Convert byte picture to BitmapImage
        public static BitmapImage ConvertByteArrayToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memoryStream = new MemoryStream(imageData))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }

        // Convert BitmapImage to byte picture
        public static byte[] ConvertBitmapImageToByteArray(string imagePath)
        {
            byte[] imageData;
            using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                imageData = new byte[fileStream.Length];
                fileStream.Read(imageData, 0, (int)fileStream.Length);
            }
            return imageData;
        }
    }
}
