using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Models;
using WpfApp1;

namespace WpfApp1Services
{
    public interface IDatabaseService
    {
        List<Category> GetCategories();
        void AddCategory(Category category);
        void DeleteCategory(string categoryName);
        void UpdateCategory(string oldName, string newName);
        void SaveExpense(Expense expense);
        List<Expense> GetExpensesByDateRange(DateTime startDate, DateTime endDate);
    }

    public interface IAppDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Expense> Expenses { get; set; }
        int SaveChanges();
    }

    public class DatabaseService : IDatabaseService
    { 
        private readonly IAppDbContext _context;
        public DatabaseService(IAppDbContext context)
        {
            _context = context;
        }  
        public List<Category> GetCategories()       
        {        
            return _context.Categories.ToList();            
        }        
        public void AddCategory(Category category)       
        {       
            _context.Categories.Add(category);
            _context.SaveChanges();
        }       
       public void DeleteCategory(string categoryName)        
        {       
            var category = _context.Categories.FirstOrDefault(c => c.Name == categoryName);            
            if (category != null)           
            {           
                _context.Categories.Remove(category);                
                _context.SaveChanges();               
            }           
        }       
        public void UpdateCategory(string oldName, string newName)
        {       
            var category = _context.Categories.FirstOrDefault(c => c.Name == oldName);          
            if (category != null)        
            {           
                category.Name = newName;
                _context.SaveChanges();
            }
        }
        public void SaveExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
        }
        public List<Expense> GetExpensesByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.Expenses
                .Where(expense => expense.ExpenseDate >= startDate && expense.ExpenseDate < endDate)
                .Include(expense => expense.Categoryy)
                .ToList();
        }

        
    }
}




