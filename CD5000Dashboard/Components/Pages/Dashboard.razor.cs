namespace CD5000Dashboard.Components.Pages
{
    public partial class Dashboard
    {
        private bool _loading = true;
        private int _transactionCount;
        private string? _errorMessage;
        private IEnumerable<Data.DashboardRepository.ProductHourlyStat>? _hourlyData;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _transactionCount = await Repository.GetTransactionCountAsync();
                _hourlyData = await Repository.GetTransactionsByHourAsync();
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            finally
            {
                _loading = false;
            }
        }
    }
}
