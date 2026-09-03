using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class Customer
    {
        public bool logged_in { get; set; }
        public int successful_purchases { get; set; }
        public int created_at { get; set; }
        public int updated_at { get; set; }
        public int offline_payments { get; set; }
        public int score { get; set; }
    }

    public class LineItemCharge
    {
        public string name { get; set; }
        public string description { get; set; }
        public int unit_price { get; set; }
        public int quantity { get; set; }
        public string sku { get; set; }
        public string category { get; set; }
    }

    public class BillingAddress
    {
        public string street1 { get; set; }
        public string street2 { get; set; }
        public object street3 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string tax_id { get; set; }
        public string company_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

    public class Details
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public Customer customer { get; set; }
        public List<LineItemCharge> line_items { get; set; }
        public BillingAddress billing_address { get; set; }
    }

    public class ConektaCharge
    {
        public string description { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string reference_id { get; set; }
        public string card { get; set; }
        public Details details { get; set; }
        public bool capture { get; set; }
    }

    public class CustomerInfo
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public Antifraud_info antifraud_info { get; set; }
    }

    public class Antifraud_info {
        public int account_created_at { get; set; }
        public int first_paid_at { get; set; }
        public int account_age { get; set; }
        public int paid_transactions { get; set; }
    }

    public class LineItem
    {
        public string name { get; set; }
        public int unit_price { get; set; }
        public int quantity { get; set; }
        public Antifraud_info_lineItem antifraud_info { get; set; }
    }

    public class Antifraud_info_lineItem
    {
        public string trip_id { get; set; }
        public string ticket_class { get; set; }
        public string pickup_latlon { get; set; }
        public string dropoff_latlon { get; set; }
        public int first_paid_at { get; set; }
        public int account_created_at { get; set; }
    }



    public class PaymentMethod
    {
        public string type { get; set; }
        public string token_id { get; set; }
    }

    public partial class Charge
    {
        public PaymentMethod payment_method { get; set; }
    }

    public class Metadata
    {
        public string yes { get; set; }
    }

    public class ConektaOrder
    {
        public string currency { get; set; }
        public CustomerInfo customer_info { get; set; }
        public List<LineItem> line_items { get; set; }
        public List<Charge> charges { get; set; }
        public Metadata metadata { get; set; }
    }

    public class orderAppDTO {
        public string id { get; set; }
        public string payment_status { get; set; }
        public bool hasError { get; set; }
    }
    
}
