
namespace CrudKayes.Models
{
	public class Person
	{
		//public Person() 
		//{
		//	Name = ""; //for default value , in this case all property er default value likha lagbe
		// warnings uthaite
		//}


		[Key]
		public int Id { get; set; }


		// ei gulo server --> "side validation "
		[Display(Name = "Person Name")]
		[StringLength(maximumLength: 20, ErrorMessage = "{0} length between {2} and {1}", MinimumLength = 3)]
		[Required(ErrorMessage ="{0} is required")]

		public string Name { get; set; }
		//public string Name { get; set; } = "default";
		//warnings uthate ? deya lagbe

		public string Gender { get; set; }
		public string Height { get; set; }
		public string Weight { get; set; }
		public DateOnly DOB { get; set; }
		public string HairColor { get; set; }
		public string PresentAddress { get; set; }
		public string PermanentAddress { get; set; }

		
		[NotMapped]
		public string FullAddress { get; set; }
		
	}
}
