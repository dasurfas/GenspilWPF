using System.Collections.Generic;

namespace GenspilWPF.Repositories
{
    // IRepository<T> er et generisk interface, der definerer kontrakten for repositories.
    // Det har to metoder: LoadAll for at loade alle objekter af type T og SaveAll for at gemme en liste.
    internal interface IRepository<T>
    {
        List<T> LoadAll();
        void SaveAll(List<T> items);
    }
}