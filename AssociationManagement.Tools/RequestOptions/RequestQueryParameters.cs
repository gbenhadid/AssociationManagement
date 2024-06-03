using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace AssociationManagement.Tools.RequestFeatures {
    public class RequestQueryParameters : RequestParameters {
        public string? searchTerm { get; set; }
        public string? GroupBy { get; set; }

        public string? Filtres {
            get => JsonSerializer.Serialize(FilterParameters);
            set {
                FilterParameters = JsonSerializer.Deserialize<List<FilterParameter>>(value ?? string.Empty) ?? new();
            }
        }

        [BindNever]
        public List<FilterParameter>? FilterParameters { get; private set; }


        public string? Sorts {
            get => JsonSerializer.Serialize(SortParameters);
            set {
                SortParameters = JsonSerializer.Deserialize<List<SortParameter>>(value ?? string.Empty) ?? new();
            }
        }


        [BindNever]
        public List<SortParameter>? SortParameters { get; set; }

    }


}
