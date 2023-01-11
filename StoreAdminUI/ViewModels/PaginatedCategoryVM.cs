using System;
namespace StoreAdminUI.ViewModels
{
	public class PaginatedCategoryVM<T>
	{
        //public PaginatedCategoryVM(List<T> items, int pageIndex, int pageSize, int totalCount)
        //{
        //    Items = items;
        //    PageIndex = pageIndex;
        //    PageSize = pageSize;
        //    TotalPage = (int)Math.Ceiling(totalCount / (double)pageSize);
        //}
        public List<T> Items { get; set; }
        public bool HasNext => PageIndex < TotalPage;
        public bool HasPrevious => PageIndex > 1;
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }
    }
}

