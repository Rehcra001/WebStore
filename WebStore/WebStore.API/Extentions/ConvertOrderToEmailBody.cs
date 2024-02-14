using WebStore.DTO;

namespace WebStore.API.Extentions
{
    public static class ConvertOrderToEmailBody
    {
        // TODO add code to convert order to html format for email body
        public static EmailDTO ConvertToEmailBody(this OrderDTO orderDTO)
        {
            EmailDTO email = new EmailDTO();
            string logo = @"<img src=""cid:{0}""/>";
            string body = "";
            body += $"<div style=\"font-size: 2rem; \">{logo}&nbsp;&nbsp; Web Store</div>";
            body += "<div style=\"font-family: Arial, Helvetica, sans-serif; font-size: 1rem;text-align: left; padding: 5px 10px;\">"; //open body
            body += $"<div style=\"font-size: 1.1rem; \"> To: {orderDTO.FirstName} {orderDTO.LastName}</div>";
            body += "<div style=\"font-size: 1rem;\"> <p>Thank you for your order.</p></div>";

            //Shipping Address
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

            //Order Items
            //Head
            body += $"<table>";
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
            body += $"";
            body += $"";
            body += $"";
            body += $"";


            body += "</div>";//close Body
            email.Body = body;

            return email;
        }
    }
}
