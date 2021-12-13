namespace batzen
{
	using ZenLib;

	public class SrHeader
	{
		//public int Length {get;set;}

		public int SegmentLeft {get;set;}

		public int LastEntry {get;set;}

		public string[] SegmentList {get;set;}
	}
}