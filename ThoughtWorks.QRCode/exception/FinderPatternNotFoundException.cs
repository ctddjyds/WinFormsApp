using System;
namespace ThoughtWorks.QRCode
{
	[Serializable]
	public class FinderPatternNotFoundException:System.Exception
	{
        internal string message = null;
		public override string Message
		{
			get
			{
				return message;
			}
			
		}		
		public FinderPatternNotFoundException(string message)
		{
			this.message = message;
		}
	}
}