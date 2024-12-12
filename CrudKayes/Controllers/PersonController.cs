
using Microsoft.Extensions.Caching.Memory;
using System.Net.WebSockets;

namespace CrudKayes.Controllers
{
	public class PersonController : BaseController
	{
		private static MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
		string CacheName = "personCache";
		private readonly CrudDbContext _context;


        public PersonController(CrudDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
			if(memoryCache.TryGetValue(CacheName, out var result))
			{
				return View(result);
			}
			//var DataSet = _context.Person;// database e hit korbe na. onlu query ta generate korbe. database e execute hobe na
			var DataSet = _context.Person.ToList(); // ToList command dile sathe sathe database e chole jabe.hit korbe database e
			//var DataSet = _context.Person.FirstAsync(); // Async ta slow process e kaj kore. data volume beshi hole FirstAsync use korbo sathe await use kora lagbe.
			foreach(var item in DataSet)
			{
				item.FullAddress = item.PresentAddress + "###" + item.PermanentAddress;
			}

			memoryCache.Set(CacheName, DataSet, TimeSpan.FromSeconds(10));
			// 10 seconds er majhe request asle if block thekei result return korbe r 10seconds over hoye gele abar database theke data niye asbe 
			// 10 seconds er vitor request pathale cache theke data dekhabe

			return View(DataSet);
		}

		[HttpGet]
		public IActionResult AddOrEdit(int? id)
		{
			var person = new Person();
			if(id !=null || id != 0)
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
				var personEntity = _context.Person.Find(person.Id);
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

				}
				else
				{
					_context.Person.Add(person);
					// new person asle
				}

				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(person);
		}

		public IActionResult Delete(int? id) 
		{ 
			if(id !=null || id !=0)
			{
				var person = _context.Person.Find(id);
				_context.Person.Remove(person);
				_context.SaveChanges();
			}
			return RedirectToAction("Index");
		}


	}
}
