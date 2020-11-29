using System;
using System.Collections.Generic;
using System.Text;
using Vinotronics;

namespace example
{
    public class InventoryResult
    {
        public string SellerID { get; set; }
        public string ItemNumber { get; set; }
        public int AvailableQuantity { get; set; }
        public string Active { get; set; }
        public string SellerPartNumber { get; set; }
        public string ShipByNewegg { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Demo demo = new Demo();

            try
            {

                demo.GetOrderInfo();
                demo.GetOrderStatus();
                demo.GetAddOrderInfo();

                demo.GetCANOrderInfo();
                demo.GetCANOrderStatus(159243598);

                //demo.GetInternationalPrice();
                //demo.UpdateItemlPrice();

                //demo.GetSellerStatusCheck();
                //demo.GetSubcategoryProperties();
                //demo.GetSubcategoryStatus();

                //demo.SubmitFeed();
                //demo.GetFeedStatus();
                //demo.GetFeedResult();

                //demo.GetRMAInfo();
                //demo.GetCourtesyRefundRequestStatus();
                //demo.GetCourtesyRefundInformation();

                //demo.SubmitShippingRequest();
                //demo.GetShippingRequestDetail();
                //demo.VoidShippingRequest();
                //demo.ConfirmShippingRequest();

                //demo.SubmitDailyInventoryReport();
                //demo.SubmitDailyPriceReport();
                //demo.GetReportStatus();
                //demo.GetDailyInventoryReport();

                //demo.VerifyServiceStatus();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exit with error.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine("Exit.");
            //Console.ReadKey();

        }
    }

}
