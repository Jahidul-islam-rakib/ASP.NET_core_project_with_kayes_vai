namespace CrudKayes.Controllers
{
	public class PersonController : BaseController
	{
		//private static MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions()); //cls-1
		//string CacheName = "personCache";
		private readonly CrudDbContext _context;

		/*
		 2nd class kayes vai
		1.Error Summary
		2.only one person can save data simultaneously
		3. Guid use in primary key
		4. hosting
		 */
        public PersonController(CrudDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
			//if(memoryCache.TryGetValue(CacheName, out var result))
			//{
			//	return View(result);   //cls-1
			//}
			//var DataSet = _context.Person;// database e hit korbe na. onlu query ta generate korbe. database e execute hobe na
			var DataSet = _context.Person.ToList(); // ToList command dile sathe sathe database e chole jabe.hit korbe database e
			//var DataSet = _context.Person.FirstAsync(); // Async ta slow process e kaj kore. data volume beshi hole FirstAsync use korbo sathe await use kora lagbe.
			foreach(var item in DataSet)
			{
				item.FullAddress = item.PresentAddress + "###" + item.PermanentAddress;
			}

			//memoryCache.Set(CacheName, DataSet, TimeSpan.FromSeconds(10)); //cls-1
			// 10 seconds er majhe request asle if block thekei result return korbe r 10seconds over hoye gele abar database theke data niye asbe 
			// 10 seconds er vitor request pathale cache theke data dekhabe


			return View(DataSet);
		}

		[HttpGet]
		public IActionResult AddOrEdit(string? id)
		{
			var person = new Person();
			if(!string.IsNullOrWhiteSpace(id))
			{
				//person = _context.Person.Find(id);   // id primary key hole fast kaj kore i.e. primary key 1 ta hole find()
				person = _context.Person.Where(x => x.Id == id).FirstOrDefault();
				//multiple key hole where use kora jay.First() data null asle ta exception e chole jabe
			}
			return View(person);
		}

		[HttpPost]
		public IActionResult AddOrEdit(Person person)
		{
			if (ModelState.IsValid)
			{
				bool CanSave = false;
				if (person.Id == Guid.Empty.ToString())
				{
					person.Id = Guid.NewGuid().ToString();
					_context.Person.Add(person);
					CanSave = true;

				}
				else
				{
					var personEntity = _context.Person.Where(p => p.RowVersion == person.RowVersion && p.Id == person.Id).FirstOrDefault();

					if (personEntity != null)
					{
						personEntity.Name = person.Name;
						personEntity.Gender = person.Gender;
						personEntity.Height = person.Height;
						personEntity.Weight = person.Weight;
						personEntity.DOB = person.DOB;
						personEntity.HairColor = person.HairColor;
						personEntity.PresentAddress = person.PresentAddress;
						personEntity.PermanentAddress = person.PermanentAddress;

						_context.Entry(personEntity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
						// person edit or modify korle
						// eta update er command
						CanSave = true;


					}
					else
					{
						ViewData["ErrorMessage"] = "Data not found or edit restrictted ";
						return View(person);

					}
				}

				if (CanSave)
				{
						_context.SaveChanges();
					//database e save korar jonno
				}

				//var personEntity = _context.Person.Find(person.Id);
			
				//_context.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(e => e.Errors).Select(s => s.ErrorMessage));
				//model er error message gulo collect kore errorMessage e rakhbe
				ViewData["ErrorMessage"] = errorMessage;

			}

			return View(person);
		}


		public IActionResult Delete(string? id) 
		{ 
			if(!string.IsNullOrWhiteSpace(id))
			{
				//var person = _context.Person.Find(id); //nicher tao use kora jay
				var person = _context.Person.Where(x => x.Id ==id).FirstOrDefault();
				if (person != null)
				{

					_context.Person.Remove(person);
					_context.SaveChanges();
				} 
			}
			return RedirectToAction("Index");
		}


	}
}
