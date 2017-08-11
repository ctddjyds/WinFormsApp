using System;
namespace ThoughtWorks.QRCode
{
	[Serializable]
	public class AlignmentPatternNotFoundException:System.ArgumentException
	{
        internal string message = null;

		public override string Message
		{
			get
			{
				return message;
			}			
		}		
		public AlignmentPatternNotFoundException(string message)
		{
			this.message = message;
		}
	}
}