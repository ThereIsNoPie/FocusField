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
}
