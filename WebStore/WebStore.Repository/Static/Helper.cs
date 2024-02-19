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
                if (String.IsNullOrWhiteSpace(address.AddressLine2) == false)
                {
                    row[nameof(AddressModel.AddressLine2)] = address.AddressLine2;
                }
                else
                {
                    row[nameof(AddressModel.AddressLine2)] = DBNull.Value;
                }                
                row[nameof(AddressModel.Suburb)] = address.Suburb;
                row[nameof(AddressModel.City)] = address.City;
                row[nameof(AddressModel.PostalCode)] = address.PostalCode;
                row[nameof(AddressModel.Country)] = address.Country;

                addresses.Rows.Add(row);
            }

            return addresses;
        }

        internal static DataTable CreateEFTTable(CompanyEFTDetailModel companyEFTDetailModel)
        {
            DataTable eft = new DataTable();
            eft.Columns.Add(nameof(CompanyEFTDetailModel.Bank), typeof(string));
            eft.Columns.Add(nameof(CompanyEFTDetailModel.AccountType), typeof(string));
            eft.Columns.Add(nameof(CompanyEFTDetailModel.AccountNumber), typeof(string));
            eft.Columns.Add(nameof(CompanyEFTDetailModel.BranchCode), typeof(string));

            DataRow row = eft.NewRow();
            row[nameof(CompanyEFTDetailModel.Bank)] = companyEFTDetailModel.Bank;
            row[nameof(CompanyEFTDetailModel.AccountType)] = companyEFTDetailModel.AccountType;
            row[nameof(CompanyEFTDetailModel.AccountNumber)] = companyEFTDetailModel.AccountNumber;
            row[nameof(CompanyEFTDetailModel.BranchCode)] = companyEFTDetailModel.BranchCode;

            eft.Rows.Add(row);

            return eft;
        }

        internal static DataTable CreateOrderQuantityTable(OrderModel orderModel)
        {
            DataTable orderQuantities = new DataTable();
            orderQuantities.Columns.Add(nameof(OrderItemModel.ProductId), typeof(Int32));
            orderQuantities.Columns.Add(nameof(OrderItemModel.Quantity), typeof(Int32));

            foreach (OrderItemModel item in orderModel.OrderItems)
            {
                DataRow row = orderQuantities.NewRow();
                row[nameof(OrderItemModel.ProductId)] = item.ProductId;
                row[nameof(OrderItemModel.Quantity)] = item.Quantity;

                orderQuantities.Rows.Add(row);
            }
            return orderQuantities;
        }
    }
}
