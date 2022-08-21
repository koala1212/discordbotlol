using System.Collections.Generic;

namespace Discord_bot_app.ListViewData.Interfaces;

public interface IListViewPopulate
{
    void Populate<T>(List<T> info, CollectionType collectionName);
    
}