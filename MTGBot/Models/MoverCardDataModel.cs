using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraping.Data_Models
{
   public class MoverCardDataModel
    {
        private string _priceChange;
        private string _name;
        private string _totalPrice;
        private string _changePercentage;

        public string PriceChange
        {
            get { return _priceChange; }
            set { _priceChange = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; }
        }
        public string ChangePercentage
        {
            get { return _changePercentage; }
            set { _changePercentage = value; }
        }
    }

}
