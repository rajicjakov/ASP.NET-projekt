using MVC_projekt.Models;

namespace MVC_projekt.Services
{
    public interface ITabRepository
    {
        List<Tab> GetAllTabs();
        Tab GetTabById(int id);
    }
}