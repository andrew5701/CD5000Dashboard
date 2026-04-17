namespace CD5000Dashboard.Components.Pages
{
    public partial class Tables
    {
        private bool _loading = true;
        private string? _errorMessage;

        private List<string> _tables = new();
        private string? _selectedTable;

        private List<Dictionary<string, object?>> _rows = new();
        private List<Dictionary<string, object?>> _filteredRows = new();

        private string _tableSearch = string.Empty;
        private string _rowSearch = string.Empty;
        private int _rowLimit = 100;

        private string TableSearch
        {
            get => _tableSearch;
            set => _tableSearch = value;
        }

        private string RowSearch
        {
            get => _rowSearch;
            set
            {
                _rowSearch = value;
                ApplyRowFilter();
            }
        }

        private IEnumerable<string> FilteredTables =>
            string.IsNullOrWhiteSpace(_tableSearch)
                ? _tables
                : _tables.Where(t => t.Contains(_tableSearch, StringComparison.OrdinalIgnoreCase));

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _tables = (await TableService.GetTableNamesAsync()).ToList();
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

        private async Task LoadTableAsync(string tableName)
        {
            _selectedTable = tableName;
            _rows = await TableService.GetTableRowsAsync(tableName, _rowLimit);
            ApplyRowFilter();
        }

        private async Task ReloadSelectedTableAsync()
        {
            if (string.IsNullOrWhiteSpace(_selectedTable))
                return;

            _rows = await TableService.GetTableRowsAsync(_selectedTable, _rowLimit);
            ApplyRowFilter();
        }

        private void ApplyRowFilter()
        {
            if (string.IsNullOrWhiteSpace(_rowSearch))
            {
                _filteredRows = _rows.ToList();
                return;
            }

            _filteredRows = _rows
                .Where(row => row.Values.Any(v =>
                    v?.ToString()?.Contains(_rowSearch, StringComparison.OrdinalIgnoreCase) == true))
                .ToList();
        }
    }
}