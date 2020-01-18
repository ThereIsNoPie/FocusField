using System.Linq;
using System.Net.Http;

namespace FocusField.Core.DataProcessor
{
    public interface IUploadService
    {
        void StoreData(FocusData focusData);
    }
}
