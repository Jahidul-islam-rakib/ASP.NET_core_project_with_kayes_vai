namespace CrudKayes.Models
{
	public class BaseModel
	{
		// timestamp ta prottek ta table er sathe add korbo. tai base model akare nisi 
		// sob table etake inherit korbe
		[Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
