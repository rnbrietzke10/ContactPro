using ContactPro.Data;
using ContactPro.Models;
using ContactPro.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ContactPro.Services
{
	public class AddressBookService : IAddressBookService
	{
		private readonly ApplicationDbContext _context;

		public AddressBookService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task AddContactToCategoryAsync(int catgoryId, int contactId)
		{
			try
			{
				// check to see if category is in the contact
				if(!await IsContactInCategory(catgoryId, contactId))
				{
					Contact? contact = await _context.Contacts.FindAsync(contactId);
					Category? category = await _context.Categories.FindAsync(catgoryId);

					if(category != null && contact != null) 
					{ 
						category.Contacts.Add(contact);
						await _context.SaveChangesAsync();
					}
				}
			}catch (Exception)
			{
				throw;
			}
		}

		public Task<ICollection<Category>> GetContactCategoriesAsync(int contactId)
		{
			throw new NotImplementedException();
		}

		public async Task<ICollection<int>> GetContactCategoryIdsAsync(int contactId)
		{
			try
			{
				var contact = await _context.Contacts.Include(c => c.Categories)
													 .FirstOrDefaultAsync(c => c.Id == contactId);
				
				List<int> categoryIds = contact!.Categories.Select(c => c.Id).ToList();
				return categoryIds;
			} 
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<IEnumerable<Category>> GetUserCategoriesAsync(string userId)
		{
			List<Category> categories = new List<Category>();
			try
			{
				categories = await _context.Categories.Where(c => c.AppUserId == userId).OrderBy(c => c.Name).ToListAsync();
			} catch (Exception)
			{
				throw;
			}
			return categories;
		}

		public async Task<bool> IsContactInCategory(int catgoryId, int contactId)
		{
			Contact? contact = await _context.Contacts.FindAsync(contactId);

			return await _context.Categories.Include(c => c.Contacts).Where(c => c.Id == catgoryId && c.Contacts.Contains(contact)).AnyAsync();
		}

		public Task RemoveContactFromCategoryAsync(int catgoryId, int contactId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Contact> SearchForContacts(string searchString, string userId)
		{
			throw new NotImplementedException();
		}
	}
}
