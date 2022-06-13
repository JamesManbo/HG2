﻿namespace HG.Entities
{
	public class BaseModel
	{
		public Guid Id { get; set; }
		public int? Status { get; set; }
		public DateTime? CreatedDateUtc { get; set; }
		public Guid? CreatedUid { get; set; }
		public DateTime? UpdatedDateUtc { get; set; }
		public Guid? UpdatedUid { get; set; }
		public int? Deleted { get; set; }
		public Guid? DeletedBy { get; set; }
	}
}