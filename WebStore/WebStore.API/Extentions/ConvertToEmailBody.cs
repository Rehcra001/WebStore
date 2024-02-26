using WebStore.DTO;

namespace WebStore.API.Extentions
{
    public static class ConvertToEmailBody
    {

        // TODO add code to convert order to html format for email body
        public static EmailDTO ConvertToOrderEmailBody(this OrderDTO orderDTO, CompanyDetailDTO companyDetailDTO, string message)
        {
            EmailDTO email = new EmailDTO();
            //Header
            string logo = @"<img src=""cid:{0}""/>";
            string body = "";
            body += $"<div style=\"font-size: 2rem; \">{logo}&nbsp;&nbsp; Web Store</div>";
            body += "<div style=\"font-family: Arial, Helvetica, sans-serif; font-size: 1rem;text-align: left; padding: 5px 10px;\">"; //open body
            body += $"<div style=\"font-size: 1.1rem; \"> To: {orderDTO.FirstName} {orderDTO.LastName}</div>";
            body += $"<div style=\"font-size: 1rem;\"> <p>{message}</p></div>";

            //Company Details
            body += "<div>";
            body += "<div style=\"font-weight: bold;\">Company and Payment Details:</div>";
            body += $"<div>Contact us:&nbsp;{companyDetailDTO.EmailAddress}</div>";
            body += $"<div>Reference:&nbsp;{orderDTO.LastName}:{orderDTO.OrderId}</div>";
            body += $"<div>Bank:&nbsp;{companyDetailDTO.CompanyEFT.Bank}</div>";
            body += $"<div>Account Type:&nbsp;{companyDetailDTO.CompanyEFT.AccountType}</div>";
            body += $"<div>Account Number:&nbsp;{companyDetailDTO.CompanyEFT.AccountNumber}</div>";
            body += $"<div>Branch Code:&nbsp;{companyDetailDTO.CompanyEFT.BranchCode}</div>";
            body += $"<div style=\"font-weight: bold; \">Order will be shipped once payment has been confirmed</div>";
            body += "</div><br><br>";

            //Shipping Address
            body += "<div style=\"margin-right: 10px; \">";
            body += "<div style=\"font-weight: bold;\">Ship To:</div>";
            body += $"<div>Order: {orderDTO.OrderId}</div>";
            body += $"<div>{orderDTO.FirstName} {orderDTO.LastName}</div>";
            body += $"<div>{orderDTO.Address.AddressLine1}</div>";
            if (!String.IsNullOrWhiteSpace(orderDTO.Address.AddressLine2))
            {
                body += $"<div>{orderDTO.Address.AddressLine2}</div>";
            }
            body += $"<div>{orderDTO.Address.Suburb}</div>";
            body += $"<div>{orderDTO.Address.City}</div>";
            body += $"<div>{orderDTO.Address.PostalCode}</div>";
            body += $"<div>{orderDTO.Address.Country}</div><br>";
            body += "</div><br>";

            //Order Items
            //Head
            body += $"<table style=\"border: 1px solid black; \">";
            body += $"<thead>";
            body += $"<tr>";
            body += $"<th style=\"text-align: left; padding: 5px 10px;\">Product</th>";
            body += $"<th style=\"text-align: left; padding: 5px 10px;\">Quantity</th>";
            body += $"<th style=\"text-align: left; padding: 5px 10px;\">Price per Unit</th>";
            body += $"<th style=\"text-align: left; padding: 5px 10px;\">Line Price</th>";
            body += $"</tr>";
            body += $"</thead>";

            //Body
            body += $"<tbody>";
            foreach (OrderItemDTO item in orderDTO.OrderItems)
            {
                body += $"<tr>";
                body += $"<td style=\"text-align: left; padding: 5px 10px;\">{item.ProductName}</td>";
                body += $"<td style=\"text-align: left; padding: 5px 10px;\">{item.Quantity}</td>";
                body += $"<td style=\"text-align: left; padding: 5px 10px;\">R{item.Price.ToString("N2")}</td>";
                body += $"<td style=\"text-align: left; padding: 5px 10px;\">R{(item.Quantity * item.Price).ToString("N2")}</td>";
                body += $"</tr>";
            }
            body += $"</tbody>";
            body += $"</table>";

            // Order Totals
            //Head
            body += "<br>";
            body += "<div>Totals:</div>";
            body += $"<table style=\"border: 1px solid black; \">";

            body += $"<thead>";
            body += $"<tr>";
            body += $"<th style=\"text-align: left; padding: 5px 10px;\">Total excl. VAT</th>";
            body += $"<th style=\"text-align: left; padding: 5px 10px;\">VAT 15%</th>";
            body += $"<th style=\"text-align: left; padding: 5px 10px;\">Total incl. VAT</th>";
            body += $"</tr>";
            body += $"</thead>";

            body += "<tbody>";
            body += $"<tr>";
            body += $"<td style=\"text-align: left; padding: 5px 10px;\">R{orderDTO.TotalPrice.ToString("N2")}</td>";
            body += $"<td style=\"text-align: left; padding: 5px 10px;\">R{(orderDTO.TotalPrice * 0.15M).ToString("N2")}</td>";
            body += $"<td style=\"text-align: left; padding: 5px 10px;\">R{(orderDTO.TotalPrice + orderDTO.TotalPrice * 0.15M).ToString("N2")}</td>";
            body += $"</tr>";
            body += "</tbody>";

            body += "</table>";

            body += "<br>";
            body += $"<div style=\"font-weight: bold; \">Regards,</div>";
            body += $" <div style=\"font-weight: bold; \"> {companyDetailDTO.CompanyName}.</div>";
            body += "</div>";//close Body
            email.Body = body;

            return email;
        }
       
    }
}
