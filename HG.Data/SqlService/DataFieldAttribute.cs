using System;

namespace HG.Data.SqlService
{
	[AttributeUsageAttribute (AttributeTargets.Property)]
	public class DataFieldAttribute : Attribute
	{
		public DataFieldAttribute(string DataFieldName)
		{
			this.DataFieldName = DataFieldName;
		}

		public string DataFieldName { get; set; }

	} 
}
