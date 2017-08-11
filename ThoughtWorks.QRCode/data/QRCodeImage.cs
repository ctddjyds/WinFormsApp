using System;

namespace ThoughtWorks.QRCode
{
	public interface QRCodeImage
	{
        int Width
        {
            get;
        }
        int Height
        {
            get;
        }
        int getPixel(int x, int y);
	}
}