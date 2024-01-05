using System.Data;
using WebStore.Models;

namespace WebStore.Repository.Static
{
    public static class Helper
    {
        internal static DataTable CreateAddressesTable(IEnumerable<AddressModel> addressList)
        {
            DataTable addresses = new DataTable();
            addresses.Columns.Add(nameof(AddressModel.AddressLine1), typeof(string));
            addresses.Columns.Add(nameof(AddressModel.AddressLine2), typeof(string));
            addresses.Columns.Add(nameof(AddressModel.Suburb), typeof(string));
            addresses.Columns.Add(nameof(AddressModel.City), typeof(string));
            addresses.Columns.Add(nameof(AddressModel.PostalCode), typeof(string));
            addresses.Columns.Add(nameof(AddressModel.Country), typeof(string));

            foreach (AddressModel address in addressList)
            {
                DataRow row = addresses.NewRow();
                row[nameof(AddressModel.AddressLine1)] = address.AddressLine1;
                row[nameof(AddressModel.AddressLine2)] = address.AddressLine2;
                row[nameof(AddressModel.Suburb)] = address.Suburb;
                row[nameof(AddressModel.City)] = address.City;
                row[nameof(AddressModel.PostalCode)] = address.PostalCode;
                row[nameof(AddressModel.Country)] = address.Country;

                addresses.Rows.Add(row);
            }

            return addresses;
        }
    }
}
