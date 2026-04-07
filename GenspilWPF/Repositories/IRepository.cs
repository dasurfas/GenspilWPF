using System.Collections.Generic;

namespace GenspilWPF.Repositories
{
    internal interface IRepository<T>
    {
        List<T> LoadAll();
        void SaveAll(List<T> items);
    }
}
