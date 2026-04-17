using CD5000Dashboard.Data.Models;

namespace CD5000Dashboard.Components.Pages
{
    public partial class Dashboard
    {
        private bool _loading = true;
        private int _transactionCount;
        private string? _errorMessage;

        private IEnumerable<HourlyTransactionRow> _hourlyData = Enumerable.Empty<HourlyTransactionRow>();
        private IEnumerable<MissingRfidRow> _missingRfid = Enumerable.Empty<MissingRfidRow>();
        private IEnumerable<AvgDurationByVehicleRow> _avgDurationByVehicle = Enumerable.Empty<AvgDurationByVehicleRow>();
        private IEnumerable<AvgDurationByUnitTrainRow> _avgDurationByUnitTrain = Enumerable.Empty<AvgDurationByUnitTrainRow>();
        private IEnumerable<TopProductRow> _topProducts = Enumerable.Empty<TopProductRow>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _transactionCount = await Repository.GetTransactionCountAsync();
                _hourlyData = await Repository.GetTransactionsByHourAsync();
                _missingRfid = await Repository.GetMissingRfidAsync();
                _avgDurationByVehicle = await Repository.GetAverageDurationByVehicleAsync();
                _avgDurationByUnitTrain = await Repository.GetAverageDurationByUnitTrainAsync();
                _topProducts = await Repository.GetTopProductsAsync();
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