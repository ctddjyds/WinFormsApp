using System;
namespace ThoughtWorks.QRCode
{
	[Serializable]
	public class InvalidDataBlockException:System.ArgumentException
	{
        internal string message = null;

		public override string Message
		{
			get
			{
				return message;
			}		
		}
		
		public InvalidDataBlockException(string message)
		{
			this.message = message;
		}
	}
}