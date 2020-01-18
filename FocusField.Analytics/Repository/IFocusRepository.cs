using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using CSharpFunctionalExtensions;
using FocusField.Analytics.Models;

namespace FocusField.Analytics.Repository
{
    public interface IFocusRepository
    {
        void SaveData(FocusData data);
        IEnumerable<FocusItem> GetAll();
    }

    public class FocusRepository : IFocusRepository
    {
        private static IEnumerable<FocusItem> _data = new List<FocusItem>();

        public void SaveData(FocusData data)
        {
            if (_data.Any(x => x.ItemId == data.ItemId)) {
                _data.First(x => x.ItemId == data.ItemId).AddAndCombineItem(data);
            }
            else {
                var focusItem = new FocusItem(data.ItemId);
                focusItem.AddAndCombineItem(data);
                _data = _data.Concat(new List<FocusItem>() { focusItem });
            }
        }


        public IEnumerable<FocusItem> GetAll() => _data;
    }
}
