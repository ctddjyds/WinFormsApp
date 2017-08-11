using System;
namespace ThoughtWorks.QRCode
{
	
	// Possible Exceptions
	//DecodingFailedException
	//- SymbolNotFoundException
	//  - FinderPatternNotFoundException
	//  - AlignmentPatternNotFoundException
	//- SymbolDataErrorException
	//  - IllegalDataBlockException
	//	- InvalidVersionInfoException
	//- UnsupportedVersionException
	
	[Serializable]
	public class DecodingFailedException:ArgumentException
	{
        internal string message = null;

		public override string Message
		{
			get
			{
				return message;
			}			
		}
		
		public DecodingFailedException(string message)
		{
			this.message = message;
		}
	}
}