using System;
using System.IO;

namespace ThoughtWorks.QRCode
{	
	public class QRCodeDataBlockReader
	{
        internal virtual int NextMode
		{
			get
			{
				if ((blockPointer > blocks.Length - numErrorCorrectionCode - 2))
					return 0;
				else
					return getNextBits(4);
			}		
		}
        public virtual sbyte[] DataByte
		{
			get
			{
				canvas.println("Reading data blocks.");
				MemoryStream output = new MemoryStream();			
				try
				{
					do 
					{
						int mode = NextMode;
						if (mode == 0)
						{
							if (output.Length > 0)
								break;
							else
								throw new InvalidDataBlockException("Empty data block");
						}
						if (mode != MODE_NUMBER && mode != MODE_ROMAN_AND_NUMBER && mode != MODE_8BIT_BYTE && mode != MODE_KANJI)
						{		
							throw new InvalidDataBlockException("Invalid mode: " + mode + " in (block:" + blockPointer + " bit:" + bitPointer + ")");
						}
						dataLength = getDataLength(mode);
						if (dataLength < 1)
							throw new InvalidDataBlockException("Invalid data length: " + dataLength);
						switch (mode)
						{						
							case MODE_NUMBER: 
								sbyte[] temp_sbyteArray;
								temp_sbyteArray = SystemUtils.ToSByteArray(SystemUtils.ToByteArray(getFigureString(dataLength)));
								output.Write(SystemUtils.ToByteArray(temp_sbyteArray), 0, temp_sbyteArray.Length);
								break;							
							case MODE_ROMAN_AND_NUMBER: 
								sbyte[] temp_sbyteArray2;
								temp_sbyteArray2 = SystemUtils.ToSByteArray(SystemUtils.ToByteArray(getRomanAndFigureString(dataLength)));
								output.Write(SystemUtils.ToByteArray(temp_sbyteArray2), 0, temp_sbyteArray2.Length);
								break;							
							case MODE_8BIT_BYTE: 
								sbyte[] temp_sbyteArray3;
								temp_sbyteArray3 = get8bitByteArray(dataLength);
								output.Write(SystemUtils.ToByteArray(temp_sbyteArray3), 0, temp_sbyteArray3.Length);
								break;							
							case MODE_KANJI: 
								sbyte[] temp_sbyteArray4;
								temp_sbyteArray4 = SystemUtils.ToSByteArray(SystemUtils.ToByteArray(getKanjiString(dataLength)));
								output.Write(SystemUtils.ToByteArray(temp_sbyteArray4), 0, temp_sbyteArray4.Length);
								break;
							}
					}
					while (true);
				}
				catch (System.IndexOutOfRangeException e)
				{
					SystemUtils.WriteStackTrace(e, Console.Error);
					throw new InvalidDataBlockException("Data Block Error in (block:" + blockPointer + " bit:" + bitPointer + ")");
				}
				catch (System.IO.IOException e)
				{
					throw new InvalidDataBlockException(e.Message);
				}
				return SystemUtils.ToSByteArray(output.ToArray());
			}
			
		}
        public virtual string DataString
		{
			get
			{
				canvas.println("Reading data blocks...");
				string dataString = "";
                while (true)
                {
					int mode = NextMode;
					canvas.println("mode: " + mode);
                    switch(mode)
                    {
                        case 0:
                            Console.Out.WriteLine("");
                        return dataString;
                    }
                    dataLength = getDataLength(mode);
					canvas.println(System.Convert.ToString(blocks[blockPointer]));
					System.Console.Out.WriteLine("length: " + dataLength);
					switch (mode)
					{					
						case MODE_NUMBER: 
							dataString += getFigureString(dataLength);
							break;						
						case MODE_ROMAN_AND_NUMBER: 
							dataString += getRomanAndFigureString(dataLength);
							break;						
						case MODE_8BIT_BYTE: 
							dataString += get8bitByteString(dataLength);
							break;						
						case MODE_KANJI: 
							dataString += getKanjiString(dataLength);
							break;
						}
				}
			}		
		}
		internal int[] blocks;
		internal int dataLengthMode;
		internal int blockPointer = 0;
        internal int bitPointer = 7;
        internal int dataLength = 0;
        internal int numErrorCorrectionCode;
		internal DebugCanvas canvas;

        private const  int MODE_NUMBER = 1;
        private const int MODE_ROMAN_AND_NUMBER = 2;
        private const int MODE_8BIT_BYTE = 4;
        private const int MODE_KANJI = 8;
	    int[][] sizeOfDataLengthInfo = new int[][] { new int[] { 10, 9, 8, 8 }, new int[] { 12, 11, 16, 10 }, new int[] { 14, 13, 16, 12 } };
		
		public QRCodeDataBlockReader(int[] blocks, int version, int numErrorCorrectionCode)
		{									
			this.blocks = blocks;
			this.numErrorCorrectionCode = numErrorCorrectionCode;
			if (version <= 9)
				dataLengthMode = 0;
			else if (version >= 10 && version <= 26)
				dataLengthMode = 1;
			else if (version >= 27 && version <= 40)
				dataLengthMode = 2;
			canvas = QRCodeDecoder.Canvas;
		}

        internal virtual int getDataLength(int modeIndicator)
        {
            int index = 0;
            while (true)
            {
                if ((modeIndicator >> index) == 1)
                    break;
                index++;
            }
            return getNextBits(sizeOfDataLengthInfo[dataLengthMode][index]);
        }

        internal virtual string getFigureString(int dataLength)
        {
            int length = dataLength;
            int intData = 0;
            string strData = "";
            do
            {
                if (length >= 3)
                {
                    intData = getNextBits(10);
                    if (intData < 100)
                        strData += "0";
                    if (intData < 10)
                        strData += "0";
                    length -= 3;
                }
                else if (length == 2)
                {
                    intData = getNextBits(7);
                    if (intData < 10)
                        strData += "0";
                    length -= 2;
                }
                else if (length == 1)
                {
                    intData = getNextBits(4);
                    length -= 1;
                }
                strData += System.Convert.ToString(intData);
            }
            while (length > 0);
            return strData;
        }

        internal virtual int getNextBits(int numBits)
		{			
			int bits = 0;
			if (numBits < bitPointer + 1)
			{
				int mask = 0;
				for (int i = 0; i < numBits; i++)
				{
					mask += (1 << i);
				}
				mask <<= (bitPointer - numBits + 1);				
				bits = (blocks[blockPointer] & mask) >> (bitPointer - numBits + 1);
				bitPointer -= numBits;
				return bits;
			}
			else if (numBits < ((this.bitPointer + 1) + 8))
			{
				int mask1 = 0;
				for (int i = 0; i < bitPointer + 1; i++)
				{
					mask1 += (1 << i);
				}
				bits = (blocks[blockPointer] & mask1) << (numBits - (bitPointer + 1));
                blockPointer++;
				bits += ((blocks[blockPointer]) >> (8 - (numBits - (bitPointer + 1))));
				
				bitPointer = bitPointer - numBits % 8;
				if (bitPointer < 0)
				{
					bitPointer = 8 + bitPointer;
				}
				return bits;
			}
			else if (numBits < bitPointer + 1 + 16)
			{
				int mask1 = 0; // mask of first block
				int mask3 = 0; // mask of 3rd block
				for (int i = 0; i < bitPointer + 1; i++)
				{
					mask1 += (1 << i);
				}
				int bitsFirstBlock = (blocks[blockPointer] & mask1) << (numBits - (bitPointer + 1));
				blockPointer++;
				
				int bitsSecondBlock = blocks[blockPointer] << (numBits - (bitPointer + 1 + 8));
				blockPointer++;
				
				for (int i = 0; i < numBits - (bitPointer + 1 + 8); i++)
				{
					mask3 += (1 << i);
				}
				mask3 <<= 8 - (numBits - (bitPointer + 1 + 8));
				int bitsThirdBlock = (blocks[blockPointer] & mask3) >> (8 - (numBits - (bitPointer + 1 + 8)));
				
				bits = bitsFirstBlock + bitsSecondBlock + bitsThirdBlock;
				bitPointer = bitPointer - (numBits - 8) % 8;
				if (bitPointer < 0)
				{
					bitPointer = 8 + bitPointer;
				}
				return bits;
			}
			else
			{
				System.Console.Out.WriteLine("ERROR!");
				return 0;
			}
		}
		
		internal virtual int guessMode(int mode)
		{
			switch (mode)
			{				
				case 3: 
					return MODE_NUMBER;				
				case 5: 
					return MODE_8BIT_BYTE;				
				case 6: 
					return MODE_8BIT_BYTE;				
				case 7: 
					return MODE_8BIT_BYTE;				
				case 9: 
					return MODE_KANJI;				
				case 10: 
					return MODE_KANJI;				
				case 11: 
					return MODE_KANJI;				
				case 12: 
					return MODE_8BIT_BYTE;				
				case 13: 
					return MODE_8BIT_BYTE;				
				case 14: 
					return MODE_8BIT_BYTE;				
				case 15: 
					return MODE_8BIT_BYTE;				
				default: 
					return MODE_KANJI;			
			}
		}
												
		internal virtual string getRomanAndFigureString(int dataLength)
		{
			int length = dataLength;
			int intData = 0;
			string strData = "";
			char[] tableRomanAndFigure = new char[]{
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%', '*',
                '+', '-', '.', '/', ':'};
			do 
			{
				if (length > 1)
				{
					intData = getNextBits(11);
					int firstLetter = intData / 45;
					int secondLetter = intData % 45;
					strData += Convert.ToString(tableRomanAndFigure[firstLetter]);
					strData += Convert.ToString(tableRomanAndFigure[secondLetter]);
					length -= 2;
				}
				else if (length == 1)
				{
					intData = getNextBits(6);
					strData += Convert.ToString(tableRomanAndFigure[intData]);
					length -= 1;
				}
			}
			while (length > 0);			
			return strData;
		}
		
		public virtual sbyte[] get8bitByteArray(int dataLength)
		{
			int length = dataLength;
			int intData = 0;
			MemoryStream output = new MemoryStream();			
			do 
			{
                canvas.println("Length: " + length);
				intData = getNextBits(8);
				output.WriteByte((byte) intData);
				length--;
			}
			while (length > 0);
			return SystemUtils.ToSByteArray(output.ToArray());
		}
		
		internal virtual string get8bitByteString(int dataLength)
		{
			int length = dataLength;
			int intData = 0;
			string strData = "";
			do 
			{
				intData = getNextBits(8);
				strData += (char) intData;
				length--;
			}
			while (length > 0);
			return strData;
		}
		
		internal virtual string getKanjiString(int dataLength)
		{
			int length = dataLength;
			int intData = 0;
			string unicodeString = "";
			do 
			{
				intData = getNextBits(13);
				int lowerByte = intData % 0xC0;
				int higherByte = intData / 0xC0;			
				int tempWord = (higherByte << 8) + lowerByte;
				int shiftjisWord = 0;
				if (tempWord + 0x8140 <= 0x9FFC)
				{
					// between 8140 - 9FFC on Shift_JIS character set
					shiftjisWord = tempWord + 0x8140;
				}
				else
				{
					// between E040 - EBBF on Shift_JIS character set
					shiftjisWord = tempWord + 0xC140;
				}
				
				sbyte[] tempByte = new sbyte[2];
				tempByte[0] = (sbyte) (shiftjisWord >> 8);
				tempByte[1] = (sbyte) (shiftjisWord & 0xFF);
				unicodeString += new String(SystemUtils.ToCharArray(SystemUtils.ToByteArray(tempByte)));
				length--;
			}
			while (length > 0);						
			return unicodeString;
		}
	}
}