using CD5000Dashboard.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace CD5000Dashboard.Components.Pages
{
    public partial class Dashboard
    {
        private bool _loading = true;
        private bool _chartRendered = false;
        private int _transactionCount;
        private string? _errorMessage;

        [Inject] private IJSRuntime JS { get; set; } = default!;

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!_chartRendered && !_loading && _hourlyData.Any())
            {
                var labels = _hourlyData.Select(x => x.Hour).ToArray();
                var data = _hourlyData.Select(x => x.TransactionCount).ToArray();

                await JS.InvokeVoidAsync("renderChart", labels, data);

                _chartRendered = true;
            }
        }
    }
}