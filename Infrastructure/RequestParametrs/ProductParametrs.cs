namespace recipe_web_api.Infrastructure.RequestParametrs
{
    //ПРОСТО ПРИМЕР ДЛЯ ПАГИНАЦИИ
    public class ProductParametrs: QueryPaginationParameters
    {
        public ProductParametrs(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public ProductParametrs() { }

        public int ProductId { get; set; }
    }
}
