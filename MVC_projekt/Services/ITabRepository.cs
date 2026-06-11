using MVC_projekt.Models;

namespace MVC_projekt.Services
{
    public interface ITabRepository
    {
        List<Tab> GetAllTabs();
        List<Tab> GetTabsByCreator(int creatorId);
        Tab GetTabById(int id);
        void AddTab(Tab tab);
        void UpdateTab(Tab tab);
        void DeleteTabById(int id);
    }
}